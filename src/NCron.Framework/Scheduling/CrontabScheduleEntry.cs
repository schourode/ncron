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
using NCrontab;

namespace NCron.Framework.Scheduling
{
    /// <summary>
    /// Implements the <see cref="IScheduleEntry"/> interface using the <see cref="NCrontab.CrontabSchedule"/> class to compute the schedule.
    /// </summary>
    public class CrontabScheduleEntry : IScheduleEntry
    {
        private readonly CrontabSchedule _schedule;
        public string JobName { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="CrontabScheduleEntry"/> class with a specified crontab schedule and job name.
        /// </summary>
        /// <param name="schedule">The crontab schedule in witch the job should be executed.</param>
        /// <param name="jobName">The name of the job to be executed on each occurence.</param>
        public CrontabScheduleEntry(CrontabSchedule schedule, string jobName)
        {
            _schedule = schedule;
            JobName = jobName;
        }

        public DateTime GetNextOccurrence(DateTime baseTime)
        {
            return _schedule.GetNextOccurrence(baseTime);
        }
    }
}
