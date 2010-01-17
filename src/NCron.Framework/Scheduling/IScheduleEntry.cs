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

using System;

namespace NCron.Framework.Scheduling
{
    /// <summary>
    /// Defines the interface of an entry returned by a <see cref="ISchedule"/>.
    /// </summary>
    public interface IScheduleEntry
    {
        /// <summary>
        /// Gets the name of the job to be executed according to the schedule.
        /// </summary>
        string JobName { get; }

        /// <summary>
        /// Calculates the next occurence of the job after a specified base <see cref="DateTime"/>.
        /// </summary>
        /// <param name="baseTime">The starting point for the service or the last invocation of the job.</param>
        /// <returns>The next occurence according to the schedule. This must always be strictly larger than the baseTime parameter.</returns>
        DateTime GetNextOccurrence(DateTime baseTime);
    }
}
