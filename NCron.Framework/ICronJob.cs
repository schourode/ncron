/*
 * Copyright 2008, 2009 Joern Schou-Rode
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

namespace NCron.Framework
{
    /// <summary>
    /// Defines the interface to be implemented by all NCron jobs.
    /// </summary>
    public interface ICronJob : IDisposable
    {
        /// <summary>
        /// Prepares the job before its first execution.
        /// For simple jobs, the body of this method will often be empty.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Executes the job. This method will be called whenever a scheduled execution time for the job is reached.
        /// </summary>
        void Execute();
    }
}