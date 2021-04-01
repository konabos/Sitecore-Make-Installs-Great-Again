﻿using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines.InstallModules
{
  using MIGA.Pipelines.Processors;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  #region

  #endregion

  public abstract class InstallModulesProcessor : Processor
  {
    #region Methods

    #region Public methods

    public override sealed long EvaluateStepsCount(ProcessorArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      return EvaluateStepsCount((InstallModulesArgs)args);
    }

    public override sealed bool IsRequireProcessing(ProcessorArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      return IsRequireProcessing((InstallModulesArgs)args);
    }

    #endregion

    #region Protected methods

    protected virtual long EvaluateStepsCount([NotNull] InstallModulesArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      return 1;
    }

    protected virtual bool IsRequireProcessing([NotNull] InstallModulesArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      return true;
    }

    protected override void Process(ProcessorArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      Process((InstallModulesArgs)args);
    }

    protected abstract void Process([NotNull] InstallModulesArgs args);

    #endregion

    #endregion
  }
}