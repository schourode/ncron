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

namespace NCron.Service
{
    /// <summary>
    /// Represents a method that wraps the execution of a job, allowing dependencies to be resolved and released appropriatly.
    /// Implementations will typically be in three steps: (1) resolve dependencies and job, (2) invoke callback, (3) release job and dependencies.
    /// </summary>
    /// <param name="executionCallback">A method that will initialize and execute the job. This method should be invoked once the job is fully constructed.</param>
    public delegate void JobExecutionWrapper(Action<ICronJob> executionCallback);
}
