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

using Autofac;
using NCron.Framework;

namespace NCron.Integration.Autofac
{
    internal class ScopeNestingJobWrapper : ICronJob
    {
        private readonly IContainer _container;
        private readonly ICronJob _actualCronJob;

        public ScopeNestingJobWrapper(IContainer parentContainer, string jobName)
        {
            _container = parentContainer.CreateInnerContainer();

            try
            {
                _actualCronJob = _container.Resolve<ICronJob>(jobName);
            }
            catch
            {
                _container.Dispose();
                throw;
            }
        }

        public void Initialize(CronContext context)
        {
            _actualCronJob.Initialize(context);
        }

        public void Execute()
        {
            _actualCronJob.Execute();
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
