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

namespace NCron
{
    /// <summary>
    /// Defines the interface of a factory that allows creation of <see cref="ICronJob"/> instances by name.
    /// This is the interface to implement, for instance, if you need integration with a IoC container.
    /// </summary>
    public interface IJobFactory
    {
        /// <summary>
        /// Gets an <see cref="ICronJob"/> instance from a name specified by a cron schedule.
        /// </summary>
        /// <remarks>
        /// It is for the <see cref="IJobFactory"/> implementation to decide whether a new <see cref="ICronJob"/> instance is created on each invocation,
        /// or if the same object can be returned for several invocations.
        /// </remarks>
        /// <param name="name">The name of the job to be fetched. This could be a type name, but other naming schemes are allowed as well.</param>
        /// <returns>An <see cref="ICronJob"/> instance matching the provided name.</returns>
        /// <exception cref="JobNotFoundException">if no job can be found with the specified name.</exception>
        ICronJob GetJobByName(string name);
    }
}
