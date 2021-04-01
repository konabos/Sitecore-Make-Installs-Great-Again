﻿namespace MIGA.Pipelines.Reinstall
{
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  #region

  #endregion

  [UsedImplicitly]
  public class DeleteDataFolder : ReinstallProcessor
  {
    #region Methods

    protected override void Process([NotNull] ReinstallArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      var path = args.DataFolderPath;
      FileSystem.FileSystem.Local.Directory.DeleteIfExists(path);
    }

    #endregion
  }
}