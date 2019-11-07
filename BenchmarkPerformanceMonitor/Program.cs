using SharedUtility;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BenchmarkPerformanceMonitor
{
    class Program
    {
        private static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        static void Main(string[] args)
        {
            long loggingSteps = 50;

            CommandLine.ParserResult<Options> result = CommandLine.Parser.Default.ParseArguments<Options>(args);

            if (result.Tag == CommandLine.ParserResultType.Parsed)
            {
                var parsed = (CommandLine.Parsed<Options>)result;

                loggingSteps = Convert.ToInt64(parsed.Value.TotalSeconds);

                Console.WriteLine("loggingSteps: {0}", loggingSteps);

                DoMonitor(loggingSteps);
            }
            else
            {
                Console.WriteLine("\nPlease check correct arguments!");
            }
        }

        public static long GetUnixTime(DateTime targetTime)
        {
            targetTime = targetTime.ToUniversalTime();
            TimeSpan elapsedTime = targetTime - UNIX_EPOCH;

            return (long)elapsedTime.TotalSeconds;
        }

        private static void DoMonitor(long loggingSteps)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Console.WriteLine("steps\tWorking Set(MB)\tPrivate Memory(MB)\tPage File(MB)\tTotal CPU Usage\tDisk Time");
            TotalCPUCounter cpuCounter = new TotalCPUCounter();
            DiskUsageCounter diskCounter = new DiskUsageCounter();
            MonitorProcesses monitor = new MonitorProcesses(cpuCounter, diskCounter);

            for (int i = 0; i < loggingSteps; i++)
            {
                DateTime targetTime = DateTime.Now;
                long currentTime = GetUnixTime(targetTime);

                Console.Write(String.Format("{0, 8}", i));
                Task.Run(() => monitor.Run());

                while (GetUnixTime(DateTime.Now) <= currentTime)
                {
                    Thread.Sleep(1);
                }
            }
            sw.Stop();
            Console.Write(String.Format("{0, 8}", loggingSteps));
            monitor.Run();
            Console.WriteLine(String.Format("elapsed time(second): {0}", (float)(sw.ElapsedMilliseconds / 1000.0)));

            Console.WriteLine("Minotoring done.");
        }
    }
}