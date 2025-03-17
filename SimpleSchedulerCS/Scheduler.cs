using System;

namespace SimpleSchedulerCS
{
    public class Scheduler : IScheduler
    {
        private readonly Object padlock = new();
        private readonly Dictionary<String, IJob> jobs = new();

        public Scheduler()
        {
        }

        public void AddJob<T>(String jobName, T worker) where T : IWorker
        {
            lock (this.padlock)
            {
                IJob job = new Job(worker);
                this.jobs.TryAdd(jobName, job);
            }
        }

        public void RemoveJob(String jobName)
        {
            lock (this.padlock)
            {
                IJob job;
                if (this.jobs.TryGetValue(jobName, out job))
                {
                    job.Stop();
                }

                this.jobs.Remove(jobName);
            }
        }

        public void RemoveAllJobs()
        {
            lock (this.padlock)
            {
                foreach (KeyValuePair<String, IJob> job in this.jobs)
                {
                    job.Value.Stop();
                }

                this.jobs.Clear();
            }
        }

        public void AddTrigger(String jobName, String triggerName, UInt16 intervalSec)
        {
            lock (this.padlock)
            {
                IJob job;
                if (this.jobs.TryGetValue(jobName, out job))
                {
                    job.AddTrigger(triggerName, intervalSec);
                }
            }
        }

        public void RemoveTrigger(String jobName, String triggerName)
        {
            lock (this.padlock)
            {
                IJob job;
                if (this.jobs.TryGetValue(jobName, out job))
                {
                    job.RemoveTrigger(triggerName);
                }
            }
        }

        public void RemoveAllTriggers(String jobName)
        {
            lock (this.padlock)
            {
                IJob job;
                if (this.jobs.TryGetValue(jobName, out job))
                {
                    job.RemoveAllTriggers();
                }
            }
        }

        public void PauseTrigger(String jobName, String triggerName)
        {
            lock (this.padlock)
            {
                IJob job;
                if (this.jobs.TryGetValue(jobName, out job))
                {
                    job.PauseTrigger(triggerName);
                }
            }
        }

        public void PauseAllTriggers(String jobName)
        {
            lock (this.padlock)
            {
                IJob job;
                if (this.jobs.TryGetValue(jobName, out job))
                {
                    job.PauseAllTriggers();
                }
            }
        }

        public void ResumeTrigger(String jobName, String triggerName)
        {
            lock (this.padlock)
            {
                IJob job;
                if (this.jobs.TryGetValue(jobName, out job))
                {
                    job.ResumeTrigger(triggerName);
                }
            }
        }

        public void ResumeAllTriggers(String jobName)
        {
            lock (this.padlock)
            {
                IJob job;
                if (this.jobs.TryGetValue(jobName, out job))
                {
                    job.ResumeAllTriggers();
                }
            }
        }

        public void ChangeTriggerInterval(String jobName, String triggerName, UInt16 interval)
        {
            lock (this.padlock)
            {
                IJob job;
                if (this.jobs.TryGetValue(jobName, out job))
                {
                    job.ChangeTriggerInterval(triggerName, interval);
                }
            }
        }

        public void ChangeAllTriggersInterval(String jobName, UInt16 interval)
        {
            lock (this.padlock)
            {
                IJob job;
                if (this.jobs.TryGetValue(jobName, out job))
                {
                    job.ChangeAllTriggersInterval(interval);
                }
            }
        }

        public void StartAllJobs()
        {
            lock (this.padlock)
            {
                foreach (KeyValuePair<String, IJob> job in this.jobs)
                {
                    job.Value.Start();
                }
            }
        }

        public void StopAllJobs()
        {
            lock (this.padlock)
            {
                foreach (KeyValuePair<String, IJob> job in this.jobs)
                {
                    job.Value.Stop();
                }
            }
        }
    }
}