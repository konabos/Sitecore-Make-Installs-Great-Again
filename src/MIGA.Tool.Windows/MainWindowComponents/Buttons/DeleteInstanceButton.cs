﻿using System.IO;
using System.Linq;
using System.Windows;
using JetBrains.Annotations;
using MIGA.ContainerInstaller;
using MIGA.Instances;
using MIGA.Pipelines.Delete;
using MIGA.SitecoreEnvironments;
using MIGA.Tool.Base;
using MIGA.Tool.Base.Pipelines;
using MIGA.Tool.Base.Profiles;
using MIGA.Tool.Base.Wizards;

namespace MIGA.Tool.Windows.MainWindowComponents.Buttons
{
  [UsedImplicitly]
  public class DeleteInstanceButton : InstanceOnlyButton
  {
    #region Public methods

    public override void OnClick(Window mainWindow, Instance instance)
    {
      if (instance != null)
      {
        var connectionString = ProfileManager.GetConnectionString();
        var args = new DeleteArgs(instance, connectionString);
        args.OnCompleted += () => mainWindow.Dispatcher.Invoke(() => OnPipelineCompleted(args));
        var index = MainWindowHelper.GetListItemID(instance.ID);
        int version;
        if (int.TryParse(instance.Product.ShortVersion,out version)&& version < 90)
        {
          WizardPipelineManager.Start("delete", mainWindow, args, null, (ignore) => OnWizardCompleted(index, args.HasInstallationBeenCompleted), () => null);
        }
        else if (
          instance?.Name != null &&
          SitecoreEnvironmentHelper.GetExistingSitecoreEnvironment(instance.Name).EnvType == SitecoreEnvironment.EnvironmentType.Container
                 )
        {
          DeleteContainersWizardArgs deleteContainersWizardArgs = new DeleteContainersWizardArgs()
          {
             DestinationFolder = SitecoreEnvironmentHelper.GetExistingSitecoreEnvironment(instance.Name).UnInstallDataPath,
             EnvironmentId = SitecoreEnvironmentHelper.GetExistingSitecoreEnvironment(instance.Name).ID,
             Env = EnvModel.LoadFromFile(Path.Combine(SitecoreEnvironmentHelper.GetExistingSitecoreEnvironment(instance.Name).UnInstallDataPath, ".env"))
        };

          WizardPipelineManager.Start(
            "deleteContainer", 
            mainWindow, 
            null, 
            null, 
            (ignore) => {
              if (deleteContainersWizardArgs.ShouldRefreshInstancesList)
              {
                MainWindowHelper.RefreshInstances();
              }
            }, 
            () => deleteContainersWizardArgs
            );
        }
        else
        {
          string uninstallPath = string.Empty;
          SitecoreEnvironment env = SitecoreEnvironmentHelper.GetExistingSitecoreEnvironment(instance.Name);
          if (!string.IsNullOrEmpty(env?.UnInstallDataPath))
          {
            uninstallPath = env.UnInstallDataPath;
          }
          else
          {
            foreach (string installName in Directory.GetDirectories(ApplicationManager.UnInstallParamsFolder).OrderByDescending(s => s.Length))
            {
              if (instance.Name.StartsWith(Path.GetFileName(installName)))
              {
                uninstallPath = installName;
                break;
              }
            }
          }
          if (string.IsNullOrEmpty(uninstallPath))
          {
            WindowHelper.ShowMessage("UnInstall files not found.");
            return;
          }

          Delete9WizardArgs delete9WizardArgsargs = new Delete9WizardArgs(instance, connectionString, uninstallPath);
          WizardPipelineManager.Start("delete9", mainWindow, null, null, (ignore) => OnWizardCompleted(index, delete9WizardArgsargs.ShouldRefreshInstancesList),() => delete9WizardArgsargs);
        }
      }
    }

    #endregion

    #region Private methods

    private static void OnWizardCompleted(int index, bool hasInstallationBeenCompleted)
    {
      if (hasInstallationBeenCompleted)
      {
        MainWindowHelper.SoftlyRefreshInstances();
      }

      MainWindowHelper.MakeInstanceSelected(index);
    }

    private void OnPipelineCompleted(DeleteArgs args)
    {
      var root = new DirectoryInfo(args.RootPath);
      if (root.Exists && root.GetFiles("*", SearchOption.AllDirectories).Length > 0)
      {
        FileSystem.FileSystem.Local.Directory.TryDelete(args.RootPath);
      }

      args.HasInstallationBeenCompleted = true;

    }

    #endregion
  }
}