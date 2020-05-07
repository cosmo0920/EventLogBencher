using SharedUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;

namespace EventLogBencher
{
    class Program
    {
        private static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private static long BINNUM = 10;
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

            Console.WriteLine("events\tWorking Set(MB)\tPrivate Memory(MB)\tPage File(MB)\tTotal CPU Usage\t%Disk Time");
            TotalCPUCounter cpuCounter = new TotalCPUCounter();
            DiskUsageCounter diskCounter = new DiskUsageCounter();
            MonitorProcesses monitor = new MonitorProcesses(cpuCounter, diskCounter);
            for (int i = 0; i < totalEvents / 10; i++)
            {
                if (i % 10 == 0)
                {
                    Console.Write(String.Format("{0, 8}", i * 10));
                    Task.Run(() => monitor.Run());
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
            monitor.Run();
            Console.WriteLine(String.Format("Flow rate: {0} events per second.", totalEvents / (float)(sw.ElapsedMilliseconds / 1000.0)));

            Console.WriteLine("Message written to event log.");
        }

        public static long GetUnixTime(DateTime targetTime)
        {
            targetTime = targetTime.ToUniversalTime();
            TimeSpan elapsedTime = targetTime - UNIX_EPOCH;

            return (long)elapsedTime.TotalSeconds;
        }

        static void DoBenchmarkBatchedLoremIpsum(EventLog benchLog, long batchSize, long totalSteps, long loremIpsumLength)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //  loremIpsumLength should be less equal than 65535.
            loremIpsumLength = loremIpsumLength > 65535 ? 65535 : loremIpsumLength;

            Console.WriteLine("events\tWorking Set(MB)\tPrivate Memory(MB)\tPage File(MB)\tTotal CPU Usage\tDisk Time");
            TotalCPUCounter cpuCounter = new TotalCPUCounter();
            var text = LoremIpsum.ASCIIText();
            Encoding e = System.Text.Encoding.GetEncoding("UTF-8");
            string result = new String(text.TakeWhile((c, i) => e.GetByteCount(text.Substring(0, i + 1)) <= loremIpsumLength).ToArray());
            DiskUsageCounter diskCounter = new DiskUsageCounter();
            MonitorProcesses monitor = new MonitorProcesses(cpuCounter, diskCounter);

            long batchNum = batchSize / BINNUM;
            long residualNUM = batchSize % BINNUM;

            for (int i = 0; i < totalSteps; i++)
            {
                DateTime targetTime = DateTime.Now;
                long currentTime = GetUnixTime(targetTime);

                Console.Write(String.Format("{0, 8}", i * batchSize));
                Task.Run(() => monitor.Run());

                for (int j = 0; j < BINNUM; j++)
                {
                    for (int k = 0; k < batchNum; k++)
                        // Write an informational entry to the event log.
                        benchLog.WriteEntry(result);
                    //Thread.Sleep(10);
                }
                for (int j = 0; j < residualNUM; j++)
                    benchLog.WriteEntry(result);

                while (GetUnixTime(DateTime.Now) <= currentTime)
                {
                    Thread.Sleep(1);
                }
            }

            sw.Stop();
            Console.Write(String.Format("{0, 8}", totalSteps * batchSize));
            monitor.Run();
            Console.WriteLine(String.Format("Flow rate: {0} events per second.", (totalSteps  * batchSize) / (float)(sw.ElapsedMilliseconds / 1000.0)));

            Console.WriteLine("Message written to event log.");
        }

        static void DoBenchmarkLoremIpsum(EventLog benchLog, int waitMSec, long totalEvents, long loremIpsumLength)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //  loremIpsumLength should be less equal than 65535.
            loremIpsumLength = loremIpsumLength > 65535 ? 65535 : loremIpsumLength;

