/* Copyright (c) 2008, Joern Schou-Rode <jsr@malamute.dk>

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted, provided that the above
copyright notice and this permission notice appear in all copies.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE. */

using System;
using System.Collections.Generic;
using System.Threading;

namespace NCron.Scheduling
{
    public class CronTimer
    {
        private Timer timer;
        private DateTime next;
        private object syncLock = new object();

        public Plan Plan { get; private set; }
        public ICollection<ICronJob> Jobs { get; private set; }

        public CronTimer()
        {
            this.Jobs = new List<ICronJob>();
            this.Plan = new Plan();
        }

        public bool IsRunning
        {
            get { return this.timer != null; }
        }
        
        public bool Start()
        {
            lock (syncLock)
            {
                if (this.timer != null) return false;

                foreach (ICronJob job in this.Jobs)
                    job.Initialize();

                timer = new Timer(TimerCallback);
                ScheduleNextExecution(true);
                return true;
            }
        }

        public bool Stop()
        {
            lock (syncLock)
            {
                if (this.timer == null) return false;

                foreach (ICronJob job in this.Jobs)
                    job.Dispose();

                this.timer.Dispose();
                this.timer = null;
                return true;
            }
        }

        private void ScheduleNextExecution(bool first)
        {
            DateTime now = DateTime.Now;
            now = now.AddSeconds(-now.Second).AddMilliseconds(-now.Millisecond);

            next = this.Plan.ComputeNextExecution(first ? now : next, first);
            timer.Change(next.Subtract(now), TimeSpan.FromMilliseconds(-1));
        }

        public void Dispose()
        {
            Stop();
        }

        private void TimerCallback(object state)
        {
            double correction = DateTime.Now.Subtract(next).TotalMinutes;
            if (correction < -1 || correction > 1)
            {
                ScheduleNextExecution(true);
            }
            else
            {
                foreach (ICronJob job in this.Jobs)
                    ThreadPool.QueueUserWorkItem(JobExecutionCallback, job);

                ScheduleNextExecution(false);
            }
        }

        private void JobExecutionCallback(object state)
        {
            ICronJob job = (ICronJob)state;
            job.Execute();
        }
    }
}
