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

namespace NCron.Integration.log4net
{
    public class LogFactory : Framework.Logging.ILogFactory
    {
        public Framework.Logging.ILog GetLogForType(Type type)
        {
            var internalLogger = LogManager.GetLogger(type);
            return new DeferredLog(internalLogger);
        }
    }
}
