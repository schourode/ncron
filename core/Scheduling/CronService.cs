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

namespace NCron.Scheduling
{
    public class CronService : ServiceBase
    {
        public ICollection<CronTimer> Timers { get; private set; }

        public CronService()
        {
            this.Timers = new List<CronTimer>();
        }

        protected override void OnStart(string[] args)
        {
            StartAllTimers();
        }

        protected override void OnStop()
        {
            StopAllTimers();
        }

        protected override void OnPause()
        {
            StopAllTimers();
        }

        protected override void OnContinue()
        {
            StartAllTimers();
        }

        public void StartAllTimers()
        {
            foreach (CronTimer timer in this.Timers)
            {
                timer.Start();
            }
        }

        public void StopAllTimers()
        {
            foreach (CronTimer timer in this.Timers)
            {
                timer.Stop();
            }
        }
    }
}
