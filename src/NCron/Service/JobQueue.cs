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
using C5;
using NCron.Scheduling;

namespace NCron.Service
{
    internal class JobQueue
    {
        private readonly IPriorityQueue<Entry> _queue;

        public Entry Head { get; private set; }

        public JobQueue()
        {
            _queue = new IntervalHeap<Entry>();
            Head = _queue.DeleteMin();
        }

        public void Advance()
        {
            _queue.Add(Head.GetSubsequentEntry());
            Head = _queue.DeleteMin();
        }

        public class Entry : IComparable<Entry>
        {
            private readonly Func<ICronJob> _jobConstructor;
            private readonly IScheduleEntry _schedule;
            private readonly DateTime _nextOccurence;

            public DateTime NextOccurence
            {
                get { return _nextOccurence; }
            }

            public Entry(Func<ICronJob> jobConstructor, IScheduleEntry schedule, DateTime baseTime)
            {
                _jobConstructor = jobConstructor;
                _schedule = schedule;
                _nextOccurence = schedule.GetNextOccurrence(baseTime);
            }

            public int CompareTo(Entry other)
            {
                return NextOccurence.CompareTo(other.NextOccurence);
            }

            public ICronJob GetJobInstance()
            {
                return _jobConstructor();
            }

            public Entry GetSubsequentEntry()
            {
                return new Entry(_jobConstructor, _schedule, _nextOccurence);
            }
        }
    }
}
