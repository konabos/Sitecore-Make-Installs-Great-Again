﻿using MIGA.Adapters.WebServer;

namespace MIGA.Pipelines.Delete
{
  using MIGA.Adapters.WebServer;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  #region

  #endregion

  [UsedImplicitly]
  public class UpdateHosts : DeleteProcessor
  {
    #region Methods

    protected override void Process([NotNull] DeleteArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      Hosts.Remove(args.InstanceHostNames);
    }

    #endregion
  }
}