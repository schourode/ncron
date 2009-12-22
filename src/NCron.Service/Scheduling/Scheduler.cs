/*
 * Copyright 2009 Joern Schou-Rode
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
using Autofac;
using C5;
using NCron.Framework;

namespace NCron.Service.Scheduling
{
    internal class Scheduler : IDisposable
    {
        private static readonly TimeSpan NoPeriod = TimeSpan.FromMilliseconds(-1);
        private readonly IPriorityQueue<QueueEntry> _queue; 
        private readonly Timer _timer;
        private readonly IContainer _container;
        private QueueEntry _head;

        public Scheduler(IContainer container)
        {
            _queue = new IntervalHeap<QueueEntry>();
            _timer = new Timer(TimerCallbackHandler);
            _container = container;
        }

        public void Enqueue(QueueEntry entry)
        {
            _queue.Add(entry);
        }

        public void Run()
        {
            _head = _queue.DeleteMin();
            TimerCallbackHandler(null);
        }

        private void TimerCallbackHandler(object data)
        {
            if (DateTime.Now >= _head.NextOccurence)
            {
                ThreadPool.QueueUserWorkItem(WaitCallbackHandler, _head.JobName);

                _head.ComputeNextOccurence();
                _queue.Add(_head);

                _head = _queue.DeleteMin();
                TimerCallbackHandler(null);
            }
            else
            {
                var timeToNext = _head.NextOccurence - DateTime.Now;
                _timer.Change(timeToNext, NoPeriod);
            }
        }

        private void WaitCallbackHandler(object data)
        {
            var jobName = (string)data;

            using (var inner = _container.CreateInnerContainer())
            {
                var job = (ICronJob)inner.Resolve(jobName);
                job.Log = new Logging.DefaultLog(job.GetType());
                job.Execute();
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
