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
using System.Reflection;
using System.Text.RegularExpressions;
using NCrontab;

namespace NCron.Scheduling
{
    /// <summary>
    /// Implements the <see cref="ISchedule"/> interface using text file with crontab expressions as its source.
    /// </summary>
    public class CrontabFileSchedule : ISchedule
    {
        private static readonly Regex LinePattern = new Regex(@"^((?:\S+\s+){5})(.+)$", RegexOptions.Compiled);

        private readonly string _filePath;

        /// <summary>
        /// Creates a new instance of the <see cref="CrontabFileSchedule"/> class, reading from a "crontab.txt" file in the executing assembly's directory.
        /// </summary>
        public CrontabFileSchedule()
        {
            var entryAssembly = Assembly.GetExecutingAssembly();
            var appDirectory = Path.GetDirectoryName(entryAssembly.Location);
            _filePath = Path.Combine(appDirectory, "crontab.txt");
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CrontabFileSchedule"/> class, reading from a specified file name.
        /// </summary>
        /// <param name="filePath">The file system path from where the crontab text file is loaded.</param>
        public CrontabFileSchedule(string filePath)
        {
            _filePath = filePath;
        }
        
        public IEnumerable<IScheduleEntry> GetEntries()
        {
            using (var reader = File.OpenText(_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var match = LinePattern.Match(line);
                    if (!match.Success) continue;

                    var crontab = CrontabSchedule.Parse(match.Groups[1].Value);
                    var jobName = match.Groups[2].Value;

                    var schedule = new CrontabScheduleEntry(crontab, jobName);
                    yield return schedule;
                }
            }
        }
    }
}
