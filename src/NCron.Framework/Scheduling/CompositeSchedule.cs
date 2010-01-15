/*
 * Copyright 2010 Joern Schou-Rode
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

using System.Collections.Generic;

namespace NCron.Framework.Scheduling
{
    /// <summary>
    /// Implements the <see cref="ISchedule"/> interface over a set of other objects implementing the same interface.
    /// This provides an easy way to combine several schedules into one, without adding overhead.
    /// </summary>
    public class CompositeSchedule : ISchedule
    {
        private readonly IEnumerable<ISchedule> _schedules;

        public CompositeSchedule(IEnumerable<ISchedule> schedules)
        {
            _schedules = schedules;
        }

        public IEnumerable<IScheduleEntry> GetEntries()
        {
            foreach (var schedule in _schedules)
            {
                foreach (var entry in schedule.GetEntries())
                {
                    yield return entry;
                }
            }
        }
    }
}
