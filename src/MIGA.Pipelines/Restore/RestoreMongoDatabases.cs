namespace MIGA.Pipelines.Restore
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;
  using Sitecore.Diagnostics.Logging;

  [UsedImplicitly]
  public class RestoreMongoDatabases : RestoreProcessor
  {
    #region Fields

    protected readonly List<string> _Done = new List<string>();

    #endregion

    #region Methods

    protected override long EvaluateStepsCount(RestoreArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      return args.Backup.MongoDatabaseFilenames.Count();
    }

    protected override bool IsRequireProcessing(RestoreArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      return args.Backup.BackupMongoDatabases;
    }

    protected override void Process([NotNull] RestoreArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      var databases = args.Backup.MongoDatabaseFilenames;

      var instance = args.Instance;

      try
      {
        instance.Stop();
        foreach (var database in databases)
        {
          if (_Done.Contains(database))
          {
            continue;
          }

          MongoHelper.Restore(database);
          IncrementProgress();

          _Done.Add(database);
        }
      }
      finally
      {
        try
        {
          instance.Start();
        }
        catch (Exception ex)
        {
          Log.Warn(ex, "An error occurred during starting an instance");
        }
      }


      IncrementProgress();
    }

    #endregion
  }
}