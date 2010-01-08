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

namespace NCron.Framework.Configuration
{
    public class TextFileCrontab : ICrontab
    {
        private readonly string _filePath;
        private readonly FileSystemWatcher _watcher;

        public event ChangedEventHandler Changed;

        public TextFileCrontab(string filePath)
        {
            _filePath = filePath;

            _watcher = new FileSystemWatcher(Path.GetDirectoryName(filePath), Path.GetFileName(filePath));
            _watcher.Changed += (s,e) => Changed(this);
            _watcher.EnableRaisingEvents = true;
        }
        
        public IEnumerable<string> GetEntries()
        {
            using (var reader = File.OpenText(_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }
}
