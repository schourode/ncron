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
using System.ServiceProcess;

namespace NCron.Framework.Scheduling
{
    /// <summary>
    /// Built on top of <see cref="System.ServiceProcess.ServiceBase"/>, this class serves as the cron service entry point.
    /// The service can be installed using classes from the <see cref="System.Configuration.Install"/> namespace.
    /// </summary>
    public class CronService : ServiceBase
    {
        /// <summary>
        /// Gets the collection of <see cref="CronTimer"/>s controlled by this service.
        /// </summary>
        public ICollection<CronTimer> Timers { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="CronService"/> class.
        /// </summary>
        public CronService()
        {
            this.Timers = new List<CronTimer>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CronService"/> class, with a specified inital set of timers.
        /// </summary>
        /// <param name="timers">The <see cref="CronTimer"/> objects to be initially added to the service.</param>
        public CronService(IEnumerable<CronTimer> timers)
        {
            this.Timers = new List<CronTimer>(timers);
        }

        /// <summary>
        /// Executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically).
        /// Specifies actions to take when the service starts (starts all timers).
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            StartAllTimers();
        }

        /// <summary>
        /// Executes when a Stop command is sent to the service by the Service Control Manager (SCM).
        /// Specifies actions to take when a service stops running (stops all timers).
        /// </summary>
        protected override void OnStop()
        {
            StopAllTimers();
        }

        /// <summary>
        /// Executes when a Pause command is sent to the service by the Service Control Manager (SCM).
        /// Specifies actions to take when a service pauses (stops all timers).
        /// </summary>
        protected override void OnPause()
        {
            StopAllTimers();
        }

        /// <summary>
        /// Executes when a Continue command is sent to the service by the Service Control Manager (SCM).
        /// Specifies actions to take when a service resumes normal functioning after being paused (starts all timers).
        /// </summary>
        protected override void OnContinue()
        {
            StartAllTimers();
        }

        /// <summary>
        /// Starts all timers maintained by this cron service.
        /// </summary>
        public void StartAllTimers()
        {
            foreach (CronTimer timer in this.Timers)
            {
                timer.Start();
            }
        }

        /// <summary>
        /// Stops all timers maintained by this cron service.
        /// </summary>
        public void StopAllTimers()
        {
            foreach (CronTimer timer in this.Timers)
            {
                timer.Stop();
            }
        }

        /// <summary>
        /// Disposes of the resources used by the <see cref="CronService"/> and all of its referenced plans.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            StopAllTimers();
            base.Dispose(disposing);
        }
    }
}
