using MIGA.Instances;

namespace MIGA.Pipelines.Reinstall
{
  using Sitecore.Diagnostics.Base;
  using MIGA.Instances;

  #region

  #endregion

  public class StartInstance : ReinstallProcessor
  {
    #region Protected methods

    protected override void Process(ReinstallArgs args)
    {
      InstanceManager.Default.Initialize();

      var instance = InstanceManager.Default.GetInstance(args.InstanceName);
      Assert.IsNotNull(instance, nameof(instance));

      if (ProcessorDefinition.Param == "nowait")
      {
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