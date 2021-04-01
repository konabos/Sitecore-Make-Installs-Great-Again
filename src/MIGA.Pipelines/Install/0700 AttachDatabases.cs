﻿using MIGA.Adapters.WebServer;
using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines.Install
{
  using System.Collections.Generic;
  using MIGA.Adapters.WebServer;
  using MIGA.Pipelines.Processors;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  #region

  #endregion

  [UsedImplicitly]
  public class AttachDatabases : InstallProcessor
  {
    #region Fields

    private readonly List<string> _Done = new List<string>();
    
    #endregion

    #region Methods

    public override long EvaluateStepsCount([CanBeNull] ProcessorArgs args)
    {
      return AttachDatabasesHelper.StepsCount;
    }

    protected override void Process(InstallArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      var defaultConnectionString = args.ConnectionString;
      Assert.IsNotNull(defaultConnectionString, "SQL Connection String isn't set in the Settings dialog");

      var instance = args.Instance;
      Assert.IsNotNull(instance, nameof(instance));

      var sqlPrefix = args.InstanceSqlPrefix;
      Assert.IsNotNull(sqlPrefix, nameof(sqlPrefix));

      var controller = Controller;

      foreach (ConnectionString connectionString in instance.Configuration.ConnectionStrings)
      {
        var connectionStringName = connectionString.Name;
        if (_Done.Contains(connectionStringName))
        {
          continue;
        }
        
        if (connectionStringName.Contains("apikey"))
        {
          _Done.Add(connectionStringName);
          continue;
        }

        AttachDatabasesHelper.AttachDatabase(connectionString, defaultConnectionString, args.Name, sqlPrefix, args.InstanceAttachSql, args.DatabasesFolderPath, instance.Name, controller);

        if (controller != null)
        {
          controller.IncrementProgress(AttachDatabasesHelper.StepsCount / args.ConnectionString.Count);
        }

        _Done.Add(connectionStringName);
      }
    }

    #endregion
  }
}
