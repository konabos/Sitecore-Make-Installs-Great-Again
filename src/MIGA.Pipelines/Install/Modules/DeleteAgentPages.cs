using MIGA.Pipelines.Agent;

namespace MIGA.Pipelines.Install.Modules
{
  using MIGA.Pipelines.Agent;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  #region

  #endregion

  [UsedImplicitly]
  public class DeleteAgentPages : InstallProcessor
  {
    #region Methods

    protected override void Process([NotNull] InstallArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      Assert.IsNotNull(args.Instance, "Instance");
      AgentHelper.DeleteAgentFiles(args.Instance);
    }

    #endregion
  }
}