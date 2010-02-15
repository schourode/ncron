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
    public class QueueEntry : IComparable<QueueEntry>
    {
        private readonly Func<DateTime, DateTime> _schedule;

        public DateTime NextOccurence { get; private set; }

        public Action<Action<ICronJob>> ExecutionWrapper { get; private set; }

        internal QueueEntry(Func<DateTime, DateTime> schedule, Action<Action<ICronJob>> executionWrapper, DateTime baseTime)
        {
            _schedule = schedule;
            ExecutionWrapper = executionWrapper;
            NextOccurence = schedule(baseTime);
        }

        internal void Advance()
        {
            NextOccurence = _schedule(NextOccurence);
        }

        public int CompareTo(QueueEntry other)
        {
            return NextOccurence.CompareTo(other.NextOccurence);
        }
    }
}
