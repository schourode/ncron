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

using NCron.Service;

namespace NCron.Fluent
{
    /// <summary>
    /// Represents a configured schedule entry in the fluent API.
    /// </summary>
    public class JobPart : Part
    {
        internal JobPart(SchedulingService service, QueueEntry queueEntry)
            : base(service, queueEntry)
        {
        }

        /// <summary>
        /// Attaches a name to the schedule entry, allowing it to be executed outside of its schedule using a command line argument.
        /// </summary>
        /// <param name="name">The name under which the schedule entry should be registered.</param>
        public void Named(string name)
        {
            Service.NameEntry(name, QueueEntry);
        }
    }
}
