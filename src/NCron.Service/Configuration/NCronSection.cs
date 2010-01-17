/*
 * Copyright 2008, 2009, 2010 Joern Schou-Rode
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

using System.Configuration;

namespace NCron.Service.Configuration
{
    public class NCronSection : ConfigurationSection
    {
        [ConfigurationProperty("schedule")]
        public ScheduleElement Schedule
        {
            get { return (ScheduleElement) base["schedule"]; }
        }

        [ConfigurationProperty("jobFactory")]
        public JobFactoryElement JobFactory
        {
            get { return (JobFactoryElement)base["jobFactory"]; }
        }

        [ConfigurationProperty("logFactory")]
        public LogFactoryElement LogFactory
        {
            get { return (LogFactoryElement) base["logFactory"]; }
        }

        internal static NCronSection GetConfiguration()
        {
            var section = ConfigurationManager.GetSection("ncron");
            if (section == null)
            {
                throw new ConfigurationErrorsException("Configuration \"ncron\" not found in application configuration.");
            }

            var ncronSection = section as NCronSection;
            if (section == null)
            {
                throw new ConfigurationErrorsException(
                    string.Format("Configuration \"ncron\" should be of type {0}.", typeof(NCronSection).FullName));
            }

            return ncronSection;
        }
    }
}
