using System.Windows;
using JetBrains.Annotations;
using MIGA.Instances;

namespace MIGA.Tool.Base.Plugins
{
  public interface IMainWindowGroup
  {
    #region Public methods

    bool IsVisible([NotNull] Window mainWindow, [CanBeNull] Instance instance);

    #endregion
  }
}
