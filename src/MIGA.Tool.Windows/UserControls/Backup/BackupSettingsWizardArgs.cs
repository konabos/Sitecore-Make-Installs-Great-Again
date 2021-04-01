using MIGA.FileSystem;
using MIGA.Instances;
using MIGA.Pipelines.Backup;
using MIGA.Pipelines.Processors;
using MIGA.Tool.Base.Profiles;
using MIGA.Tool.Base.Wizards;

namespace MIGA.Tool.Windows.UserControls.Backup
{
  using System;
  using MIGA.FileSystem;
  using MIGA.Instances;
  using MIGA.Pipelines.Backup;
  using MIGA.Pipelines.Processors;
  using MIGA.Tool.Base.Profiles;
  using MIGA.Tool.Base.Wizards;

  public class BackupSettingsWizardArgs : WizardArgs
  {
    #region Fields

    public Instance Instance { get; }
    public bool _Databases;
    public bool _ExcludeClient;
    public bool _Files;
    public bool _MongoDatabases;
    private string _instanceName { get; }

    #endregion

    #region Constructors

    public BackupSettingsWizardArgs(Instance instance)
    {
      Instance = instance;
      _instanceName = instance.Name;
      BackupName = string.Format("{0:yyyy-MM-dd} at {0:hh-mm-ss}", DateTime.Now);
    }

    #endregion

    #region Public properties

    public string BackupName { get; set; }

    public string InstanceName
    {
      get
      {
        return _instanceName;
      }
    }

    #endregion

    #region Public methods

    public override ProcessorArgs ToProcessorArgs()
    {
      var backupArgs = new BackupArgs(Instance, ProfileManager.GetConnectionString(), PathUtils.EscapePath(BackupName.Trim(), "."), _Files, _Databases, _ExcludeClient, _MongoDatabases);

      return backupArgs;
    }

    #endregion
  }
}
