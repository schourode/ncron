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

namespace NCron.Fluent.Generics
{
    /// <summary>
    /// Provides extension methods on the fluent API, allowing job registrations by type paramter.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Registers a job for execution using the job type as generic parameter.
        /// </summary>
        /// <typeparam name="TJob">The type of job to be registered. Must implement <see cref="ICronJob"/> and must have a default (parameterless) constructor.</typeparam>
        /// <param name="part">The fluent part upon which the job is to be registered.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static JobPart Run<TJob>(this SchedulePart part)
            where TJob : ICronJob, new()
        {
            return part.Run(() => new TJob());
        }
    }
}
