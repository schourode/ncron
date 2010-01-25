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
    public static class Extensions
    {
        private static readonly Type[] NoFormalArguments = new Type[0];
        private static readonly object[] NoActualArguments = new object[0];

        public static JobPart Run(this SchedulePart part, Type jobType)
        {
            var constructor = jobType.GetConstructor(NoFormalArguments);
            return part.Run(() => (ICronJob)constructor.Invoke(NoActualArguments));
        }

        public static JobPart Run(this SchedulePart part, string jobType)
        {
            var type = Type.GetType(jobType, true, false);
            return part.Run(type);
        }

        public static JobPart Run<T>(this SchedulePart part)
            where T : ICronJob, new()
        {
            return part.Run(() => new T());
        }
    }
}
