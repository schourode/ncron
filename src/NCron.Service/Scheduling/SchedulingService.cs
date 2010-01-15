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
using NCron.Framework;
using NCron.Framework.Logging;
using NCron.Framework.Scheduling;

namespace NCron.Service.Scheduling
{
    internal class SchedulingService : IDisposable
    {
        private static readonly TimeSpan NoPeriod = TimeSpan.FromMilliseconds(-1);

        private readonly IJobFactory _jobFactory;
        private readonly ILogFactory _logFactory;
        private readonly JobQueue _queue;
        private readonly Timer _timer;

        public SchedulingService(ISchedule schedule, IJobFactory jobFactory, ILogFactory logFactory)
        {
            _jobFactory = jobFactory;
            _logFactory = logFactory;

            _queue = new JobQueue(schedule);
            _timer = new Timer(TimerCallbackHandler);
        }

        public void Start()
        {
            TimerCallbackHandler(null);
        }

        public void Stop()
        {
            _timer.Dispose();
        }

        private void TimerCallbackHandler(object data)
        {
            if (DateTime.Now >= _queue.Head.NextOccurence)
            {
                ThreadPool.QueueUserWorkItem(WaitCallbackHandler, _queue.Head.JobName);
                _queue.Advance();
                TimerCallbackHandler(null);
            }
            else
            {
                var timeToNext = _queue.Head.NextOccurence - DateTime.Now;
                _timer.Change(timeToNext, NoPeriod);
            }
        }

        private void WaitCallbackHandler(object data)
        {
            var jobName = (string) data;

            using (var job = _jobFactory.GetJobByName(jobName))
            using (var log = _logFactory.GetLogByName(jobName))
            {
                var context = new CronContext(job, log);
                job.Initialize(context);
                job.Execute();
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}

public class TestJob : CronJob
{
    public override void Execute()
    {
        Log.Info(() => GetHashCode().ToString());
    }
}