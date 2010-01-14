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
using System.ServiceProcess;
using NCron.Service.Scheduling;

namespace NCron.Service
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            // We explicitly use the EventLog for logging purposes in the main exception handling.
            // If the configuration cannot be loaded - and no log factory created - it will be logged.
            // If a custom ILog implementation throws, this will also be logged here.

            using (var eventLog = new EventLog { Source = "NCron" })
            {
                try
                {
                    var service = new SchedulingService();

                    if (args.Length > 0 && args[0] == "debug")
                    {
                        service.Start();
                        Console.ReadLine();
                        service.Stop();
                    }
                    else
                    {
                        var adapter = new ServiceProcessAdapter(service);
                        ServiceBase.Run(adapter);
                    }

                }
                catch (Exception ex)
                {
                    eventLog.WriteEntry("An unhandled exception has occured. " + ex, EventLogEntryType.Error);
                }
            }
        }
    }
}