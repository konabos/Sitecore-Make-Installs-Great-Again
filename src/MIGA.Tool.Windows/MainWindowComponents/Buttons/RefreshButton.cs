﻿using System;
using System.Windows;
using JetBrains.Annotations;
using MIGA.Extensions;
using Sitecore.Diagnostics.Base;
using Sitecore.Diagnostics.Logging;
using TaskDialogInterop;

namespace MIGA.Tool.Windows.MainWindowComponents.Buttons
{
  [UsedImplicitly]
  public class RefreshButton : WindowOnlyButton
  {
    #region Enums

    public enum RefreshMode
    {
      Undefined = -1, 
      Instances = 0, 
      Installer = 1, 
      Caches = 2, 
      Everything = 3, 
    }

    #endregion

    #region Fields

    private RefreshMode Mode { get; }

    #endregion

    #region Constructors

    public RefreshButton()
    {
      Mode = RefreshMode.Undefined;
    }

    public RefreshButton([NotNull] string param)
    {
      Assert.ArgumentNotNull(param, nameof(param));

      switch (param.ToLower())
      {
        case "all":
          Mode = RefreshMode.Everything;
          return;
        case "sites":
          Mode = RefreshMode.Instances;
          return;
        case "installer":
          Mode = RefreshMode.Installer;
          return;
        case "caches":
          Mode = RefreshMode.Caches;
          return;
        default:
          throw new NotSupportedException("The {0} type is not supported".FormatWith(param));
      }
    }

    #endregion

    #region Protected methods

    protected override void OnClick(Window mainWindow)
    {
      using (new ProfileSection("Refresh main window instances", this))
      {
        ProfileSection.Argument("mainWindow", mainWindow);

        var refreshMode = this.GetMode(mainWindow);
        switch (refreshMode)
        {
          case RefreshMode.Instances:
            MainWindowHelper.RefreshInstances();
            return;
          case RefreshMode.Installer:
            MainWindowHelper.RefreshInstaller();
            return;
          case RefreshMode.Caches:
            MainWindowHelper.RefreshCaches();
            return;
          case RefreshMode.Everything:
            MainWindowHelper.RefreshEverything();
            return;
        }
      }
    }

    #endregion

    #region Private methods

    private RefreshMode GetMode([NotNull] Window mainWindow)
    {
      Assert.ArgumentNotNull(mainWindow, nameof(mainWindow));

      if (Mode != RefreshMode.Undefined)
      {
        return Mode;
      }

      var config = new TaskDialogOptions
      {
        Owner = mainWindow, 
        Title = "Refresh", 
        MainInstruction = "Choose what would you like to refresh.", 
        Content = "The application has three kinds of storages that you can refresh.", 
        CommandButtons = new[]
        {
          // 0
          "Refresh sites list\nReading metadata to find Sitecore instances", 
          
          // 1
          "Refresh installer\nLooking for *.zip files in your local repository", 
          
          // 2
          "Refresh caches\nFlushing internal caches", 
          
          // 3
          "Refresh everything\nFlushing internal caches and refreshing instances and installer", 
        }, 
        MainIcon = VistaTaskDialogIcon.Information
      };

      var res = TaskDialog.Show(config);
      if (res == null)
      {
        return RefreshMode.Undefined;
      }

      var result = res.CommandButtonResult;
      if (result == null)
      {
        return RefreshMode.Undefined;
      }

      return (RefreshMode)((int)result);
    }

    #endregion
  }
}