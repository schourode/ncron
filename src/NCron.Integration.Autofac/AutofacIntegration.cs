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

using Autofac;
using NCron.Fluent;

namespace NCron.Integration.Autofac
{
    /// <summary>
    /// Provides extension methods on the fluent API, allowing job registrations using Autofac as resolving container.
    /// </summary>
    public static class AutofacIntegration
    {
        /// <summary>
        /// Gets or sets the root container from which nested containers are created for each job execution.
        /// This property should usually only be set once, and its value is never disposed.
        /// </summary>
        public static IContainer RootContainer { private get; set; }

        /// <summary>
        /// Registers a job to be executed using Autofac as resolving container.
        /// </summary>
        /// <typeparam name="TJob">The type of job to be registered. Must implement <see cref="ICronJob"/>.</typeparam>
        /// <param name="part">The fluent part upon which the job is to be registered.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static JobPart Run<TJob>(this SchedulePart part)
            where TJob : ICronJob
        {
            return part.With(() => RootContainer.CreateInnerContainer()).Run(c => c.Resolve<TJob>());
        }

        /// <summary>
        /// Registers a job to be executed using Autofac as resolving container.
        /// </summary>
        /// <param name="part">The fluent part upon which the job is to be registered.</param>
        /// <param name="serviceName">The name used when registering the component with Autofac.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static JobPart Run(this SchedulePart part, string serviceName)
        {
            return part.With(() => RootContainer.CreateInnerContainer()).Run(c => c.Resolve<ICronJob>(serviceName));
        }
    }
}
