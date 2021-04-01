﻿using MIGA.Core;
using MIGA.Tool.Base;
using MIGA.Tool.Base.Wizards;
using MIGA.Tool.Windows.Pipelines.Setup;

namespace MIGA.Tool.Windows.UserControls.Setup
{
  using System.IO;
  using System.Windows;
  using MIGA.Tool.Base;
  using MIGA.Tool.Base.Wizards;
  using MIGA.Tool.Windows.Pipelines.Setup;
  using JetBrains.Annotations;
  using MIGA.Core;
  using MIGA.Extensions;

  public partial class LocalRepository : IWizardStep, IFlowControl
  {
    #region Constructors

    public LocalRepository()
    {
      InitializeComponent();
    }

    #endregion

    #region Private methods

    bool IFlowControl.OnMovingBack(WizardArgs wizardArgs)
    {
      return true;
    }

    bool IFlowControl.OnMovingNext(WizardArgs wizardArgs)
    {
      var args = (SetupWizardArgs)wizardArgs;
      var repository = args.LocalRepositoryFolderPath;
      var one = !string.IsNullOrEmpty(repository) && FileSystem.FileSystem.Local.Directory.Exists(repository);
      if (!one)
      {
        WindowHelper.ShowMessage("The Local Repository folder does not exist, please choose existing folder", MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.OK);
        return false;
      }

      var license = args.LicenseFilePath;
      var two = !string.IsNullOrEmpty(license) && FileSystem.FileSystem.Local.File.Exists(license);
      if (!two)
      {
        WindowHelper.ShowMessage("The Sitecore License file does not exist, please choose existing file", MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.OK);
        return false;
      }

      return true;
    }

    private void PickLicenseFileClick([CanBeNull] object sender, [CanBeNull] RoutedEventArgs e)
    {
      WindowHelper.PickFile("Choose the Sitecore license file", LicenseFile, PickLicenseFile, "*.xml|*.xml");
    }

    private void PickLocalRepositoryFolderClick([CanBeNull] object sender, [CanBeNull] RoutedEventArgs e)
    {
      WindowHelper.PickFolder("Choose the Local Repository folder", LocalRepositoryFolder, PickLocalRepositoryFolder);
    }

    private void ShowMoreInfo([CanBeNull] object sender, [CanBeNull] RoutedEventArgs e)
    {
      CoreApp.OpenInBrowser("https://bitbucket.org/sitecore/sitecore-instance-manager/wiki/Manual-Download", true);
    }

    private void ShowSupportedModules([CanBeNull] object sender, [CanBeNull] RoutedEventArgs e)
    {
      CoreApp.OpenInBrowser("https://bitbucket.org/sitecore/sitecore-instance-manager/wiki/Home", true);
    }

    #endregion

    #region IWizardStep Members

    private static string GetRepositoryPath()
    {
      var path = Path.Combine(ApplicationManager.DataFolder, "Repository");
      FileSystem.FileSystem.Local.Directory.Ensure(path);
      return path;
    }

    void IWizardStep.InitializeStep(WizardArgs wizardArgs)
    {
      var args = (SetupWizardArgs)wizardArgs;
      LicenseFile.Text = args.LicenseFilePath;
      LocalRepositoryFolder.Text = args.LocalRepositoryFolderPath.EmptyToNull() ?? GetRepositoryPath();
    }

    bool IWizardStep.SaveChanges(WizardArgs wizardArgs)
    {
      var args = (SetupWizardArgs)wizardArgs;
      args.LicenseFilePath = LicenseFile.Text;
      args.LocalRepositoryFolderPath = LocalRepositoryFolder.Text;
      return true;
    }

    #endregion
  }
}