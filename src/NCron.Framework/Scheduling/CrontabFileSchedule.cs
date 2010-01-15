/*
 * Copyright 2010 Joern Schou-Rode
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

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NCrontab;

namespace NCron.Framework.Scheduling
{
    public class CrontabFileSchedule : ISchedule
    {
        private static readonly Regex LinePattern = new Regex(@"^((?:\S+\s+){5})(.+)$");


        private readonly string _filePath;
        //private readonly FileSystemWatcher _watcher;

        //public event ChangedEventHandler Changed;

        public CrontabFileSchedule(string filePath)
        {
            _filePath = filePath;

            //_watcher = new FileSystemWatcher(Path.GetDirectoryName(filePath), Path.GetFileName(filePath));
            //_watcher.Changed += (s,e) => Changed(this);
            //_watcher.EnableRaisingEvents = true;
        }
        
        public IEnumerable<IScheduleEntry> GetEntries()
        {
            using (var reader = File.OpenText(_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var match = LinePattern.Match(line);
                    var crontab = CrontabSchedule.Parse(match.Groups[1].Value);
                    var jobName = match.Groups[2].Value;

                    var schedule = new CrontabScheduleEntry(crontab, jobName);
                    yield return schedule;
                }
            }
        }

        //public void Dispose()
        //{
        //    _watcher.Dispose();
        //}
    }
}
