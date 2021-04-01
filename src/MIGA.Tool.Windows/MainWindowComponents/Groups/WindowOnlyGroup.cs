using System.Windows;
using MIGA.Instances;
using MIGA.Tool.Base.Plugins;

namespace MIGA.Tool.Windows.MainWindowComponents.Groups
{
  public class WindowOnlyGroup : IMainWindowGroup
  {
    #region Public methods

    public virtual bool IsVisible(Window mainWindow, Instance instance)
    {
      return true;
    }

    #endregion
  }
}