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

        /// <summary>
        /// Creates a new schedule executing once every hour.
        /// </summary>
        /// <param name="service">The service to which the schedule should be added.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static SchedulePart Hourly(this SchedulingService service)
        {
            return service.At("01 * * * *");
        }

        /// <summary>
        /// Creates a new schedule executing once every day.
        /// </summary>
        /// <param name="service">The service to which the schedule should be added.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static SchedulePart Daily(this SchedulingService service)
        {
            return service.At("02 4 * * *");
        }

        /// <summary>
        /// Creates a new schedule executing once every week.
        /// </summary>
        /// <param name="service">The service to which the schedule should be added.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static SchedulePart Weekly(this SchedulingService service)
        {
            return service.At("22 4 * * 0");
        }

        /// <summary>
        /// Creates a new schedule executing once every month.
        /// </summary>
        /// <param name="service">The service to which the schedule should be added.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static SchedulePart Monthly(this SchedulingService service)
        {
            return service.At("42 4 1 * *");
        }
    }
}
