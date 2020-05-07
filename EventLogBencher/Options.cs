using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventLogBencher
{
    [Verb("wait", HelpText = "Do wait based Windows EventLog writing")]
    class WaitBenchOptions
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

        [CommandLine.Option('l', "lorem-ipsum-length", Required = false, Default = -1, HelpText = "Lorem Ipsum 文字列長")]
        public long LoremIpsumLength
        {
            get;
            set;
        }
    }

    [Verb("batch", HelpText = "Do batch size based Windows EventLog writing")]
    class BatchBenchOptions
    {
        [CommandLine.Option('b', "batch-size", Required = true, HelpText = "1ループ当たりの書き込みサイズ")]
        public long BatchSize
        {
            get;
            set;
        }

        [CommandLine.Option('t', "total-steps", Required = true, HelpText = "出力するイベントの総数")]
        public long TotalSteps
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
