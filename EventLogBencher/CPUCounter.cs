using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventLogBencher
{
    class TotalCPUCounter
    {
        private PerformanceCounter counter;
        public PerformanceCounter Counter {
            get { return counter; }
            private set { counter = value; }
        }

        public TotalCPUCounter()
        {
            var counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            counter.NextValue();
            Counter = counter;
        }

        public void GetCPUUsage()
        {
            Console.WriteLine("\t{0, 8}", Counter.NextValue());
        }
    }
}
