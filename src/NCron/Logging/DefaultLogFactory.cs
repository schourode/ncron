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

using System.Diagnostics;

namespace NCron.Logging
{
    /// <summary>
    /// Implements the <see cref="ILogFactory"/> returning <see cref="EventLogAdapter"/> instances for all requests.
    /// </summary>
    public class DefaultLogFactory : ILogFactory
    {
        /// <summary>
        /// Gets an <see cref="EventLogAdapter"/> instance, writing to the "Application" event log with a source name of "NCron".
        /// </summary>
        /// <param name="job">The job which the log will be used with. This parameter is ignored.</param>
        /// <returns>An <see cref="EventLogAdapter"/>, writing to the "Application" event log with a source name of "NCron".</returns>
        public ILog GetLogForJob(ICronJob job)
        {
            var eventLog = new EventLog { Source = "NCron" };
            return new EventLogAdapter(eventLog);
        }
    }
}
