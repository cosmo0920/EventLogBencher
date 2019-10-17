using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkPerformanceMonitor
{
    class Options
    {
        [CommandLine.Option('t', "total-seconds", Required = true, HelpText = "実行秒数")]
        public string TotalSeconds
        {
            get;
            set;
        }

        [CommandLine.Option('o', "output", Required = false, Default = "dummy.log", HelpText = "出力するファイルの名前")]
        public string OutputFile
        {
            get;
            set;
        }
    }
}