using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventLogBencher
{
    class Program
    {
        public static void Main(string[] args)
        {
            int waitMSec = 1 * 1000;

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
            if (args.Length == 1) {
                waitMSec = Convert.ToInt32(args[0]);
            }

            // Create an EventLog instance and assign its source.
            EventLog benchLog = new EventLog { Source = "FluentBench" };

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Console.WriteLine("events\tMB");
            for (int i = 0; i < 1000; i++)
            {
                if (i % 10 == 0)
                {
                    Console.Write(String.Format("{0}", i * 10));
                    Task.Run(() => MonitorRubyProcesses());
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
            Console.Write("10000");
            MonitorRubyProcesses();
            Console.WriteLine(String.Format("{0} events per seconds emitted.", 10000.0 / (float)(sw.ElapsedMilliseconds / 1000.0)));

            Console.WriteLine("Message written to event log.");
        }

        static void MonitorRubyProcesses()
        {
            long memory = 0;
            Process[] rubies;
            rubies = Process.GetProcessesByName("ruby");
            for (int i = 0; i < rubies.Count(); i++)
            {
                memory += rubies[i].PrivateMemorySize64;
                if ((i + 1)% rubies.Count() == 0)
                    Console.WriteLine("\t{0}", memory / (float)(1024.0 * 1024.0));
            }
        }
    }
}
