﻿using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines.Reinstall
{
  using System.Collections.Generic;
  using MIGA.Adapters.SqlServer;
  using MIGA.Pipelines.Processors;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  #region

  #endregion

  [UsedImplicitly]
  public class DeleteDatabases : ReinstallProcessor
  {
    #region Fields

    private readonly List<string> _Done = new List<string>();

    #endregion

    #region Methods

    public override long EvaluateStepsCount(ProcessorArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      return ((ReinstallArgs)args)._InstanceDatabases.Count;
    }

    protected override void Process(ReinstallArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      IEnumerable<Database> detectedDatabases = args._InstanceDatabases;
      var rootPath = args.RootPath.ToLower();
      var connectionString = args.ConnectionString;
      var instanceName = args.InstanceName;
      IPipelineController controller = Controller;

      DeleteDatabasesHelper.Process(detectedDatabases, rootPath, connectionString, instanceName, controller, _Done);

      if (controller != null)
      {
        controller.IncrementProgress(args._InstanceDatabases.Count - 1);
      }
    }

    #endregion
  }
}