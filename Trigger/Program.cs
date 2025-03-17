using SimpleSchedulerCS;
using Trigger;

Console.WriteLine("Hello, World!");

WorkerClass one = new("one");
WorkerClass two = new("two");

IScheduler scheduler = new Scheduler();
scheduler.AddJob("thread 1", one);
scheduler.AddJob("thread 2", two);

scheduler.AddTrigger("thread 1", "trigger 1", 3);
scheduler.AddTrigger("thread 1", "trigger 2", 1);

scheduler.StartAllJobs();
Thread.Sleep(10000);

scheduler.ChangeTriggerInterval("thread 1", "trigger 1", 1);
Thread.Sleep(10000);
