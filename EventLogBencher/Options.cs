using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventLogBencher
{
    class Options
    {
        [CommandLine.Option('w', "wait-msec", Required = false, HelpText = "ループで待つミリ秒")]
        public string WaitMSec
        {
            get;
            set;
        }

        [CommandLine.Option('b', "batch-size", Required = false, HelpText = "1ループ当たりの書き込みサイズ")]
        public long BatchSize
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

        [CommandLine.Option('l', "lorem-ipsum-length", Required = false, Default=-1, HelpText = "Lorem Ipsum 文字列長")]
        public long LoremIpsumLength
        {
            get;
            set;
        }
    }
}
