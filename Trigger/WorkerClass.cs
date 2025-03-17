using System;
using SimpleSchedulerCS;

namespace Trigger
{
    public class WorkerClass : IWorker
    {
        private readonly String name = String.Empty;

        public WorkerClass(String name)
        {
            this.name = name;
        }

        public void Run(String parapmeter)
        {
            Console.WriteLine(parapmeter);
        }
    }
}