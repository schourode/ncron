/* Copyright (c) 2008, Joern Schou-Rode <jsr@malamute.dk>

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted, provided that the above
copyright notice and this permission notice appear in all copies.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE. */

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
