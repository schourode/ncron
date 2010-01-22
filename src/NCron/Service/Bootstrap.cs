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
using System.Reflection;
using System.ServiceProcess;

namespace NCron.Service
{
    public static class Bootstrap
    {
        // We explicitly use the EventLog for logging purposes in the main exception handling.
        // If the configuration cannot be loaded - and no log factory created - it will be logged.
        // If a custom ILog implementation throws, this will also be logged here.
        private static readonly EventLog CoreLog = new EventLog { Source = "NCron" };
        
        internal static void LogUnhandledException(object exception)
        {
            CoreLog.WriteEntry("An unhandled exception has occured. " + exception, EventLogEntryType.Error);
        }

        private static void PrintUsageGuide()
        {
            var assemblyName = Assembly.GetEntryAssembly().GetName().Name;

            Console.WriteLine("Usage:");
            Console.WriteLine("  {0} debug", assemblyName);
            Console.WriteLine("    Starts the service in interactive mode. Press [ENTER] to exit.");
            Console.WriteLine("  {0} exec {{jobname}}", assemblyName);
            Console.WriteLine("    Execute a single job, using job and log factories defined in configuration.");
            Console.WriteLine("  {0} install", assemblyName);
            Console.WriteLine("    Installs NCron as a Windows service.");
            Console.WriteLine("  {0} uninstall", assemblyName);
            Console.WriteLine("    Uninstalls NCron as a Windows service.");
        }

        public static void Main(string[] args, ServiceSetupHandler setupHandler)
        {
            if (!Environment.UserInteractive)
            {
                RunService(setupHandler, false);
            }
            else if (args.Length == 0)
            {
                PrintUsageGuide();
            }
            else switch (args[0].ToLower())
            {
                case "debug":
                    if (args.Length != 1) PrintUsageGuide();
                    else RunService(setupHandler, true);
                    break;

                case "exec":
                case "execute":
                    if (args.Length != 2) PrintUsageGuide();
                    else ExecuteJob(setupHandler, args[1]);
                    break;

                case "install":
                    if (args.Length != 1) PrintUsageGuide();
                    Install(false);
                    break;

                case "uninstall":
                    if (args.Length != 1) PrintUsageGuide();
                    Install(true);
                    break;

                default:
                    PrintUsageGuide();
                    break;
            }
        }

        private static void RunService(ServiceSetupHandler setupHandler, bool debug)
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException +=
                    (o, e) => LogUnhandledException(e.ExceptionObject);

                using (var service = new SchedulingService())
                {
                    setupHandler(service);

                    if (debug)
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

        private static void ExecuteJob(ServiceSetupHandler setupHandler, string jobName)
        {
            try
            {
                using (var service = new SchedulingService())
                {
                    setupHandler(service);
                    service.ExecuteJob(jobName);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        private static void Install(bool undo)
        {
            try
            {
                ProjectInstaller.Install(undo);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }

    public delegate void ServiceSetupHandler(SchedulingService service);
}