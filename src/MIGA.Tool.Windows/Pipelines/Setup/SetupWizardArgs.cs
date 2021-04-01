using MIGA.Pipelines.Processors;
using MIGA.Tool.Base.Profiles;
using MIGA.Tool.Base.Wizards;

namespace MIGA.Tool.Windows.Pipelines.Setup
{
  using MIGA.Pipelines.Processors;
  using MIGA.Tool.Base.Profiles;
  using MIGA.Tool.Base.Wizards;
  using System;

  public class SetupWizardArgs : WizardArgs
  {
    #region Constructors

    public SetupWizardArgs()
    {
    }

    public SetupWizardArgs(Profile profile)
    {
      if (profile == null)
      {
        return;
      }

      InstancesRootFolderPath = profile.InstancesFolder;
      LicenseFilePath = profile.License;
      ConnectionString = profile.ConnectionString;
      LocalRepositoryFolderPath = profile.LocalRepository;
    }

    #endregion

    #region Public properties

    public string ConnectionString { get; set; }

    public string InstancesRootFolderPath { get; set; }

    public string LicenseFilePath { get; set; }
    public string LocalRepositoryFolderPath { get; set; }

    #endregion

    #region Public methods

    public override ProcessorArgs ToProcessorArgs()
    {
      return new ProcessorArgs();
    }

    #endregion
  }
}
