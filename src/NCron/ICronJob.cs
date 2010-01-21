/*
 * Copyright 2008, 2009, 2010 Joern Schou-Rode
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

namespace NCron
{
    /// <summary>
    /// Defines the interface to be implemented by all NCron jobs.
    /// </summary>
    public interface ICronJob : IDisposable
    {
        /// <summary>
        /// This method is invoked before the job is executed and is used to pass context information to the job.
        /// </summary>
        /// <param name="context">Contains information about the context in which the job is executed.</param>
        void Initialize(CronContext context);

        /// <summary>
        /// This method is invoked whenever the job is due for execution, after job initialization.
        /// </summary>
        void Execute();
    }
}