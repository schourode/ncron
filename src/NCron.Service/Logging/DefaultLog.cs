/*
 * Copyright 2009 Joern Schou-Rode
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

namespace NCron.Service.Logging
{
    class DefaultLog : Framework.ILog
    {
        private readonly ILog _log;

        public DefaultLog(Type type)
        {
            _log = LogManager.GetLogger(type);
        }

        public void Debug(Func<string> msgCallback)
        {
            Debug(msgCallback, null);
        }

        public void Debug(Func<string> msgCallback, Exception exception)
        {
            if (_log.IsDebugEnabled)
            {
                _log.Debug(msgCallback(), exception);
            }
        }

        public void Info(Func<string> msgCallback)
        {
           Info(msgCallback, null);
        }

        public void Info(Func<string> msgCallback, Exception exception)
        {
            if (_log.IsInfoEnabled)
            {
                _log.Info(msgCallback(), exception);
            }
        }

        public void Warn(Func<string> msgCallback)
        {
            Warn(msgCallback, null);
        }

        public void Warn(Func<string> msgCallback, Exception exception)
        {
            if (_log.IsWarnEnabled)
            {
                _log.Warn(msgCallback(), exception);
            }
        }

        public void Error(Func<string> msgCallback)
        {
            Error(msgCallback, null);
        }

        public void Error(Func<string> msgCallback, Exception exception)
        {
            if (_log.IsErrorEnabled)
            {
                _log.Error(msgCallback(), exception);
            }
        }

        public void Fatal(Func<string> msgCallback)
        {
            Fatal(msgCallback, null);
        }

        public void Fatal(Func<string> msgCallback, Exception exception)
        {
            if (_log.IsFatalEnabled)
            {
                _log.Fatal(msgCallback(), exception);
            }
        }
    }
}
