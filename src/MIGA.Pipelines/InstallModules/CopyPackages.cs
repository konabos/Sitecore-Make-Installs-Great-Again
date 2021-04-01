using MIGA.Pipelines.Agent;

namespace MIGA.Pipelines.InstallModules
{
  using System.Linq;
  using MIGA.Pipelines.Agent;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  #region

  #endregion

  [UsedImplicitly]
  public class CopyPackages : InstallModulesProcessor
  {
    #region Methods

    protected override bool IsRequireProcessing(InstallModulesArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      return args._Modules.Any(m => m.IsPackage);
    }

    protected override void Process([NotNull] InstallModulesArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      AgentHelper.CopyPackages(args.Instance, args._Modules);
    }

    #endregion
  }
}