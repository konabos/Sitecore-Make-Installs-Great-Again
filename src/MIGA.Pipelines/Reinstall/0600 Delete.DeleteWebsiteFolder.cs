﻿namespace MIGA.Pipelines.Reinstall
{
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  #region

  #endregion

  [UsedImplicitly]
  public class DeleteWebsiteFolder : ReinstallProcessor
  {
    #region Methods

    protected override void Process([NotNull] ReinstallArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      FileSystem.FileSystem.Local.Directory.DeleteIfExists(args.WebRootPath);
    }

    #endregion
  }
}