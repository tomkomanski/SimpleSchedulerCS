using System;
using System.Collections.Concurrent;

namespace SimpleSchedulerCS
{
    internal sealed class Job : IJob
    {
        private readonly ConcurrentDictionary<String, ITrigger> triggers = new();
        private readonly IWorker worker;
        private Task task;
        private CancellationTokenSource taskCancellationTokenSource;
        private CancellationToken taskCancellationToken;
        private Boolean isStarted;

        public Job(IWorker worker) 
        {
            this.worker = worker;
        }

        public void Start()
        {
            if (!this.isStarted)
            {
                this.isStarted = true;
                this.taskCancellationTokenSource = new();
                this.taskCancellationToken = this.taskCancellationTokenSource.Token;
                this.task = Task.Run(() => this.RunWorkerLoop(), this.taskCancellationToken);
            }
        }

        public void Stop()
        {
            if (this.taskCancellationTokenSource != null)
            {
                this.taskCancellationTokenSource.Cancel();
            }

            if (this.task != null)
            {
                if (!(this.task.Status == TaskStatus.Canceled))
                {
                    Thread.Sleep(10);
                }
            }

            this.isStarted = false;
        }

        public void AddTrigger(String triggerName, UInt16 intervalSec)
        {
            ITrigger trigger = new Trigger(intervalSec);
            this.triggers.TryAdd(triggerName, trigger);
        }

        public void RemoveTrigger(String triggerName)
        {
            this.triggers.TryRemove(triggerName, out ITrigger trigger);
        }

        public void RemoveAllTriggers()
        {
            this.triggers.Clear();
        }

        public void PauseTrigger(String triggerName)
        {
            if (this.triggers.TryRemove(triggerName, out ITrigger trigger))
            {
                trigger.SetIsPaused(true);
                this.triggers.TryAdd(triggerName, trigger);
            }
        }

        public void PauseAllTriggers()
        {
            foreach (KeyValuePair<String, ITrigger> item in this.triggers)
            {
                if (this.triggers.TryRemove(item.Key, out ITrigger trigger))
                {
                    trigger.SetIsPaused(true);
                    this.triggers.TryAdd(item.Key, trigger);
                }
            }
        }

        public void ResumeTrigger(String triggerName)
        {
            if (this.triggers.TryRemove(triggerName, out ITrigger trigger))
            {
                trigger.SetIsPaused(false);
                this.triggers.TryAdd(triggerName, trigger);
            }
        }

        public void ResumeAllTriggers()
        {
            foreach (KeyValuePair<String, ITrigger> item in this.triggers)
            {
                if (this.triggers.TryRemove(item.Key, out ITrigger trigger))
                {
                    trigger.SetIsPaused(false);
                    this.triggers.TryAdd(item.Key, trigger);
                }
            }
        }

        public void ChangeTriggerInterval(String triggerName, UInt16 interval)
        {
            if (this.triggers.TryRemove(triggerName, out ITrigger trigger))
            {
                trigger.SetInterval(interval);
                this.triggers.TryAdd(triggerName, trigger);
            }
        }

        public void ChangeAllTriggersInterval(UInt16 interval)
        {
            foreach (KeyValuePair<String, ITrigger> item in this.triggers)
            {
                if (this.triggers.TryRemove(item.Key, out ITrigger trigger))
                {
                    trigger.SetInterval(interval);
                    this.triggers.TryAdd(item.Key, trigger);
                }
            }
        }

        private async Task RunWorkerLoop()
        {
            while (true)
            {
                foreach (KeyValuePair<String, ITrigger> trigger in this.triggers.Where(x => !x.Value.IsPaused))
                {
                    if (this.taskCancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    DateTime start = DateTime.Now;
                    TimeSpan duration = start - trigger.Value.LastShot;

                    if (duration.TotalSeconds >= trigger.Value.Interval)
                    {
                        trigger.Value.SetLastShot(start);
                        this.worker.Run($"{trigger.Key} {DateTime.Now}");
                    }
                }

                Int32 closestShotMilliseconds = Int32.MaxValue;

                while (true)
                {
                    if (this.taskCancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    foreach (KeyValuePair<String, ITrigger> trigger in this.triggers.Where(x => !x.Value.IsPaused))
                    {
                        DateTime start = DateTime.Now;
                        TimeSpan duration = start - trigger.Value.LastShot;

                        if (duration.TotalSeconds >= trigger.Value.Interval)
                        {
                            closestShotMilliseconds = 0;
                        }
                        else
                        {
                            if ((Int32)trigger.Value.Interval * 1000 - (Int32)duration.TotalMilliseconds < closestShotMilliseconds)
                            {
                                closestShotMilliseconds = (Int32)trigger.Value.Interval * 1000 - (Int32)duration.TotalMilliseconds;
                            }
                        }
                    }

                    if (closestShotMilliseconds > 1000)
                    {
                        await Task.Delay(1000, this.taskCancellationToken);
                    }
                    else
                    {
                        await Task.Delay(closestShotMilliseconds, this.taskCancellationToken);
                        break;
                    }
                }
            }
        }
    }
}