﻿namespace MIGA.Pipelines.Install
{
  using System;
  using System.IO;
  using JetBrains.Annotations;

  public class AddServerTxt : InstallProcessor
  {
    [UsedImplicitly]
    public AddServerTxt()
    {
    }

    protected override void Process(InstallArgs args)
    {
      if (Settings.CoreInstallCreateServerTxt.Value)
      {
        File.WriteAllText(Path.Combine(args.WebRootPath, "server.txt"), $"{Environment.MachineName}-{args.InstanceName}");
      }
    }
  }
}
