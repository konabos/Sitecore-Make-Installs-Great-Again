﻿namespace MIGA.Pipelines.Delete
{
  using System;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;
  using Sitecore.Diagnostics.Logging;

  #region

  #endregion

  [UsedImplicitly]
  public class StopInstance : DeleteProcessor
  {
    #region Methods

    protected override void Process([NotNull] DeleteArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      try
      {
        args.InstanceStop();
      }
      catch (Exception ex)
      {
        Log.Warn(ex, $"Cannot stop instance {args.InstanceName}. {ex.Message}");
      }
    }

    #endregion
  }
}