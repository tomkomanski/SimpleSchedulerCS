using System;

namespace SimpleSchedulerCS
{
    public interface IScheduler
    {
        void AddJob<T>(String jobName, T job) where T : IWorker;
        void RemoveJob(String jobName);
        void RemoveAllJobs();
        void AddTrigger(String jobName, String triggerName, UInt16 intervalSec);
        void RemoveTrigger(String jobName, String triggerName);
        void RemoveAllTriggers(String jobName);
        void PauseTrigger(String jobName, String triggerName);
        void PauseAllTriggers(String jobName);
        void ResumeTrigger(String jobName, String triggerName);
        void ResumeAllTriggers(String jobName);
        void ChangeTriggerInterval(String jobName, String triggerName, UInt16 interval);
        void ChangeAllTriggersInterval(String jobName, UInt16 interval);
        void StartAllJobs();
        void StopAllJobs();
    }
}