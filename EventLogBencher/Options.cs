using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventLogBencher
{
    class Options
    {
        [CommandLine.Option('w', "wait-msec", Required = true, HelpText = "ループで待つミリ秒")]
        public string WaitMSec
        {
            get;
            set;
        }

        [CommandLine.Option('t', "total-events", Required = true, HelpText = "出力するイベントの総数")]
        public string TotalEvents
        {
            get;
            set;
        }
    }
}
