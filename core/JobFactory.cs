// Copyright (c) 2007-2008, Joern Schou-Rode <jsr@malamute.dk>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
// 
// * Redistributions of source code must retain the above copyright
//   notice, this list of conditions and the following disclaimer.
// 
// * Redistributions in binary form must reproduce the above copyright
//   notice, this list of conditions and the following disclaimer in the
//   documentation and/or other materials provided with the distribution.
// 
// * Neither the name of the NCron nor the names of its contributors may
//   be used to endorse or promote products derived from this software
//   without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE REGENTS OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Globalization;

namespace NCron
{
    public class JobFactory
    {
        static Type[] EMPTY_TYPE_ARRAY = new Type[0];
        static object[] EMPTY_OBJECT_ARRAY = new object[0];

        Dictionary<string, Type> _types;

        public JobFactory(bool autoLoadCoreAssembly)
        {
            _types = new Dictionary<string, Type>();
        }

        public void LoadAssembly(Assembly assembly, string prefix)
        {
            //TODO Internal.Program.CoreLog.Trace("Loading module '{0}'...", assembly.GetName().Name);

            foreach (Type type in assembly.GetTypes())
            {
                foreach (object attr in type.GetCustomAttributes(false))
                {
                    if (attr is JobAttribute)
                    {
                        string name = string.Concat(prefix, ((JobAttribute)attr).Name);
                        _types.Add(name.ToLower(CultureInfo.InvariantCulture), type);

                        //TODO Internal.Program.CoreLog.Trace("Node type '{0}' registered.", type.FullName);
                    }
                }
            }
        }

        public void LoadAssembly(string assemblyName, string prefix)
        {
            LoadAssembly(Assembly.Load(assemblyName), prefix);
        }

        public JobBase CreateTask(string typeName, NameValueCollection properties)
        {
            // Check if we have a type registered with the specified name.
            string nameLowered = typeName.ToLower(CultureInfo.InvariantCulture);
            if (!_types.ContainsKey(nameLowered))
                throw new ApplicationException(string.Format("No check type is registered with the name '{0}'.", typeName));

            // Grab a reference to the parameterless constructor of the type - exception if not found.
            Type type = _types[nameLowered];
            ConstructorInfo constructor = type.GetConstructor(EMPTY_TYPE_ARRAY);
            if (constructor == null)
                throw new ApplicationException(string.Format("Cannot access the constructor of '{0}'.", type.FullName));

            // Invoke constructor and cast result as CheckBase - exception if casting unsuccesful.
            JobBase check = constructor.Invoke(EMPTY_OBJECT_ARRAY) as JobBase;
            if (check == null)
                throw new ApplicationException(string.Format("The type '{0}' is not a valid check.", type.FullName));

            // Loop through the properties for the newly instanciated check object.
            foreach (string propName in properties.Keys)
            {
                // Look for a public instance property with a matching name - exception if not found.
                PropertyInfo prop = type.GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop == null)
                    throw new ApplicationException(string.Format("Property '{1}' unknown for checks of type {0}.", type.FullName, propName));

                // Convert value to applicable type, and assign it to the property.
                object val = Convert.ChangeType(properties[propName], prop.PropertyType, CultureInfo.InvariantCulture);
                prop.SetValue(check, val, null);
            }

            // Return the fully instanciated task object.
            return check;
        }
    }
}
