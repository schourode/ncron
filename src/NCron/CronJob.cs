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

using System;
using NCron.Framework.Logging;

namespace NCron.Framework
{
    /// <summary>
    /// Provides an abstract implementation of the <see cref="ICronJob"/> interface.
    /// Extending this class, rather than implementing <see cref="ICronJob"/> directly, makes it easier to implement new jobs.
    /// </summary>
    public abstract class CronJob : ICronJob
    {
        /// <summary>
        /// Gets the <see cref="CronContext"/> in which the job is being executed.
        /// </summary>
        protected CronContext Context { get; private set; }

        /// <summary>
        /// Gets the <see cref="ILog"/> from the current <see cref="CronContext"/>.
        /// </summary>
        protected ILog Log { get; private set; }

        /// <summary>
        /// This method is invoked before the job is executed and is used to pass context information to the job.
        /// The context gets stored in the <see cref="Context"/> property.
        /// </summary>
        /// <param name="context">Contains information about the context in which the job is executed.</param>
        public void Initialize(CronContext context)
        {
            Log = context.Log;
        }

        /// <summary>
        /// This method is invoked whenever the job is due for execution, after job initialization.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="CronJob"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CronJob()
        {
            Dispose(false);
        }
    }
}
