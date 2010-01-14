/*
 * Copyright 2009, 2010 Joern Schou-Rode
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
using NCron.Framework;
using NCron.Service.Configuration;

namespace NCron.Service.Scheduling
{
    internal class SchedulingService
    {
        public SchedulingService()
        {
            var config = Configuration.NCronSection.GetConfiguration();
            /*            var jobFactory = (IJobFactory)config.JobFactory.Type.InvokeDefaultConstructor();
                        var logFactory = (ILogFactory)config.LogFactory.Type.InvokeDefaultConstructor();
                        var appDirectory = Path.GetDirectoryName(typeof(Program).Assembly.Location);
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
            */
        }

        public void Start()
        {
        }

        public void Stop()
        {
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