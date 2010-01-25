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

namespace NCron.Fluent.Reflection
{
    /// <summary>
    /// Provides extension methods on the fluent API, allowing job registrations by late bound type references.
    /// </summary>
    public static class Extensions
    {
        private static readonly Type[] NoFormalArguments = new Type[0];
        private static readonly object[] NoActualArguments = new object[0];

        /// <summary>
        /// Registers a job for execution using the job type as parameter.
        /// </summary>
        /// <param name="part">The fluent part upon which the job is to be registered.</param>
        /// <param name="jobType">The type of the job to be created.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static JobPart Run(this SchedulePart part, Type jobType)
        {
            var constructor = jobType.GetConstructor(NoFormalArguments);
            return part.Run(() => (ICronJob)constructor.Invoke(NoActualArguments));
        }

        /// <summary>
        /// Registers a job for execution using the job type name as parameter.
        /// </summary>
        /// <param name="part">The fluent part upon which the job is to be registered.</param>
        /// <param name="jobType">The fully qualified type name of the job (including assembly name if from seperate assembly).</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static JobPart Run(this SchedulePart part, string jobType)
        {
            var type = Type.GetType(jobType, true, false);
            return part.Run(type);
        }
    }
}
