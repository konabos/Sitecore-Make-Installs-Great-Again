﻿using MIGA.Instances;

namespace MIGA.Tool.Base.Plugins
{
  using System.Windows;
  using MIGA.Instances;
  using JetBrains.Annotations;

  public interface IMainWindowButton
  {
    #region Public methods

    bool IsEnabled([NotNull] Window mainWindow, [CanBeNull] Instance instance);

    bool IsVisible([NotNull] Window mainWindow, [CanBeNull] Instance instance);

    void OnClick([NotNull] Window mainWindow, [CanBeNull] Instance instance);

    #endregion
  }
}