using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileLoggingBencher
{
    class Options
    {
        [CommandLine.Option('r', "rate", Required = true, HelpText = "一秒間に生成するメッセージの数")]
        public string Rate
        {
            get;
            set;
        }

        [CommandLine.Option('t', "total-seconds", Required = true, HelpText = "メッセージの生成秒数")]
        public string TotalSeconds
        {
            get;
            set;
        }

        [CommandLine.Option('o', "output", Required = false, Default="dummy.log", HelpText = "出力するファイルの名前")]
        public string OutputFile
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
