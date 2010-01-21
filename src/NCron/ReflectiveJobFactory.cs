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

namespace NCron.Framework
{
    /// <summary>
    /// Implements the <see cref="IJobFactory"/> using reflection to load jobs by type names.
    /// </summary>
    public class ReflectiveJobFactory : IJobFactory
    {
        private static readonly Type[] NoFormalArguments = new Type[0];
        private static readonly object[] NoActualArguments = new object[0];

        /// <summary>
        /// Gets an <see cref="ICronJob"/> instance from a name specified by a cron schedule.
        /// The name is expected to be a assembly qualified type name (eg "Acme.Cron.SomeJob, Acme.Cron").
        /// A new instance of the specified type is created with each call to this method.
        /// </summary>
        /// <param name="name">The assembly qualified type name of the class implementing <see cref="ICronJob"/>.</param>
        /// <returns>A newly created <see cref="ICronJob"/> instance matching the provided name.</returns>
        /// <exception cref="JobNotFoundException">if the type is not found, it has no public default constructor or it does not implement <see cref="ICronJob"/>.</exception>
        public ICronJob GetJobByName(string name)
        {
            try
            {
                var type = Type.GetType(name, true);
                var ctor = type.GetConstructor(NoFormalArguments);
                var instance = ctor.Invoke(NoActualArguments);
                return (ICronJob)instance;
            }
            catch (Exception innerException)
            {
                throw new JobNotFoundException(
                    string.Format("A job with the name \"{0}\" could not be found.", name),
                    innerException);
            }
        }
    }
}
