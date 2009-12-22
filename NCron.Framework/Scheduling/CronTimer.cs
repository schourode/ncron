/*
 * Copyright 2008, 2009 Joern Schou-Rode
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Threading;

namespace NCron.Framework.Scheduling
{
    /// <summary>
    /// Provides a mechanism for executing one or more cron jobs in a specific scheduling <see cref="Plan"/>.
    /// </summary>
    public class CronTimer : IDisposable
    {
        private Timer timer;
        private DateTime next;
        private object syncLock = new object();

        /// <summary>
        /// Gets <see cref="Plan"/> (schedule) in which the jobs associated with the timers are executed.
        /// </summary>
        public Plan Plan { get; private set; }

        /// <summary>
        /// Gets the collection of jobs to be executed when the schedule is met.
        /// </summary>
        public ICollection<ICronJob> Jobs { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the timer is currently running.
        /// </summary>
        public bool IsRunning
        {
            get { return this.timer != null; }
        }
        
        /// <summary>
        /// Creates a new instance of the <see cref="CronTimer"/> class with an empty plan and an empty set of jobs.
        /// </summary>
        /// <remarks>
        /// Note that the <see cref="Plan"/> initializes to be empty, and thus the timer will execute every one second.
        /// Add one ore more constraints to the plan to make it execute less frequently.
        /// </remarks>
        public CronTimer()
        {
            this.Jobs = new List<ICronJob>();
            this.Plan = new Plan();
        }

        /// <summary>
        /// Starts the timer. After calling this method <see cref="IsRunning"/> will be true, and all jobs within the plan will be executed whenever the schedule is met.
        /// </summary>
        /// <returns>true if the timer was started by the method call, false if the timer is already running.</returns>
        public bool Start()
        {
            lock (syncLock)
            {
                if (this.timer != null) return false;

                foreach (ICronJob job in this.Jobs)
                {
                    job.Initialize();
                }

                timer = new Timer(TimerCallback);
                ScheduleNextExecution(true);
                return true;
            }
        }

        /// <summary>
        /// Stops the timer. After calling this method <see cref="IsRunning"/> will be false, and no jobs will be triggered by the plan.
        /// </summary>
        /// <returns>true if the timer was stopped by the method call, false if the timer is already stopped.</returns>
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

        /// <summary>
        /// Disposes of the resources used by the <see cref="CronTimer"/>.
        /// </summary>
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
