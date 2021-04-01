
using MIGA.Tool.Base.Profiles;
using MIGA.Tool.Base.Wizards;

namespace MIGA.Tool.Windows.Pipelines.Setup
{
  using MIGA.Tool.Base.Profiles;
  using MIGA.Tool.Base.Wizards;

  public class SetupProcessor : SaveAgreement, IAfterLastWizardPipelineStep
  {
    public new void Execute(WizardArgs wargs)
    {
      base.Execute(wargs);

      var args = (SetupWizardArgs)wargs;
      var profile = ProfileManager.Profile ?? new Profile();
      profile.ConnectionString = args.ConnectionString;
      profile.InstancesFolder = args.InstancesRootFolderPath;
      profile.License = args.LicenseFilePath;
      profile.LocalRepository = args.LocalRepositoryFolderPath;
      ProfileManager.SaveChanges(profile);
    }
  }
}