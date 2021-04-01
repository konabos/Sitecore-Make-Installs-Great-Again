using MIGA.Pipelines.Agent;

namespace MIGA.Pipelines.Install.Modules
{
  using System.Linq;
  using MIGA.Pipelines.Agent;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  #region

  #endregion

  [UsedImplicitly]
  public class CopyPackages : InstallProcessor
  {
    #region Methods

    protected override bool IsRequireProcessing(InstallArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      return args._Modules.Any(m => m.IsPackage);
    }

    protected override void Process([NotNull] InstallArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      Assert.IsNotNull(args.Instance, "Instance");
      AgentHelper.CopyPackages(args.Instance, args._Modules);
    }

    #endregion
  }
}