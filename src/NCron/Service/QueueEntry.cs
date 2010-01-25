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

namespace NCron.Service
{
    internal class QueueEntry : IComparable<QueueEntry>
    {
        private readonly ISchedule _schedule;

        public DateTime NextOccurence { get; private set; }

        public Action<Action<ICronJob>> ExecuteCallback { internal get; set; }

        public QueueEntry(ISchedule schedule, DateTime baseTime)
        {
            _schedule = schedule;
            NextOccurence = schedule.GetNextOccurrence(baseTime);
        }

        public void Advance()
        {
            NextOccurence = _schedule.GetNextOccurrence(NextOccurence);
        }

        public int CompareTo(QueueEntry other)
        {
            return NextOccurence.CompareTo(other.NextOccurence);
        }
    }
}
