using SharedUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileLoggingBencher
{
    class Program
    {
        private static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private static long BINNUM = 10;

        static void Main(string[] args)
        {
            int rate = 300;
            long totalEvents = 10000;
            long loremIpsumLength = -1;
            string outputFile = "";

            CommandLine.ParserResult<Options> result = CommandLine.Parser.Default.ParseArguments<Options>(args);

            if (result.Tag == CommandLine.ParserResultType.Parsed)
            {
                var parsed = (CommandLine.Parsed<Options>)result;

                rate = Convert.ToInt32(parsed.Value.Rate);
                totalEvents = Convert.ToInt64(parsed.Value.TotalSeconds);
                loremIpsumLength = parsed.Value.LoremIpsumLength;
                outputFile = parsed.Value.OutputFile;

                Console.WriteLine("rate: {0}", rate);
                Console.WriteLine("totalSeconds: {0}", totalEvents);
                Console.WriteLine("loremIpsumLength: {0}", loremIpsumLength);
                Console.WriteLine("outputFile: {0}", outputFile);

                DoBenchMark(rate, totalEvents, loremIpsumLength, outputFile);
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

        private static void DoBenchMark(int rate, long totalEvents, long loremIpsumLength, string outputFile)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Console.WriteLine("events\tWorking Set(MB)\tPrivate Memory(MB)\tPage File(MB)\tTotal CPU Usage");
            TotalCPUCounter counter = new TotalCPUCounter();
            MonitorProcesses monitor = new MonitorProcesses(counter);

            long batchNum = rate / BINNUM;
            long residualNUM = rate % BINNUM;

            for (int i = 0; i < totalEvents ; i++)
            {
                DateTime targetTime = DateTime.Now;
                long currentTime = GetUnixTime(targetTime);

                if (i % 10 == 0)
                {
                    Console.Write(String.Format("{0, 8}", i * 10));
                    Task.Run(() => monitor.Run());
                }

                using (StreamWriter file = new StreamWriter(outputFile, true))
                {
                    for (int j = 0; j < BINNUM; j++)
                    {
                        for (int k = 0; k < batchNum; k++)
                            file.WriteLine("Fourth line: {0}", k);
                        Thread.Sleep(10);
                    }
                    for (int j = 0; j < residualNUM; j++)
                        file.WriteLine("Ya!!!!");
                }
                while (GetUnixTime(DateTime.Now) <= currentTime)
                {
                    Thread.Sleep(1);
                }
            }
            sw.Stop();
            Console.Write(String.Format("{0, 8}", totalEvents));
            monitor.Run();
            Console.WriteLine(String.Format("elapsed time(second): {0}", (float)(sw.ElapsedMilliseconds / 1000.0)));

            Console.WriteLine("Message written to file.");
        }
    }
}
