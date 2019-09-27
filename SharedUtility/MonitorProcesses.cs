using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedUtility
{
    class MonitorProcesses
    {
        public TotalCPUCounter cpuCounter;

        public MonitorProcesses(TotalCPUCounter cpuCounter)
        {
            this.cpuCounter = cpuCounter;
        }

        public void Run()
        {
            long workingset_memory = 0, private_memory = 0, pagefile_memory = 0;
            Process[] rubies;
            rubies = Process.GetProcessesByName("ruby");
            for (int i = 0; i < rubies.Count(); i++)
            {
                workingset_memory += rubies[i].WorkingSet64;
                if ((i + 1) % rubies.Count() == 0)
                    Console.Write("\t{0, 8}", workingset_memory / (float)(1024.0 * 1024.0));
                private_memory += rubies[i].PrivateMemorySize64;
                if ((i + 1) % rubies.Count() == 0)
                    Console.Write("\t{0, 8}", private_memory / (float)(1024.0 * 1024.0));
                pagefile_memory += rubies[i].PagedMemorySize64;
                if ((i + 1) % rubies.Count() == 0)
                    Console.Write("\t{0, 8}", pagefile_memory / (float)(1024.0 * 1024.0));
            }
            cpuCounter.GetCPUUsage();
        }
    }
}
