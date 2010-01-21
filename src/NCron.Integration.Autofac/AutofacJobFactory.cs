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
using Autofac;
using NCron;

namespace NCron.Integration.Autofac
{
    /// <summary>
    /// Implements the <see cref="IJobFactory"/> interface using an Autofac container to resolve jobs.
    /// </summary>
    public abstract class AutofacJobFactory : IJobFactory
    {
        /// <summary>
        /// Gets or sets the container from which jobs are resolved.
        /// </summary>
        protected IContainer Container { get; set; }

        /// <summary>
        /// Gets an <see cref="ICronJob"/> instance from a name specified by a cron schedule.
        /// The job will be resolved by Autofac in a inner container of <see cref="Container"/>.
        /// The inner container will be disposed with the <see cref="ICronJob"/>.
        /// </summary>
        /// <param name="name">The name of the job to be resolved, as specified when registering the job.</param>
        /// <returns>An <see cref="ICronJob"/> instance matching the provided name.</returns>
        /// <exception cref="JobNotFoundException">if no job can be resolved for the specified name.</exception>
        public ICronJob GetJobByName(string name)
        {
            var innerContainer = Container.CreateInnerContainer();
            ICronJob job;

            try
            {
                job = innerContainer.Resolve<ICronJob>(name);
            }
            catch (Exception exception)
            {
                innerContainer.Dispose();

                throw new JobNotFoundException(
                    string.Format("A job with name \"{0}\" could not be resolved.", name),
                    exception);
            }
            
            return new ScopeNestingJobWrapper(innerContainer, job);
        }
    }
}
