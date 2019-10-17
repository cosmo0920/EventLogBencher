#!/usr/bin/env python3

import psutil
import time
from datetime import datetime
import argparse

parser = argparse.ArgumentParser()
parser.add_argument("steps", help="Total running seconds",
                    type=int)
args = parser.parse_args()

print(f"steps\t{'RSS(MB)':8}\t{'VMS(MB)':8}\t{'Total CPU Usage(%)':8}")
steps = 1
RUBY = "ruby"
rss = 0
vms = 0

while steps <= args.steps:
    currentTime = datetime.now().strftime("%s")
    for proc in psutil.process_iter():
        if proc.name() == RUBY:
            rss = proc.memory_info().rss + rss
            vms = proc.memory_info().vms + vms
    cpu = psutil.cpu_percent(interval=1)
    print(f"{steps}\t{rss /1024/1024 :8}\t{vms /1024/1024 :8}\t{cpu:8}")
    steps = steps + 1
    rss = 0
    while (int(currentTime) >= int(datetime.now().strftime("%s"))):
        time.sleep(0.01)
