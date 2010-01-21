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

using log4net;
using log4net.Config;
using NCron.Logging;

namespace NCron.Integration.log4net
{
    /// <summary>
    /// Implements the <see cref="ILogFactory"/> interface using "log4net" as log provider.
    /// </summary>
    public class LogFactory : ILogFactory
    {
        static LogFactory()
        {
            BasicConfigurator.Configure();
        }

        public NCron.Logging.ILog GetLogByName(string name)
        {
            var internalLogger = LogManager.GetLogger(name);
            return new LogAdapter(internalLogger);
        }
    }
}
