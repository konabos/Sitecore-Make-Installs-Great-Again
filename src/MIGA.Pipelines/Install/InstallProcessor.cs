﻿using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines.Install
{
  using MIGA.Pipelines.Processors;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  #region

  #endregion

  public abstract class InstallProcessor : Processor
  {
    #region Public Methods

    public override sealed bool IsRequireProcessing(ProcessorArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      return IsRequireProcessing((InstallArgs)args);
    }

    #endregion

    #region Methods

    protected virtual bool IsRequireProcessing([NotNull] InstallArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      return true;
    }

    protected override sealed void Process([NotNull] ProcessorArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      Process((InstallArgs)args);
    }

    protected abstract void Process([NotNull] InstallArgs args);

    #endregion
  }
}