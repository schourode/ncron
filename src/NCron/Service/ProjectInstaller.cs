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
using System.Collections;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace NCron.Service
{
    [System.ComponentModel.DesignerCategory("Code")]
    internal class ProjectInstaller : IDisposable
    {
        private readonly Installer _installer;
        private readonly Assembly _appAssembly;

        public ProjectInstaller(Assembly appAssembly)
        {
            _installer = new Installer();
            _appAssembly = appAssembly;

            CreateSubInstallers();
            SetInstallerContext();
        }

        private void CreateSubInstallers()
        {
            var serviceName = _appAssembly.GetName().Name;

            _installer.Installers.Add(new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalService
            });

            _installer.Installers.Add(new ServiceInstaller
            {
                ServiceName = serviceName,
                DisplayName = serviceName,
                Description = "Executes scheduled jobs.",
                StartType = ServiceStartMode.Automatic
            });
        }

        private void SetInstallerContext()
        {
            _installer.Context = new InstallContext();
            _installer.Context.Parameters["assemblypath"] = _appAssembly.Location;
        }

        public void Install()
        {
            var state = new Hashtable();

            try
            {
                _installer.Install(state);
                _installer.Commit(state);
            }
            catch
            {
                try
                {
                    _installer.Rollback(state);
                }
                catch { }
                throw;
            }
        }

        public void Uninstall()
        {
            _installer.Uninstall(null);
        }

        public void Dispose()
        {
            _installer.Dispose();
        }
    }
}
