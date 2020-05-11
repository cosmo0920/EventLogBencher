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
            long loggingSteps = 50;
            int parameterLength = -1;
            string outputFile = "";

            CommandLine.ParserResult<Options> result = CommandLine.Parser.Default.ParseArguments<Options>(args);

            if (result.Tag == CommandLine.ParserResultType.Parsed)
            {
                var parsed = (CommandLine.Parsed<Options>)result;

                rate = Convert.ToInt32(parsed.Value.Rate);
                loggingSteps = Convert.ToInt64(parsed.Value.TotalSeconds);
                parameterLength = parsed.Value.ParameterLength;
                outputFile = parsed.Value.OutputFile;

                Console.WriteLine("rate: {0}", rate);
                Console.WriteLine("loggingSteps: {0}", loggingSteps);
                Console.WriteLine("parameterLength: {0}", parameterLength);
                Console.WriteLine("outputFile: {0}", outputFile);

                DoBenchMark(rate, loggingSteps, parameterLength, outputFile);
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

        private static void DoBenchMark(int rate, long loggingSteps, int optionLength, string outputFile)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Console.WriteLine("steps\tWorking Set(MB)\tPrivate Memory(MB)\tPage File(MB)\tTotal CPU Usage\tDisk Time");
            TotalCPUCounter cpuCounter = new TotalCPUCounter();
            DiskUsageCounter diskCounter = new DiskUsageCounter();
            MonitorProcesses monitor = new MonitorProcesses(cpuCounter, diskCounter);

            long batchNum = rate / BINNUM;
            long residualNUM = rate % BINNUM;
            Generator generator = new Generator(optionLength);

            for (int i = 0; i < loggingSteps ; i++)
            {
                DateTime targetTime = DateTime.Now;
                long currentTime = GetUnixTime(targetTime);

                Console.Write(String.Format("{0, 8}", i));
                Task.Run(() => monitor.Run());

                using (StreamWriter file = new StreamWriter(outputFile, true))
                {
                    for (int j = 0; j < BINNUM; j++)
                    {
                        for (int k = 0; k < batchNum; k++)
                            file.WriteLine(generator.Run());
                        Thread.Sleep(10);
                    }
                    for (int j = 0; j < residualNUM; j++)
                        file.WriteLine(generator.Run());
                }
                while (GetUnixTime(DateTime.Now) <= currentTime)
                {
                    Thread.Sleep(1);
                }
            }
            sw.Stop();
            Console.Write(String.Format("{0, 8}", loggingSteps));
            monitor.Run();
            Console.WriteLine(String.Format("elapsed time(second): {0}", (float)(sw.ElapsedMilliseconds / 1000.0)));

            Console.WriteLine("Message written to file.");
        }
    }
}
