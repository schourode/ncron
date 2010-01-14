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

using System;
using System.ServiceProcess;
using NCron.Service.Scheduling;

namespace NCron.Service
{
    /// <summary>
    /// Built on top of <see cref="System.ServiceProcess.ServiceBase"/>, this class serves as the cron service entry point.
    /// The service can be installed using classes from the <see cref="System.Configuration.Install"/> namespace.
    /// </summary>
    internal class ServiceProcessAdapter : ServiceBase
    {
        private SchedulingService _service;

        /// <summary>
        /// Creates a new instance of the <see cref="CronService"/> class.
        /// </summary>
        public ServiceProcessAdapter(SchedulingService service)
        {
            _service = service;
        }

        /// <summary>
        /// Executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically).
        /// Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            _service.Start();
        }

        /// <summary>
        /// Executes when a Stop command is sent to the service by the Service Control Manager (SCM).
        /// Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            _service.Stop();
        }

        /// <summary>
        /// Executes when a Pause command is sent to the service by the Service Control Manager (SCM).
        /// Specifies actions to take when a service pauses.
        /// </summary>
        protected override void OnPause()
        {
            _service.Stop();
        }

        /// <summary>
        /// Executes when a Continue command is sent to the service by the Service Control Manager (SCM).
        /// Specifies actions to take when a service resumes normal functioning after being paused.
        /// </summary>
        protected override void OnContinue()
        {
            _service.Start();
        }
    }
}
