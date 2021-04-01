using System.Windows;
using JetBrains.Annotations;
using MIGA.Tool.Base;
using MIGA.Tool.Windows.Dialogs;
using Sitecore.Diagnostics.Base;

namespace MIGA.Tool.Windows.MainWindowComponents.Buttons
{
  [UsedImplicitly]
  public class AboutOnlyButton : WindowOnlyButton
  {
    #region Public methods

    protected override void OnClick(Window mainWindow)
    {
      Assert.ArgumentNotNull(mainWindow, nameof(mainWindow));

      WindowHelper.ShowDialog<AboutDialog>(null, mainWindow);
    }

    #endregion
  }
}