/*
 * Copyright 2008, 2009 Joern Schou-Rode
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
using System.IO;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using NCron.Framework;
using NCron.Framework.Configuration;
using NCron.Framework.Logging;
using NCron.Service.Reflection;
using NCron.Service.Scheduling;
using NCrontab;

namespace NCron.Service
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                var service = new CronService();
                ServiceBase.Run(service);
            }
            else
            {
                switch (args[0].ToLower())
                {
                    case "debug":
                        var config = Configuration.NCronSection.GetConfiguration();
                        var jobFactory = (IJobFactory) config.JobFactory.Type.InvokeDefaultConstructor();
                        var logFactory = (ILogFactory) config.LogFactory.Type.InvokeDefaultConstructor();
                        var appDirectory = Path.GetDirectoryName(typeof (Program).Assembly.Location);
                        var crontab = new TextFileCrontab(Path.Combine(appDirectory, "crontab.txt"));
                        var entryPattern = new Regex(@"^((?:\S+\s+){5})(.+)$");

                        var scheduler = new Scheduler(jobFactory, logFactory);

                        foreach (var entry in crontab.GetEntries())
                        {
                            var match = entryPattern.Match(entry);
                            var schedule = CrontabSchedule.Parse(match.Groups[1].Value);
                            var job = match.Groups[2].Value;
                            scheduler.Enqueue(new QueueEntry(schedule, job));
                        }

                        scheduler.Run();

                        Console.ReadLine();
                        break;

                    default:
                        Console.WriteLine("Uknown command: " + args[0]);
                        Environment.Exit(1);
                        break;
                }
            }
        }
    }
}

public class TestJob : CronJob
{
    public override void Execute()
    {
        Log.Info(() => GetHashCode().ToString());
    }
}