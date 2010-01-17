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

using NCron.Framework.Logging;

namespace NCron.Framework
{
    /// <summary>
    /// Holds information about the context in which a cron job is being executed.
    /// Most importantly, the <see cref="Log"/> property gives access to a job specific log.
    /// </summary>
    public class CronContext
    {
        /// <summary>
        /// Gets the name of the job being executed, as specified in the cron schedule.
        /// </summary>
        public string JobName { get; private set; }

        /// <summary>
        /// Gets the job being executed.
        /// </summary>
        public ICronJob Job { get; private set; }

        /// <summary>
        /// Gets the log to be used while executing the job in this context.
        /// </summary>
        public ILog Log { get; private set; }

        /// <summary>
        /// Creates a new <see cref="CronContext"/> with all properties explicitly set.
        /// </summary>
        /// <param name="jobName">The name of the job being executed, as specified in the cron schedule.</param>
        /// <param name="job">The job being executed.</param>
        /// <param name="log">Tog to be used while executing the job in this context.</param>
        public CronContext(string jobName, ICronJob job, ILog log)
        {
            JobName = jobName;
            Job = job;
            Log = log;
        }
    }
}
