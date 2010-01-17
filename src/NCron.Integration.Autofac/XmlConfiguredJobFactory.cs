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

using Autofac.Builder;
using Autofac.Configuration;

namespace NCron.Integration.Autofac
{
    /// <summary>
    /// Extends the <see cref="AutofacJobFactory"/> to automatically load configuration from an &lt;autofac&gt; section of the application configuration.
    /// </summary>
    public class XmlConfiguredJobFactory : AutofacJobFactory
    {
        /// <summary>
        /// Creates a new instance of the <see cref="XmlConfiguredJobFactory"/> class.
        /// </summary>
        public XmlConfiguredJobFactory()
        {
            var config = new ConfigurationSettingsReader("autofac");
            var builder = new ContainerBuilder();
            builder.RegisterModule(config);
            Container = builder.Build();
        }
    }
}
