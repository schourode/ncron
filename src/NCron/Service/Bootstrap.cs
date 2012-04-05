﻿/*
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
using System.Reflection;
using System.ServiceProcess;

namespace NCron.Service
{
    /// <summary>
    /// Defines the method which should be called from the entry point of all NCron based applications.
    /// </summary>
    public static class Bootstrap
    {

        private static void PrintUsageGuide()
        {
            var assemblyName = ApplicationInfo.ApplicationName;

            Console.WriteLine("Usage:");
            Console.WriteLine("  {0} debug", assemblyName);
            Console.WriteLine("    Starts the service in interactive mode. Press [ENTER] to exit.");
            Console.WriteLine("  {0} exec {{jobname}}", assemblyName);
            Console.WriteLine("    Executes a named schedule once and exits immediately after.");
            Console.WriteLine("  {0} install", assemblyName);
            Console.WriteLine("    Installs NCron as a Windows service.");
            Console.WriteLine("  {0} uninstall", assemblyName);
            Console.WriteLine("    Uninstalls NCron as a Windows service.");
        }

        /// <summary>
        /// Initilizes an NCron based application with a specified set of command line parameters and service setup handler.
        /// A call to this method should be made from the main entry point of all NCron based applicaitons.
        /// </summary>
        /// <param name="args">The command line parameters passed to the application.</param>
        /// <param name="setupHandler">A method that sets up the scheduling service according to application needs.</param>
        public static void Init(string[] args, Action<ISchedulingService> setupHandler)
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

        private static void RunService(Action<ISchedulingService> setupHandler, bool debug)
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException +=
                    (o, e) => ExceptionHelper.LogUnhandledException(e.ExceptionObject);

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
                ExceptionHelper.LogUnhandledException(exception);
            }
        }

        private static void ExecuteJob(Action<ISchedulingService> setupHandler, string jobName)
        {
            try
            {
                using (var service = new SchedulingService())
                {
                    setupHandler(service);
                    service.ExecuteNamedJob(jobName);
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
                var appAssembly = Assembly.GetEntryAssembly();

                using (var installer = new ProjectInstaller(appAssembly))
                {
                    if (undo)
                    {
                        installer.Uninstall();
                    }
                    else
                    {
                        installer.Install();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }
    }
}