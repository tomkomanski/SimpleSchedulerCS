using System;

namespace SimpleSchedulerCS
{
    internal interface ITrigger
    {
        UInt16 Interval { get; }
        DateTime LastShot { get; }
        Boolean IsPaused { get; }
        void SetLastShot(DateTime lastShot);
        void SetIsPaused(Boolean isPaused);
        void SetInterval(UInt16 interval);
    }
}