/*
 * Copyright 2009, 2010 Joern Schou-Rode
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
using System.Threading;
using C5;
using NCron.Logging;

namespace NCron.Service
{
    public class SchedulingService : IDisposable
    {
        private readonly IPriorityQueue<QueueEntry> _queue;
        private readonly ILogFactory _logFactory;
        private readonly Timer _timer;
        private QueueEntry _head;

        internal SchedulingService()
        {
            _queue = new IntervalHeap<QueueEntry>();
            _timer = new Timer(TimerCallbackHandler);
            _logFactory = new DefaultLogFactory();
        }

        public SchedulePart At(ISchedule schedule)
        {
            var entry = new QueueEntry(schedule, DateTime.Now);
            _queue.Add(entry);

            return new SchedulePart(entry.Jobs);
        }

        internal void Start()
        {
            _head = _queue.DeleteMin();
            TimerCallbackHandler(null);
        }

        internal void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void TimerCallbackHandler(object data)
        {
            if (DateTime.Now >= _head.NextOccurence)
            {
                ThreadPool.QueueUserWorkItem(WaitCallbackHandler, _head);
                _head.Advance();
                _queue.Add(_head);
                _head = _queue.DeleteMin();
                TimerCallbackHandler(null);
            }
            else
            {
                var timeToNext = _head.NextOccurence - DateTime.Now;
                _timer.Change((long) timeToNext.TotalMilliseconds, Timeout.Infinite);
            }
        }

        private void WaitCallbackHandler(object data)
        {
            // Global exception handling is needed within this worker thread.
            // Without this, the application will crash if something (eg a custom ILog) throws.
            try
            {
                var entry = (QueueEntry) data;

                foreach (var job in entry.Jobs.GetInstances())
                {
                    try
                    {
                        ExecuteJob(job);
                    }
                    finally
                    {
                        job.Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                Bootstrap.LogUnhandledException(exception);
            }
        }

        private void ExecuteJob(ICronJob job)
        {
            using (var log = _logFactory.GetLogForJob(job))
            {
                var context = new CronContext(job, log);

                log.Info(() => string.Format("Executing job: {0}", job));

                // This inner try-catch serves to report ICronJob failures to the ILog.
                // Such exceptions are expected, and are thus handled seperately.
                try
                {
                    job.Initialize(context);
                    job.Execute();
                }
                catch (Exception exception)
                {
                    log.Error(() => string.Format("The job \"{0}\" threw an unhandled exception.", job),
                              () => exception);
                }
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
