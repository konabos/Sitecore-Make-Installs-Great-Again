using MIGA.Pipelines.Install;

namespace MIGA.Pipelines.Reinstall
{
  using System;
  using System.IO;
  using JetBrains.Annotations;
  using MIGA.Pipelines.Install;

  public class AddServerTxt : ReinstallProcessor
  {
    [UsedImplicitly]
    public AddServerTxt()
    {
    }
      
    protected override void Process(ReinstallArgs args)
    {
      if (Settings.CoreInstallCreateServerTxt.Value)
      {
        File.WriteAllText(Path.Combine(args.WebRootPath, "server.txt"), $"{Environment.MachineName}-{args.InstanceName}");
      }
    }
  }
}
