using System.Windows;
using JetBrains.Annotations;
using MIGA.Tool.Base;
using MIGA.Tool.Windows.Dialogs;

namespace MIGA.Tool.Windows.MainWindowComponents.Buttons
{
  [UsedImplicitly]
  public class OpenHostsButton : WindowOnlyButton
  {
    #region Protected methods

    protected override void OnClick(Window mainWindow)
    {
      WindowHelper.ShowDialog(new HostsDialog(), mainWindow);
    }

    #endregion
  }
}