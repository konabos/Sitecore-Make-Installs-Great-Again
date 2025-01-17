﻿using System.Windows;
using JetBrains.Annotations;
using MIGA.Instances;
using MIGA.Tool.Base;
using Sitecore.Diagnostics.Base;

namespace MIGA.Tool.Windows.MainWindowComponents.Buttons
{
  [UsedImplicitly]
  public class LoginAdminButton : InstanceOnlyButton
  {
    #region Fields

    protected string Browser { get; }
    protected string VirtualPath { get; }
    protected readonly string[] _Params;

    #endregion

    #region Constructors

    public LoginAdminButton()
    {
      VirtualPath = string.Empty;
      Browser = string.Empty;
      _Params = new string[0];
    }

    public LoginAdminButton([NotNull] string param)
    {
      Assert.ArgumentNotNull(param, nameof(param));

      var par = Parameters.Parse(param);
      VirtualPath = par[0];
      Browser = par[1];
      _Params = par.Skip(2);
    }

    #endregion

    #region Public methods

    public override void OnClick(Window mainWindow, Instance instance)
    {
      Assert.ArgumentNotNull(mainWindow, nameof(mainWindow));
      Assert.IsNotNull(instance, nameof(instance));

      InstanceHelperEx.OpenInBrowserAsAdmin(instance, mainWindow, VirtualPath, Browser, _Params);
    }

    #endregion
  }
}