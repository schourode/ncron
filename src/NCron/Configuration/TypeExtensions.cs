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

namespace NCron.Configuration
{
    internal static class TypeExtensions
    {
        private static readonly Type[] NoFormalArguments = new Type[0];
        private static readonly object[] NoActualArguments = new object[0];

        public static object InvokeDefaultConstructor(this Type type)
        {
            var ctor = type.GetConstructor(NoFormalArguments);
            return ctor.Invoke(NoActualArguments);
        }
    }
}
