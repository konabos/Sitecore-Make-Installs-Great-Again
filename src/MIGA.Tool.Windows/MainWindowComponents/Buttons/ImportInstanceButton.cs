﻿using System.Windows;
using JetBrains.Annotations;
using Microsoft.Win32;
using MIGA.IO.Real;
using MIGA.Tool.Base;
using MIGA.Tool.Base.Wizards;
using MIGA.Tool.Windows.UserControls.Import;
using MIGA.Extensions;
using Sitecore.Diagnostics.Base;
using Sitecore.Diagnostics.Logging;

namespace MIGA.Tool.Windows.MainWindowComponents.Buttons
{
  [UsedImplicitly]
  public class ImportInstanceButton : WindowOnlyButton
  {
    #region Protected methods

    protected override void OnClick(Window mainWindow)
    {
      Assert.ArgumentNotNull(mainWindow, nameof(mainWindow));

      var fileDialog = new OpenFileDialog
      {
        Title = "Select zip file of exported solution",
        Multiselect = false,
        DefaultExt = ".zip"
      };

      fileDialog.ShowDialog();
      var filePath = fileDialog.FileName;
      if (string.IsNullOrEmpty(filePath))
      {
        return;
      }

      Log.Info($"Importing solution from {filePath}");
      var fileSystem = new RealFileSystem();
      var file = fileSystem.ParseFile(filePath);
      using (var zipFile = new RealZipFile(fileSystem.ParseFile(file.FullName)))
      {
        const string AppPoolFileName = "AppPoolSettings.xml";
        var appPool = zipFile.Entries.Contains(AppPoolFileName);
        if (!appPool)
        {
          WindowHelper.ShowMessage("Wrong package for import. The package does not contain the {0} file.".FormatWith(AppPoolFileName));
          return;
        }

        const string WebsiteSettingsFileName = "WebsiteSettings.xml";
        var websiteSettings = zipFile.Entries.Contains(WebsiteSettingsFileName);
        if (!websiteSettings)
        {
          WindowHelper.ShowMessage("Wrong package for import. The package does not contain the {0} file.".FormatWith(WebsiteSettingsFileName));

          return;
        }

        const string WebConfigFileName = @"Website/Web.config";
        if (!zipFile.Entries.Contains(WebConfigFileName))
        {
          WindowHelper.ShowMessage("Wrong package for import. The package does not contain the {0} file.".FormatWith(WebConfigFileName));

          return;
        }
      }

      WizardPipelineManager.Start("import", mainWindow, null, null, ignore => MainWindowHelper.SoftlyRefreshInstances(), () => new ImportWizardArgs(file.FullName));
    }

    #endregion
  }
}