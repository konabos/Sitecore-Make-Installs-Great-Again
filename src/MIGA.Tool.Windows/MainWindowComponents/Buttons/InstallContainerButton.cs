﻿using MIGA.Instances;
using MIGA.Products;
using MIGA.Tool.Base;
using MIGA.Tool.Base.Pipelines;
using MIGA.Tool.Base.Plugins;
using MIGA.Tool.Base.Profiles;
using MIGA.Tool.Base.Wizards;

namespace MIGA.Tool.Windows.MainWindowComponents
{
  using System.Linq;
  using System.Windows;
  using MIGA.Instances;
  using MIGA.Products;
  using MIGA.Tool.Base;
  using MIGA.Tool.Base.Plugins;
  using MIGA.Tool.Base.Profiles;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;
  using MIGA.Tool.Base.Pipelines;
  using MIGA.Tool.Base.Wizards;

  [UsedImplicitly]
  public class InstallContainerButton : IMainWindowButton
  {
    #region Public methods

    public bool IsEnabled(Window mainWindow, Instance instance)
    {
      return true;
    }

    public bool IsVisible(Window mainWindow, Instance instance)
    {
      return true;
    }

    public void OnClick(Window mainWindow, Instance instance)
    {
      Assert.IsTrue(ProfileManager.IsValid, "Some of configuration settings are invalid - please fix them in Settings dialog and try again");

      if (!ProductManager.ContainerProducts.Any())
      {
        string message =
$@"You don't have any container product package in your repository. 

If you already have them then you can either: 

* change the local repository folder (Ribbon -> Home -> Settings button) to the one that contains the files 

* download a package from 'https://github.com/Sitecore/container-deployment/releases', put the file into the current local repository folder: '{ProfileManager.Profile.LocalRepository}'";

        MessageBox.Show(message, "", MessageBoxButton.OK, MessageBoxImage.Information);

        return;
      }

      if (!ApplicationManager.IsDockerRunning)
      {
        string message = "The 'Docker Desktop' application is not running. Please start the app and re-run the deployment Sitecore to Docker.";

        MessageBox.Show(message, "", MessageBoxButton.OK, MessageBoxImage.Information);

        return;
      }

      if (ApplicationManager.IsIisRunning)
      {
        string urlToWikiPage = "https://github.com/Sitecore/Sitecore-Instance-Manager/wiki/Troubleshooting";

        string message = $@"The IIS is running now. 
It may prevent 'docker-compose' from spinning up Sitecore in Docker, due to the HTTPS port 443 usage conflict.
Please visit the '{urlToWikiPage}' for details.

Please stop the IIS and continue the installation.

Do you want to proceed with the installation process?";

        if (MessageBox.Show(message, "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
        {
          return;
        }
      }


      if (EnvironmentHelper.CheckSqlServer())
      {
        WizardPipelineManager.Start("installContainer", mainWindow, null, null, (args) =>
        {

          if (args == null)
          {
            return;
          }

          var install = (InstallWizardArgs)args;
          var product = install.Product;
          if (product == null)
          {
            return;
          }

          if (install.ShouldRefreshInstancesList)
          {
            MainWindowHelper.RefreshInstances();
          }
        }, () => new InstallContainerWizardArgs());
      }
    }

    #endregion
  }
}