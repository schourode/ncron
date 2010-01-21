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
using System.Diagnostics;

namespace NCron.Logging
{
    /// <summary>
    /// Implements the <see cref="ILog"/> interface on top of a <see cref="EventLog"/> instance.
    /// Debug messages are ignored, and all other messages are written to the underlying event log.
    /// </summary>
    public class EventLogAdapter : ILog
    {
        private readonly EventLog _log;

        /// <summary>
        /// Creates a new instance of the <see cref="EventLogAdapter"/> class for a specified <see cref="EventLog"/> instance.
        /// </summary>
        /// <param name="log">The event log to which error messages should be written.</param>
        public EventLogAdapter(EventLog log)
        {
            _log = log;
        }

        public void Debug(Func<string> msgCallback)
        {
        }

        public void Debug(Func<string> msgCallback, Func<Exception> exCallback)
        {
        }

        public void Info(Func<string> msgCallback)
        {
            _log.WriteEntry(msgCallback(), EventLogEntryType.Information);
        }

        public void Info(Func<string> msgCallback, Func<Exception> exCallback)
        {
            _log.WriteEntry(msgCallback() + ' ' + exCallback(), EventLogEntryType.Information);
        }

        public void Warn(Func<string> msgCallback)
        {
            _log.WriteEntry(msgCallback(), EventLogEntryType.Warning);
        }

        public void Warn(Func<string> msgCallback, Func<Exception> exCallback)
        {
            _log.WriteEntry(msgCallback() + ' ' + exCallback(), EventLogEntryType.Warning);
        }

        public void Error(Func<string> msgCallback)
        {
            _log.WriteEntry(msgCallback(), EventLogEntryType.Error);
        }

        public void Error(Func<string> msgCallback, Func<Exception> exCallback)
        {
            _log.WriteEntry(msgCallback() + ' ' + exCallback(), EventLogEntryType.Error);
        }

        public void Dispose()
        {
            _log.Dispose();
        }
    }
}
