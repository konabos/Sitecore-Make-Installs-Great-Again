using System.IO;
using System.Windows;
using JetBrains.Annotations;
using MIGA.ContainerInstaller;
using MIGA.Instances;
using MIGA.SitecoreEnvironments;
using MIGA.Tool.Base;
using MIGA.Tool.Base.Pipelines;
using MIGA.Tool.Base.Profiles;
using MIGA.Tool.Base.Wizards;
using MIGA.Extensions;
using Sitecore.Diagnostics.Base;

namespace MIGA.Tool.Windows.MainWindowComponents.Buttons
{
  [UsedImplicitly]
  public class ReinstallInstanceButton : InstanceOnlyButton
  {
    #region Public methods

    public override void OnClick(Window mainWindow, Instance instance)
    {
      if (instance?.Name != null &&
          SitecoreEnvironmentHelper.GetExistingSitecoreEnvironment(instance.Name)?.EnvType ==
          SitecoreEnvironment.EnvironmentType.Container
      )
      {
        DeleteContainersWizardArgs deleteContainersWizardArgs = new DeleteContainersWizardArgs()
        {
          DestinationFolder = SitecoreEnvironmentHelper.GetExistingSitecoreEnvironment(instance.Name).UnInstallDataPath,
          EnvironmentId = SitecoreEnvironmentHelper.GetExistingSitecoreEnvironment(instance.Name).ID,
          Env = EnvModel.LoadFromFile(Path.Combine(
            SitecoreEnvironmentHelper.GetExistingSitecoreEnvironment(instance.Name).UnInstallDataPath, ".env"))
        };

        WizardPipelineManager.Start(
          "reinstallContainer",
          mainWindow,
          null,
          null,
          null,
          () => deleteContainersWizardArgs);

        return;
      }

      if (instance != null)
      {
        if (!MainWindowHelper.IsInstallerReady())
        {
          WindowHelper.ShowMessage(@"The installer isn't ready - check the Settings window", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
          return;
        }

        var license = ProfileManager.Profile.License;
        Assert.IsNotNull(license, @"The license file isn't set in the Settings window");
        FileSystem.FileSystem.Local.File.AssertExists(license, "The {0} file is missing".FormatWith(license));
        int version;
        if (int.TryParse(instance.Product.ShortVersion,out version)&& version < 90)
        {
          MainWindowHelper.ReinstallInstance(instance, mainWindow, license, ProfileManager.GetConnectionString());
        }
        else
        {
          MainWindowHelper.Reinstall9Instance(instance, mainWindow, license, ProfileManager.GetConnectionString());
        }
      }
    }

    #endregion
  }
}