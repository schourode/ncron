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
    /// <summary>
    /// Executes jobs according to specified schedules.
    /// </summary>
    public class SchedulingService : IDisposable
    {
        private readonly Timer _timer;
        private readonly IPriorityQueue<ScheduledJob> _queue;
        private readonly IDictionary<string, JobExecutionWrapper> _namedEntries;
        private ILogFactory _logFactory;
        private ScheduledJob _head;

        /// <summary>
        /// Sets the log factory that is used to create a log for each job execution.
        /// </summary>
        public ILogFactory LogFactory
        {
            set { _logFactory = value; }
        }

        internal SchedulingService()
        {
            _timer = new Timer(TimerCallbackHandler);
            _queue = new IntervalHeap<ScheduledJob>();
            _namedEntries = new HashDictionary<string, JobExecutionWrapper>();
            _logFactory = new DefaultLogFactory();
        }

        /// <summary>
        /// Adds a scheduled job for automatic execution by this service.
        /// </summary>
        /// <param name="schedule">A method for computation of occurrences in the schedule, taking last execution as parameter.</param>
        /// <param name="executionWrapper">The execution wrapper method to be invoked on each occurence of the job.</param>
        /// <returns>The newly scheduled job.</returns>
        public ScheduledJob AddScheduledJob(Func<DateTime, DateTime> schedule, JobExecutionWrapper executionWrapper)
        {
            var entry = new ScheduledJob(schedule, executionWrapper, DateTime.Now);
            _queue.Add(entry);
            return entry;
        }

        /// <summary>
        /// Adds a named job for manual execution by this service.
        /// </summary>
        /// <param name="name">The name under which the job should be registered.</param>
        /// <param name="executionWrapper">The execution wrapper method to be invoked on each invocation of the job.</param>
        public void AddNamedJob(string name, JobExecutionWrapper executionWrapper)
        {
            _namedEntries.Add(name, executionWrapper);
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
            var waitTime = _head.NextOccurence - DateTime.Now;
            var waitMilliseconds = (long) waitTime.TotalMilliseconds;

            if (waitMilliseconds > 0)
            {
                _timer.Change(waitMilliseconds, Timeout.Infinite);
                return;
            }

            ThreadPool.QueueUserWorkItem(WaitCallbackHandler, _head);
            _head.Advance();
            _queue.Add(_head);
            _head = _queue.DeleteMin();
            TimerCallbackHandler(null);
        }

        private void WaitCallbackHandler(object data)
        {
            // Global exception handling is needed within this worker thread.
            // Without this, the application will crash if something (eg a custom ILog) throws.
            try
            {
                var entry = (ScheduledJob) data;
                entry.ExecutionWrapper(ExecuteJob);
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

                log.Info(() => String.Format("Executing job: {0}", job));

                // This inner try-catch serves to report ICronJob failures to the ILog.
                // Such exceptions are expected, and are thus handled seperately.
                try
                {
                    job.Initialize(context);
                    job.Execute();
                }
                catch (Exception exception)
                {
                    log.Error(() => String.Format("The job \"{0}\" threw an unhandled exception.", job),
                              () => exception);
                }
            }
        }

        internal void ExecuteNamedJob(string name)
        {
            JobExecutionWrapper executionWrapper;

            if (!_namedEntries.Find(name, out executionWrapper))
            {
                throw new ArgumentException(
                    string.Format("No job is registered with the name \"{0}\".", name)
                );
            }

            executionWrapper(ExecuteJob);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
