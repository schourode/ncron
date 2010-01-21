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
using NCron.Framework;
using NCron.Framework.Logging;
using NCron.Framework.Scheduling;
using NCron.Service.Scheduling;

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

        internal static SchedulingService GetConfiguredService()
        {
            var section = ConfigurationManager.GetSection("ncron");
            if (section == null)
            {
                throw new ConfigurationErrorsException("Configuration \"ncron\" not found in application configuration.");
            }

            var ncronSection = section as NCronSection;
            if (ncronSection == null)
            {
                throw new ConfigurationErrorsException(
                    string.Format("Configuration \"ncron\" should be of type {0}.", typeof(NCronSection).FullName));
            }

            var schedule = (ISchedule) ncronSection.Schedule.Type.InvokeDefaultConstructor();
            var jobFactory = (IJobFactory) ncronSection.JobFactory.Type.InvokeDefaultConstructor();
            var logFactory = (ILogFactory) ncronSection.LogFactory.Type.InvokeDefaultConstructor();

            return new SchedulingService(schedule, jobFactory, logFactory);
        }
    }
}
