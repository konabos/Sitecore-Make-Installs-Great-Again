﻿using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines.Reinstall
{
  using MIGA.Pipelines.Processors;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  #region

  #endregion

  public abstract class ReinstallProcessor : Processor
  {
    #region Public Methods

    public override sealed bool IsRequireProcessing(ProcessorArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      return IsRequireProcessing((ReinstallArgs)args);
    }

    #endregion

    #region Methods

    protected virtual bool IsRequireProcessing([NotNull] ReinstallArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      return true;
    }

    protected override sealed void Process([NotNull] ProcessorArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      Process((ReinstallArgs)args);
    }

    protected abstract void Process([NotNull] ReinstallArgs args);

    #endregion
  }
}