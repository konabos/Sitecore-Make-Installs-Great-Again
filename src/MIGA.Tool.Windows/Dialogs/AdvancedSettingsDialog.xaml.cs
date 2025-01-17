﻿using MIGA.Adapters.SqlServer;
using MIGA.Core;
using MIGA.Products;
using MIGA.Tool.Base;
using MIGA.Tool.Base.Profiles;
using MIGA.Tool.Windows.Properties;

namespace MIGA.Tool.Windows.Dialogs
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows;
  using System.Windows.Input;
  using MIGA.Tool.Base;
  using MIGA.Tool.Base.Profiles;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;
  using MIGA.Core;
  using MIGA.Products;

  #region

  #endregion

  public partial class AdvancedSettingsDialog
  {
    #region Constructors

    public AdvancedSettingsDialog()
    {
      // workaround for #179
      var types = new[]
      {
        typeof(CoreAppSettings), 
        typeof(WinAppSettings), 
        typeof(Settings), 
        typeof(WindowsSettings), 
        typeof(MIGA.Pipelines.Install.Settings), 
        typeof(SqlServerManager.Settings), 
        typeof(WebRequestHelper.Settings),
        typeof(ProductHelper.Settings),
        typeof(EnvironmentHelper)
      };

      foreach (Type type in types)
      {
        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);
      }

      InitializeComponent();
    }

    #endregion

    #region Properties

    [NotNull]
    private Profile Profile
    {
      get
      {
        return (Profile)DataContext;
      }

      set
      {
        Assert.ArgumentNotNull(value, nameof(value));

        DataContext = value;
      }
    }

    #endregion

    #region Methods

    private void CancelChanges([CanBeNull] object sender, [CanBeNull] RoutedEventArgs e)
    {
      Close();
    }

    private void ContentLoaded(object sender, EventArgs e)
    {
      DataGrid.DataContext = GetAdvancedProperties();
    }

    private IEnumerable<AdvancedPropertyBase> GetAdvancedProperties()
    {
      var pluginPrefix = "App/Plugins/";

      var nonPluginsSettings = new List<AdvancedPropertyBase>();
      var pluginsSettings = new List<AdvancedPropertyBase>();

      foreach (KeyValuePair<string, AdvancedPropertyBase> keyValuePair in AdvancedSettingsManager.RegisteredSettings)
      {
        if (keyValuePair.Key.StartsWith(pluginPrefix))
        {
          pluginsSettings.Add(keyValuePair.Value);
        }
        else
        {
          nonPluginsSettings.Add(keyValuePair.Value);
        }
      }

      List<AdvancedPropertyBase> result = nonPluginsSettings.OrderBy(prop => prop.Name).ToList();
      result.AddRange(pluginsSettings.OrderBy(prop => prop.Name));
      return result;
    }

    private void OpenDocumentation([CanBeNull] object sender, [CanBeNull] RoutedEventArgs e)
    {
      CoreApp.OpenInBrowser("https://github.com/sitecore/sitecore-instance-manager/wiki/Advanced", true);
    }

    private void OpenFile(object sender, RoutedEventArgs e)
    {
      CoreApp.OpenFolder(ApplicationManager.DataFolder);
    }

    private void SaveSettings()
    {
      ProfileManager.SaveChanges(Profile);
      DialogResult = true;
      Close();
    }

    private void WindowKeyUp([NotNull] object sender, [NotNull] KeyEventArgs e)
    {
      Assert.ArgumentNotNull(sender, nameof(sender));
      Assert.ArgumentNotNull(e, nameof(e));

      if (e.Key == Key.Escape)
      {
        if (e.Handled)
        {
          return;
        }

        e.Handled = true;
        Close();
      }
    }

    #endregion
  }
}