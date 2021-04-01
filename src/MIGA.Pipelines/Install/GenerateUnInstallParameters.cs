using JetBrains.Annotations;
using MIGA.Sitecore9Installer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines.Install
{
  public class GenerateUnInstallParameters : Processor
  {
    protected override void Process([NotNull] ProcessorArgs args)
    {
      Install9Args arguments = (Install9Args)args;
      if (arguments.Tasker.UnInstall||arguments.ScriptsOnly)
      {
        this.Skip();
        return;
      }
      arguments.UnInstallDataPath= arguments.Tasker.SaveUninstallData(ApplicationManager.UnInstallParamsFolder);
    }
  }
}
