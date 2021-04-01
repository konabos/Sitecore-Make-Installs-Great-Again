using JetBrains.Annotations;
using Sitecore.Diagnostics.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MIGA.Pipelines.Install;
using MIGA.Pipelines.Processors;
using MIGA.Sitecore9Installer;
using Sitecore.Diagnostics.Logging;

namespace MIGA.Pipelines.Delete
{
  public class CleanUp : Processor
  {
    protected override void Process([NotNull] ProcessorArgs args)
    {
      Install9Args arguments = args as Install9Args;
      Assert.ArgumentNotNull(arguments, nameof(arguments));
      if (arguments.ScriptsOnly)
      {
        this.Skip();
        return;
      }

      InstallParam param = arguments.Tasker.GlobalParams.FirstOrDefault(p => p.Name == "DeployRoot");
      if (param!=null)
      {
        int retriesNumber = 3;
        for (int i=0;i<= retriesNumber; i++)
        {
          if (Directory.Exists(param.Value))
          {
            try
            {
              Directory.Delete(param.Value, true);
            }
            catch(System.IO.IOException ex)
            {
              Log.Warn($"Can't remove directory: {param.Value}. {ex.Message}");
            }
            if (Directory.Exists(param.Value))
            {
              if (retriesNumber == i)
              {
                throw new Exception($"Can't remove directory: {param.Value}");
              }
              Thread.Sleep(10000);
            }
            else
            {
              break;
            }
          }
        }
      }

      Directory.Delete(arguments.Tasker.UnInstallParamsPath, true);
    }
  }
}
