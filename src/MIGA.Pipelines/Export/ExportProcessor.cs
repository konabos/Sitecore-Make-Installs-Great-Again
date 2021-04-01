﻿using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines.Export
{
  using MIGA.Pipelines.Processors;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  public abstract class ExportProcessor : Processor
  {
    #region Methods

    #region Public methods

    public override sealed long EvaluateStepsCount(ProcessorArgs args)
    {
      return EvaluateStepsCount((ExportArgs)args);
    }

    public override bool IsRequireProcessing(ProcessorArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      return IsRequireProcessing((ExportArgs)args);
    }

    #endregion

    #region Protected methods

    protected virtual long EvaluateStepsCount(ExportArgs args)
    {
      return 1;
    }

    protected virtual bool IsRequireProcessing([NotNull] ExportArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      return true;
    }

    protected override void Process(ProcessorArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      Process((ExportArgs)args);
    }

    protected abstract void Process([NotNull] ExportArgs args);

    #endregion

    #endregion
  }
}