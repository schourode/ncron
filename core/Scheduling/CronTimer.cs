// Copyright (c) 2007-2008, Joern Schou-Rode <jsr@malamute.dk>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
// 
// * Redistributions of source code must retain the above copyright
//   notice, this list of conditions and the following disclaimer.
// 
// * Redistributions in binary form must reproduce the above copyright
//   notice, this list of conditions and the following disclaimer in the
//   documentation and/or other materials provided with the distribution.
// 
// * Neither the name of the NCron nor the names of its contributors may
//   be used to endorse or promote products derived from this software
//   without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE REGENTS OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

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

                timer = new Timer(TimerCallback);
                ScheduleNextExecution(true);
                return true;
            }
        }

        private void ScheduleNextExecution(bool first)
        {
            next = this.Plan.ComputeNextExecution(first ? DateTime.Now : next, first);
            timer.Change(next.Subtract(DateTime.Now), TimeSpan.FromMilliseconds(-1));
        }

        public bool Stop()
        {
            lock (syncLock)
            {
                if (this.timer == null) return false;

                this.timer.Dispose();
                this.timer = null;
                return true;
            }
        }

        public void Dispose()
        {
            Stop();
        }

        private void TimerCallback(object state)
        {
            ScheduleNextExecution(false);
        }
    }
}
