﻿namespace SIM.Tool.Windows.MainWindowComponents
{
  using System.Linq;
  using System.Windows;
  using SIM.Core.Common;
  using SIM.Instances;
  using SIM.Products;
  using SIM.Tool.Base;
  using SIM.Tool.Base.Plugins;
  using SIM.Tool.Base.Profiles;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;
  using SIM.Tool.Base.Pipelines;
  using SIM.Tool.Base.Wizards;

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
      Assert.IsTrue(ProductManager.ContainerProducts.Any(),
        $@"You don't have any container product package in your repository. 

 If you already have them then you can either: 

* change the local repository folder (Ribbon -> Home -> Settings button) to the one that contains the files 

* put the files into the current local repository folder: 
{ProfileManager.Profile.LocalRepository}");

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

          if (install.HasInstallationBeenCompleted)
          {
            MainWindowHelper.SoftlyRefreshInstances();
          }
        }, () => new InstallContainerWizardArgs());
      }
    }

    #endregion
  }
}