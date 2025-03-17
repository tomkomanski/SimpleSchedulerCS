using System;

namespace SimpleSchedulerCS
{
    internal sealed class Trigger : ITrigger
    {
        private UInt16 interval;
        private DateTime lastShot;
        private Boolean isPaused;

        public DateTime LastShot
        {
            get
            {
                return this.lastShot;
            }
        }

        public Boolean IsPaused
        {
            get
            {
                return this.isPaused;
            }
        }

        public UInt16 Interval
        {
            get
            {
                return this.interval;
            }
        }

        public Trigger(UInt16 interval) 
        {
            this.interval = interval;
            this.lastShot = DateTime.MinValue;
            this.isPaused = false;
        }

        public void SetLastShot(DateTime lastShot)
        {
            this.lastShot = lastShot;
        }

        public void SetIsPaused(Boolean isPaused)
        {
            this.isPaused = isPaused;
        }

        public void SetInterval(UInt16 interval)
        {
            this.interval = interval;
        }
    }
}