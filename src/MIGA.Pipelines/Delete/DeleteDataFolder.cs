﻿namespace MIGA.Pipelines.Delete
{
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  #region

  #endregion

  [UsedImplicitly]
  public class DeleteDataFolder : DeleteProcessor
  {
    #region Methods

    protected override void Process([NotNull] DeleteArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      var path = args.InstanceDataFolderPath;
      FileSystem.FileSystem.Local.Directory.DeleteIfExists(path);
    }

    #endregion
  }
}