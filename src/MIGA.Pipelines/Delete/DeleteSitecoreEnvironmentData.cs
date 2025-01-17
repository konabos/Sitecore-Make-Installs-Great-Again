﻿using System.Linq;
using JetBrains.Annotations;
using MIGA.Pipelines.Install;
using MIGA.Pipelines.Processors;
using MIGA.Sitecore9Installer;
using MIGA.SitecoreEnvironments;

namespace MIGA.Pipelines.Delete
{
  public class DeleteSitecoreEnvironmentData : Processor
  {
    protected override void Process([NotNull] ProcessorArgs args)
    {
      Install9Args arguments = (Install9Args)args;
      if (!arguments.Tasker.UnInstall || arguments.ScriptsOnly)
      {
        this.Skip();
        return;
      }

      InstallParam sqlDbPrefixParam = arguments.Tasker.GlobalParams.FirstOrDefault(p => p.Name == "SqlDbPrefix");
      if (sqlDbPrefixParam != null && !string.IsNullOrEmpty(sqlDbPrefixParam.Value))
      {
        foreach (SitecoreEnvironment sitecoreEnvironment in SitecoreEnvironmentHelper.SitecoreEnvironments)
        {
          if (sitecoreEnvironment.Name == sqlDbPrefixParam.Value)
          {
            SitecoreEnvironmentHelper.SitecoreEnvironments.Remove(sitecoreEnvironment);
            SitecoreEnvironmentHelper.SaveSitecoreEnvironmentData(SitecoreEnvironmentHelper.SitecoreEnvironments);
            return;
          }
        }
      }
    }
  }
}