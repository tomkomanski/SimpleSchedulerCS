using System;

namespace SimpleSchedulerCS
{
    internal interface IJob
    {
        void Start();
        void Stop();
        void AddTrigger(String triggerName, UInt16 intervalSec);
        void RemoveTrigger(String triggerName);
        void RemoveAllTriggers();
        void PauseTrigger(String triggerName);
        void PauseAllTriggers();
        void ResumeTrigger(String triggerName);
        void ResumeAllTriggers();
        void ChangeTriggerInterval(String triggerName, UInt16 interval);
        void ChangeAllTriggersInterval(UInt16 interval);
    }
}