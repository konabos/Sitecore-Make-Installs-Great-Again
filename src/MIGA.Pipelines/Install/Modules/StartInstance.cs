using MIGA.Instances;

namespace MIGA.Pipelines.Install.Modules
{
  using MIGA.Instances;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  #region

  #endregion

  [UsedImplicitly]
  public class StartInstance : InstallProcessor
  {
    #region Methods

    protected override void Process([NotNull] InstallArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      Instance instance = args.Instance;
      Assert.IsNotNull(instance, nameof(instance));

      if (ProcessorDefinition.Param == "nowait")
      {
        if (!args.PreHeat)
        {
          return;
        }
        
        try
        {
          InstanceHelper.StartInstance(instance, 500);
        }
        catch
        {
          // ignore error
        }
      }
      else
      {
        InstanceHelper.StartInstance(instance);
      }
    }

    #endregion
  }
}