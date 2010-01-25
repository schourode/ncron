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

namespace NCron.Logging
{
    /// <summary>
    /// Defines the interface of a factory that allows creation of <see cref="ILog"/> instances for specific jobs.
    /// This is the interface to implement, if you need integration with a third-party logging framework.
    /// </summary>
    public interface ILogFactory
    {
        /// <summary>
        /// Gets the log to be used by a specific job.
        /// </summary>
        /// <param name="job">The job for which a log should be retrieved.</param>
        /// <returns>An implementation of <see cref="ILog"/> for the specified job.</returns>
        ILog GetLogForJob(ICronJob job);
    }
}
