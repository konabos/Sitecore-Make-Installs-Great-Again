﻿using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines.Install
{
  using MIGA.Pipelines.Processors;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  #region

  #endregion

  [UsedImplicitly]
  public class Extract : InstallProcessor
  {
    #region Public Methods

    public override long EvaluateStepsCount(ProcessorArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      return InstallHelper.GetStepsCount(((InstallArgs)args).PackagePath);
    }

    #endregion

    #region Methods

    protected override void Process(InstallArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));
      var packagePath = args.PackagePath;

      var webRootPath = args.WebRootPath;
      var databasesFolderPath = args.DatabasesFolderPath;
      var dataFolderPath = args.DataFolderPath;


      InstallHelper.ExtractFile(packagePath, webRootPath, databasesFolderPath, dataFolderPath, args.InstanceAttachSql, args.InstallRadControls, args.InstallDictionaries, Controller);
    }

    #endregion
  }
}