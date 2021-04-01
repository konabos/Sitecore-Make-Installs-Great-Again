using System.Windows;
using JetBrains.Annotations;
using MIGA.Tool.Base;
using MIGA.Tool.Windows.Dialogs;

namespace MIGA.Tool.Windows.MainWindowComponents.Buttons
{
  [UsedImplicitly]
  public class SettingsButton : WindowOnlyButton
  {
    #region Protected methods

    protected override void OnClick(Window mainWindow)
    {
      var result = WindowHelper.ShowDialog<SettingsDialog>(null, mainWindow);
      if (result != null)
      {
        MainWindowHelper.Initialize();
      }
    }

    #endregion
  }
}