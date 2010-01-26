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
using NCron.Service;

namespace NCron.Fluent
{
    /// <summary>
    /// Provides extension methods on the fluent API, allowing fluent schedule registrations.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Adds a new schedule to the service and returns an object that allows a chained fluent job registration.
        /// </summary>
        /// <param name="service">The service to which the schedule should be added.</param>
        /// <param name="schedule">A method for calculating the next occurence from a specified base.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static SchedulePart At(this SchedulingService service, Func<DateTime, DateTime> schedule)
        {
            var entry = service.AddSchedule(schedule);
            return new SchedulePart(service, entry);
        }
    }
}
