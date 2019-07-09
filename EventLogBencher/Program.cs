using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

namespace EventLogBencher
{
    class Program
    {
        static void CheckChannelExistence() {
            // Create the source, if it does not already exist.
            if (!EventLog.SourceExists("FluentBench"))
            {
                // An event log source should not be created and immediately used.
                // There is a latency time to enable the source, it should be created
                // prior to executing the application that uses the source.
                // Execute this sample a second time to use the new source.
                EventLog.CreateEventSource("FluentBench", "Benchmark");
                Console.WriteLine("CreatingEventSource");
                Console.WriteLine("Exiting, execute the application a second time to use the source.");
                // The source is created.  Exit the application to allow it to be registered.
                return;
            }
        }

        static void DoBenchmark(EventLog benchLog, int waitMSec, long totalEvents) {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Console.WriteLine("events\tMB\tTotal CPU Usage");
            TotalCPUCounter counter = new TotalCPUCounter();
            for (int i = 0; i < totalEvents / 10; i++)
            {
                if (i % 10 == 0)
                {
                    Console.Write(String.Format("{0, 8}", i * 10));
                    Task.Run(() => MonitorRubyProcesses(counter));
                }

                // Write an informational entry to the event log.    
                benchLog.WriteEntry(String.Format("Writing to event log. {0} times.", i));
                benchLog.WriteEntry("ⒻⓁuenⓉⒹ™");
                benchLog.WriteEntry("日本語による説明");
                benchLog.WriteEntry("สวัสดีจาก Fluentd!");
                benchLog.WriteEntry("Привет, от Fluentd.");
                benchLog.WriteEntry("Γεια σου, από την Fluentd.");
                benchLog.WriteEntry("مرحبًا ، من Fluentd.");
                benchLog.WriteEntry("हाय, Fluentd से!");
                benchLog.WriteEntry("We ❤ Fluentd!(●'◡'●)");
                benchLog.WriteEntry("Logging is fun! 😀😁😍🤩😋");
                Thread.Sleep(waitMSec);
            }
            sw.Stop();
            Console.Write(String.Format("{0, 8}", totalEvents));
            MonitorRubyProcesses(counter);
            Console.WriteLine(String.Format("{0} events per seconds emitted.", totalEvents / (float)(sw.ElapsedMilliseconds / 1000.0)));

            Console.WriteLine("Message written to event log.");
        }

        public static void Main(string[] args)
        {
            int waitMSec = 1 * 1000;
            long totalEvents = 10000;

            CommandLine.ParserResult<Options> result = CommandLine.Parser.Default.ParseArguments<Options>(args);

            if (result.Tag == CommandLine.ParserResultType.Parsed)
            {
                var parsed = (CommandLine.Parsed<Options>)result;

                waitMSec = Convert.ToInt32(parsed.Value.WaitMSec);
                totalEvents = Convert.ToInt64(parsed.Value.TotalEvents);

                Console.WriteLine("waitMSec: {0}", waitMSec);
                Console.WriteLine("totalEvents: {0}", totalEvents);

                DoBenchMark(waitMSec, totalEvents);
            }
            else
            {
                Console.WriteLine("\nPlease check correct arguments!");
            }
        }

        static void DoBenchMark(int waitMSec, long totalEvents) {
            CheckChannelExistence();

            // Create an EventLog instance and assign its source.
            EventLog benchLog = new EventLog { Source = "FluentBench" };

            DoBenchmark(benchLog, waitMSec, totalEvents);
        }

        static void MonitorRubyProcesses(TotalCPUCounter cpuCounter)
        {
            long memory = 0;
            Process[] rubies;
            rubies = Process.GetProcessesByName("ruby");
            for (int i = 0; i < rubies.Count(); i++)
            {
                memory += rubies[i].PrivateMemorySize64;
                if ((i + 1)% rubies.Count() == 0)
                    Console.Write("\t{0, 8}", memory / (float)(1024.0 * 1024.0));
            }
            cpuCounter.GetCPUUsage();
        }
    }
}
