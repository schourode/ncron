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
    /// Represents a semi-configured schedule entry (without any job callback defined) in the fluent API.
    /// </summary>
    public class SchedulePart : Part
    {
        internal SchedulePart(SchedulingService service, QueueEntry queueEntry)
            : base(service, queueEntry)
        {
        }

        /// <summary>
        /// Configures the schedule to run a job resolved using a specified closure.
        /// </summary>
        /// <param name="jobCallback">A method to create the <see cref="ICronJob"/> to be executed in the schedule.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public JobPart Run(Func<ICronJob> jobCallback)
        {
            QueueEntry.ExecuteCallback = (a) =>
            {
                using (var job = jobCallback())
                {
                    a(job);
                }
            };

            return new JobPart(Service, QueueEntry);
        }

        /// <summary>
        /// Configures the schedule to resolve its jobs using a specified container.
        /// </summary>
        /// <typeparam name="TContainer">The type of container used for this schedule.</typeparam>
        /// <param name="containerCallback">A method to create a container to be used for job resolving.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public SchedulePart<TContainer> With<TContainer>(Func<TContainer> containerCallback)
            where TContainer : IDisposable
        {
            return new SchedulePart<TContainer>(Service, QueueEntry, containerCallback);
        }
    }

    /// <summary>
    /// Represents a semi-configured schedule entry (without any job callback defined) in the fluent API.
    /// </summary>
    public class SchedulePart<TContainer> : Part
        where TContainer : IDisposable
    {
        private readonly Func<TContainer> _containerCallback;

        internal SchedulePart(SchedulingService service, QueueEntry queueEntry, Func<TContainer> containerCallback)
            : base(service, queueEntry)
        {
            _containerCallback = containerCallback;
        }

        /// <summary>
        /// Configures the schedule to run a job resolved from a container using a specified closure.
        /// </summary>
        /// <param name="jobCallback">A method to create the <see cref="ICronJob"/> to be executed in the schedule.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public JobPart Run(Func<TContainer, ICronJob> jobCallback)
        {
            QueueEntry.ExecuteCallback = (a) =>
            {
                using (var container = _containerCallback())
                using (var job = jobCallback(container))
                {
                    a(job);
                }
            };

            return new JobPart(Service, QueueEntry);
        }
    }
}
