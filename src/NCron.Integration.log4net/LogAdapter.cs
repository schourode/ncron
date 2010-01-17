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
using log4net;

namespace NCron.Integration.log4net
{
    internal class LogAdapter : Framework.Logging.ILog
    {
        private readonly ILog _log;

        public LogAdapter(ILog log)
        {
            _log = log;
        }

        public void Debug(Func<string> msgCallback)
        {
            if (_log.IsDebugEnabled)
                _log.Debug(msgCallback());
        }

        public void Debug(Func<string> msgCallback, Func<Exception> exCallback)
        {
            if (_log.IsDebugEnabled)
                _log.Debug(msgCallback(), exCallback());
        }

        public void Info(Func<string> msgCallback)
        {
            if (_log.IsInfoEnabled)
                _log.Info(msgCallback());
        }

        public void Info(Func<string> msgCallback, Func<Exception> exCallback)
        {
            if (_log.IsInfoEnabled)
                _log.Info(msgCallback(), exCallback());
        }

        public void Warn(Func<string> msgCallback)
        {
            if (_log.IsWarnEnabled)
                _log.Warn(msgCallback());
        }

        public void Warn(Func<string> msgCallback, Func<Exception> exCallback)
        {
            if (_log.IsWarnEnabled)
                _log.Warn(msgCallback(), exCallback());
        }

        public void Error(Func<string> msgCallback)
        {
            if (_log.IsErrorEnabled)
                _log.Error(msgCallback());
        }

        public void Error(Func<string> msgCallback, Func<Exception> exCallback)
        {
            if (_log.IsErrorEnabled)
                _log.Error(msgCallback(), exCallback());
        }

        public void Dispose()
        {
        }
    }
}