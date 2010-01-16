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

using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using NCron.Framework;
using NCron.Framework.Logging;
using NCron.Framework.Scheduling;
using NCron.Service.Reflection;
using NCron.Service.Scheduling;

namespace NCron.Service
{
    internal static class Program
    {
        // We explicitly use the EventLog for logging purposes in the main exception handling.
        // If the configuration cannot be loaded - and no log factory created - it will be logged.
        // If a custom ILog implementation throws, this will also be logged here.
        private static readonly EventLog CoreLog = new EventLog { Source = "NCron" };
        
        public static void LogUnhandledException(object exception)
        {
            CoreLog.WriteEntry("An unhandled exception has occured. " + exception, EventLogEntryType.Error);
        }

        private static void Main(string[] args)
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException +=
                    (o, e) => LogUnhandledException(e.ExceptionObject);

                using (var service = GetConfiguredService())
                {
                    if (args.Length > 0 && args[0] == "debug")
                    {
                        service.Start();
                        Console.ReadLine();
                        service.Stop();
                    }
                    else
                    {
                        using (var adapter = new ServiceProcessAdapter(service))
                        {
                            ServiceBase.Run(adapter);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogUnhandledException(exception);
            }
        }

        private static SchedulingService GetConfiguredService()
        {
            var appDirectory = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            var schedule = new CrontabFileSchedule(Path.Combine(appDirectory, "crontab.txt"));

            var config = Configuration.NCronSection.GetConfiguration();
            var jobFactory = (IJobFactory) config.JobFactory.Type.InvokeDefaultConstructor();
            var logFactory = (ILogFactory) config.LogFactory.Type.InvokeDefaultConstructor();

            return new SchedulingService(schedule, jobFactory, logFactory);
        }
    }
}