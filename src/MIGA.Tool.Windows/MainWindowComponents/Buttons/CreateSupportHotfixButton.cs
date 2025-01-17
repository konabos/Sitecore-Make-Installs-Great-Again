﻿using System;
using System.IO;
using System.Windows;
using Sitecore.Diagnostics.Base;
using JetBrains.Annotations;
using MIGA.Core;
using MIGA.Instances;
using MIGA.Tool.Base;

namespace MIGA.Tool.Windows.MainWindowComponents.Buttons
{
  [UsedImplicitly]
  public class CreateSupportHotfixButton : InstanceOnlyButton
  {
    private string AppArgsFilePath { get; }
    private string AppUrl { get; }

    #region Public methods

    public CreateSupportHotfixButton(string appArgsFilePath, string appUrl)
    {
      AppArgsFilePath = appArgsFilePath;
      AppUrl = appUrl;
    }

    public override void OnClick(Window mainWindow, Instance instance)
    {
      if (instance == null)
      {
        WindowHelper.ShowMessage("Choose an instance first");

        return;
      }

      var product = instance.Product;
      Assert.IsNotNull(product,
        $"The {instance.ProductFullName} distributive is not available in local repository. You need to get it first.");

      var version = product.TwoVersion + "." + product.Update;

      var args = new[]
      {
        version,
        instance.Name,
        instance.WebRootPath
      };

      var dir = Environment.ExpandEnvironmentVariables(AppArgsFilePath);
      if (!Directory.Exists(dir))
      {
        Directory.CreateDirectory(dir);
      }

      File.WriteAllLines(Path.Combine(dir, "args.txt"), args);

      CoreApp.RunApp("iexplore", AppUrl);
    }

    #endregion
  }
}
