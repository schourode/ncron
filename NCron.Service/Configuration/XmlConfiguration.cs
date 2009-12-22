/* Copyright (c) 2008, Joern Schou-Rode <jsr@malamute.dk>

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted, provided that the above
copyright notice and this permission notice appear in all copies.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE. */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;
using NCron.Framework;
using NCron.Framework.Scheduling;

namespace NCron.Service.Configuration
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

        public IEnumerable<ICronJob> GetJobs(string name)
        {
            List<ICronJob> jobs = new List<ICronJob>();
            ConfigureJobCollection(jobs, name);
            return jobs;
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
    }
}
