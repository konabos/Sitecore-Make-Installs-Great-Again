﻿namespace MIGA.Pipelines.Reinstall
{
  using System.IO;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  #region

  #endregion

  [UsedImplicitly]
  public class CopyLicense : ReinstallProcessor
  {
    #region Methods

    protected override void Process([NotNull] ReinstallArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      FileSystem.FileSystem.Local.File.Copy(args.LicenseFilePath, Path.Combine(args.DataFolderPath, "license.xml"));
    }

    #endregion
  }
}