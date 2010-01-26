/*
 * Copyright 2008, 2010 Joern Schou-Rode
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

using System.ServiceProcess;

namespace NCron.Service
{
    [System.ComponentModel.DesignerCategory("Code")]
    internal class ServiceProcessAdapter : ServiceBase
    {
        private readonly SchedulingService _service;
        
        public ServiceProcessAdapter(SchedulingService service)
        {
            _service = service;
        }

        protected override void OnStart(string[] args)
        {
            _service.Start();
        }

        protected override void OnStop()
        {
            _service.Stop();
        }

        protected override void OnPause()
        {
            _service.Stop();
        }

        protected override void OnContinue()
        {
            _service.Start();
        }
    }
}
