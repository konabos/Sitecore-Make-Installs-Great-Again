﻿using MIGA.Adapters.WebServer;
using MIGA.Instances;
using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines.Reinstall
{
  using System.Collections.Generic;
  using MIGA.Adapters.WebServer;
  using MIGA.Instances;
  using MIGA.Pipelines.Processors;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  #region

  #endregion

  [UsedImplicitly]
  public class AttachDatabases : ReinstallProcessor
  {
    #region Fields

    private readonly List<string> _Done = new List<string>();
    
    #endregion

    #region Methods

    public override long EvaluateStepsCount([CanBeNull] ProcessorArgs args)
    {
      return AttachDatabasesHelper.StepsCount;
    }

    protected override void Process(ReinstallArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      var defaultConnectionString = args.ConnectionString;
      Assert.IsNotNull(defaultConnectionString, "SQL Connection String isn't set in the Settings dialog");

      var instanceName = args.instanceName;
      var instance = InstanceManager.Default.GetInstance(instanceName);
      var controller = Controller;

      var sqlPrefix = args.SqlPrefix;

      foreach (ConnectionString connectionString in instance.Configuration.ConnectionStrings)
      {
        if (_Done.Contains(connectionString.Name))
        {
          continue;
        }

        AttachDatabasesHelper.AttachDatabase(connectionString, defaultConnectionString, args.Name, sqlPrefix, true, args.DatabasesFolderPath, args.InstanceName, controller);

        if (controller != null)
        {
          controller.IncrementProgress(AttachDatabasesHelper.StepsCount / args.ConnectionString.Count);
        }
        
        _Done.Add(connectionString.Name);
      }
    }

    #endregion
  }
}