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
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;

using NCron.Scheduling;

namespace NCron.Loader.Configuration
{
    internal class XmlConfiguration
    {
        XmlElement root;

        public XmlConfiguration(XmlElement element)
        {
            this.root = element;

            // TODO Test XML configuration against schema?
        }

        public IEnumerable<CronTimer> Timers
        {
            get
            {
                foreach (XmlElement plan in this.root.SelectNodes("./plans/plan"))
                {
                    CronTimer timer = new CronTimer();
                    ConfigureTimer(timer, plan);
                    yield return timer;
                }
            }
        }

        private void ConfigureTimer(CronTimer timer, XmlElement config)
        {
            foreach (XmlAttribute attr in config.Attributes)
            {
                switch (attr.LocalName)
                {
                    case "minutes": ConfigurePatternCollection(timer.Plan.Minutes, attr.Value); break;
                    case "hours": ConfigurePatternCollection(timer.Plan.Hours, attr.Value); break;
                    case "daysOfWeek": ConfigurePatternCollection(timer.Plan.DaysOfWeek, attr.Value); break;
                    case "daysOfMonth": ConfigurePatternCollection(timer.Plan.DaysOfMonth, attr.Value); break;
                    case "months": ConfigurePatternCollection(timer.Plan.Months, attr.Value); break;

                    case "name": ConfigureJobCollection(timer.Jobs, attr.Value); break;
                }
            }
        }

        private void ConfigurePatternCollection(ICollection<Pattern> col, string patterns)
        {
            if (patterns.Length > 0)
            {
                foreach (string pattern in patterns.Split(','))
                {
                    col.Add(Pattern.Parse(pattern));
                }
            }
        }

        private void ConfigureJobCollection(ICollection<ICronJob> col, string name)
        {
            // TODO Rewrite the following to use regular expressions.

            StringBuilder xpath = new StringBuilder("./jobs");

            string[] parts = name.Split('.');
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                xpath.Append((i == parts.Length - 1) ? "/job" : "/group");
                if (part != "*") xpath.Append("[@name='" + part + "']");
            }

            foreach (XmlElement job in this.root.SelectNodes(xpath.ToString()))
            {
                col.Add(CreateCronJob(job));
            }
        }

        private ICronJob CreateCronJob(XmlElement config)
        {
            Dictionary<string, string> properties = GetJobProperties(config);

            string typeName = properties["type"];
            if (typeName == null)
                throw new ConfigurationErrorsException("The \"type\" property is required for all cron jobs.");

            Type type = Type.GetType(typeName, true);
            properties.Remove("type");

            ConstructorInfo cstr = type.GetConstructor(new Type[0]);
            if (cstr == null)
                throw new ConfigurationErrorsException(string.Format("The type \"{0}\" does not have a default (parameter less) constructor.", typeName));

            ICronJob job = (ICronJob)cstr.Invoke(new object[0]);

            foreach (KeyValuePair<string, string> property in properties)
            {
                PropertyInfo propInfo = type.GetProperty(property.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propInfo != null)
                {
                    object val = Convert.ChangeType(property.Value, propInfo.PropertyType, CultureInfo.InvariantCulture);
                    propInfo.SetValue(job, val, null);
                }
            }

            return job;
        }

        private Dictionary<string, string> GetJobProperties(XmlNode elm)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            do
            {
                foreach (XmlAttribute attr in elm.Attributes)
                {
                    if (!properties.ContainsKey(attr.LocalName))
                        properties.Add(attr.LocalName, attr.Value);
                }
                elm = elm.ParentNode;
            } while (elm.LocalName == "group");

            return properties;
        }

        class SayHelloJob : ICronJob
        {
            public int Priority { get { return 0; } }

            public void Execute() { Console.WriteLine("{0:T} - Execute", DateTime.Now); }
            public void Initialize() { Console.WriteLine("{0:T} - Initialize", DateTime.Now); }
            public void Dispose() { Console.WriteLine("{0:T} - Dispose", DateTime.Now); }
        }
    }
}