            Console.WriteLine("events\tWorking Set(MB)\tPrivate Memory(MB)\tPage File(MB)\tTotal CPU Usage\tDisk Time");
            TotalCPUCounter cpuCounter = new TotalCPUCounter();
            var text = LoremIpsum.ASCIIText();
            Encoding e = System.Text.Encoding.GetEncoding("UTF-8");
            string result = new String(text.TakeWhile((c, i) => e.GetByteCount(text.Substring(0, i + 1)) <= loremIpsumLength).ToArray());
            DiskUsageCounter diskCounter = new DiskUsageCounter();
            MonitorProcesses monitor = new MonitorProcesses(cpuCounter, diskCounter);
            for (int i = 0; i < totalEvents / 10; i++)
            {
                if (i % 10 == 0)
                {
                    Console.Write(String.Format("{0, 8}", i * 10));
                    Task.Run(() => monitor.Run());
                }

                // Write an informational entry to the event log.    
                benchLog.WriteEntry(result);
                benchLog.WriteEntry(result);
                benchLog.WriteEntry(result);
                benchLog.WriteEntry(result);
                benchLog.WriteEntry(result);
                benchLog.WriteEntry(result);
                benchLog.WriteEntry(result);
                benchLog.WriteEntry(result);
                benchLog.WriteEntry(result);
                benchLog.WriteEntry(result);
                Thread.Sleep(waitMSec);
            }
            sw.Stop();
            Console.Write(String.Format("{0, 8}", totalEvents));
            monitor.Run();
            Console.WriteLine(String.Format("Flow rate: {0} events per second.", totalEvents / (float)(sw.ElapsedMilliseconds / 1000.0)));

            Console.WriteLine("Message written to event log.");
        }

        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<WaitBenchOptions, BatchBenchOptions>(args)
                .MapResult(
                    (WaitBenchOptions opts) => RunWaitBench(opts),
                    (BatchBenchOptions opts) => RunBatchBench(opts),
                    errs => 1);
        }

        public static int RunWaitBench(WaitBenchOptions opts) {
            int waitMsec = Convert.ToInt32(opts.WaitMSec);
            long totalEvents = Convert.ToInt64(opts.TotalEvents);
            long loremIpsumLength = opts.LoremIpsumLength;

            Console.WriteLine("waitMSec: {0}", waitMsec);
            Console.WriteLine("totalEvents: {0}", totalEvents);
            Console.WriteLine("loremIpsumLength: {0}", loremIpsumLength);
            DoBenchMark(waitMsec, totalEvents, loremIpsumLength);

            return 0;
        }

        public static int RunBatchBench(BatchBenchOptions opts)
        {
            long batchSize = opts.BatchSize;
            long totalSteps = opts.TotalSteps;
            long loremIpsumLength = opts.LoremIpsumLength;

            Console.WriteLine("batchSize: {0}", batchSize);
            Console.WriteLine("totalEvents: {0}", totalSteps);
            Console.WriteLine("loremIpsumLength: {0}", loremIpsumLength);
            DoBatchedBenchMark(batchSize, totalSteps, loremIpsumLength);

            return 0;
        }

        static void DoBatchedBenchMark(long batchSize, long totalEvents, long loremIpsumLength)
        {
            CheckChannelExistence();

            // Create an EventLog instance and assign its source.
            EventLog benchLog = new EventLog { Source = "FluentBench" };

            if (loremIpsumLength > 0)
            {
                DoBenchmarkBatchedLoremIpsum(benchLog, batchSize, totalEvents, loremIpsumLength);
            }
            else
            {
                Console.WriteLine("\nPlease check correct arguments!\nNot Implemented this condition branch.");
            }

        }

        static void DoBenchMark(int waitMSec, long totalEvents, long loremIpsumLength) {
            CheckChannelExistence();

            // Create an EventLog instance and assign its source.
            EventLog benchLog = new EventLog { Source = "FluentBench" };

            if (loremIpsumLength > 0)
            {
                DoBenchmarkLoremIpsum(benchLog, waitMSec, totalEvents, loremIpsumLength);
            } else {
                DoBenchmark(benchLog, waitMSec, totalEvents);
            }
        }
    }
}
