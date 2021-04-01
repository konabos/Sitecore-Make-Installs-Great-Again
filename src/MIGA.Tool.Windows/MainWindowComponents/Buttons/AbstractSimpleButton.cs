using System.Windows;
using JetBrains.Annotations;
using MIGA.Instances;
using MIGA.Tool.Base.Plugins;
using Sitecore.Diagnostics.Base;

namespace MIGA.Tool.Windows.MainWindowComponents.Buttons
{
  public abstract class WindowOnlyButton : IMainWindowButton
  {
    #region Public methods

    public virtual bool IsEnabled(Window mainWindow, Instance instance)
    {
      return true;
    }

    public virtual bool IsVisible(Window mainWindow, Instance instance)
    {
      return true;
    }

    public virtual void OnClick(Window mainWindow, Instance instance)
    {
      Assert.ArgumentNotNull(mainWindow, nameof(mainWindow));

      this.OnClick(mainWindow);
    }

    #endregion

    #region Protected methods

    protected abstract void OnClick([NotNull] Window mainWindow);

    #endregion
  }
}