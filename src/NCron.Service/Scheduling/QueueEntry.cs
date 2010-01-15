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
using NCron.Framework.Scheduling;

namespace NCron.Service.Scheduling
{
    internal class QueueEntry : IComparable<QueueEntry>
    {
        private readonly IScheduleEntry _schedule;

        public DateTime NextOccurence { get; private set; }

        public string JobName
        {
            get { return _schedule.JobName; }
        }

        public QueueEntry(IScheduleEntry schedule)
        {
            _schedule = schedule;
            NextOccurence = schedule.GetNextOccurrence(DateTime.Now);
        }

        public int CompareTo(QueueEntry other)
        {
            return NextOccurence.CompareTo(other.NextOccurence);
        }

        public void ComputeNextOccurence()
        {
            var baseTime = DateTime.Now > NextOccurence ? DateTime.Now : NextOccurence;
            NextOccurence = _schedule.GetNextOccurrence(baseTime);
        }
    }
}
