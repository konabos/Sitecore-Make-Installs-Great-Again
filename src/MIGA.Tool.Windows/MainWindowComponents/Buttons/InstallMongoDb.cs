using System.Windows;
using JetBrains.Annotations;
using MIGA.Core;
using Sitecore.Diagnostics.Base;

namespace MIGA.Tool.Windows.MainWindowComponents.Buttons
{
  [UsedImplicitly]
  public class InstallMongoDbButton : WindowOnlyButton
  {
    #region Protected methods

    protected override void OnClick(Window mainWindow)
    {
      Assert.ArgumentNotNull(mainWindow, nameof(mainWindow));

      CoreApp.RunApp(ApplicationManager.GetEmbeddedFile(@"MongoDb.WindowsService.Installer.zip", "MIGA.Tool.Windows", @"MongoDb.WindowsService.Installer.exe"));
    }

    #endregion
  }
}