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

using NCron.Service;
using NCrontab;

namespace NCron.Fluent.Crontab
{
    /// <summary>
    /// Provides extension methods on the fluent API, allowing easy use of crontab expressions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Creates a new schedule based on a crontab expression (eg: 0 0 * * *).
        /// Please refer to the project wiki for examples.
        /// </summary>
        /// <param name="service">The service to which the schedule should be added.</param>
        /// <param name="crontab">A crontab expression describing the schedule.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static SchedulePart At(this SchedulingService service, string crontab)
        {
            var schedule = CrontabSchedule.Parse(crontab);
            return service.At(schedule.GetNextOccurrence);
        }
    }
}
