/*
 * Copyright 2008, 2009 Joern Schou-Rode
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
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace NCron.Service
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            ServiceProcessInstaller procInst = new ServiceProcessInstaller();
            procInst.Account = ServiceAccount.LocalService;
            base.Installers.Add(procInst);

            ServiceInstaller svcInst = new ServiceInstaller();
            svcInst.ServiceName = "ncron";
            svcInst.DisplayName = "NCron task scheduling";
            svcInst.Description = "Executes task according to the configured NCron schedule.";
            svcInst.StartType = ServiceStartMode.Automatic;
            base.Installers.Add(svcInst);
        }
    }
}
