using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SharedUtility
{
    class DiskUsageCounter
    {
        private PerformanceCounter counter;
        public PerformanceCounter Counter
        {
            get { return counter; }
            private set { counter = value; }
        }

        public DiskUsageCounter()
        {
            var counter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
            counter.NextValue();
            Counter = counter;
        }

        public void GetDiskUsage()
        {
            Console.WriteLine("\t{0, 8}", Counter.NextValue());
        }
    }
}
