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
