﻿using MIGA.Core;
using MIGA.Tool.Base;
using MIGA.Tool.Base.Pipelines;
using MIGA.Tool.Windows.MainWindowComponents.Buttons;

namespace MIGA.Tool.Windows.Pipelines.Install
{
  using MIGA.Tool.Base;
  using MIGA.Tool.Base.Pipelines;
  using MIGA.Tool.Windows.MainWindowComponents.Buttons;
  using JetBrains.Annotations;

  using Sitecore.Diagnostics.Base;

  using MIGA.Core;

  [UsedImplicitly]
  public static class InstallModulesActions
  {
    [UsedImplicitly]
    public static void OpenBrowser(InstallModulesWizardArgs args)
    {
      InstanceHelperEx.BrowseInstance(args.Instance, args.WizardWindow, string.Empty, true);
    }

    [UsedImplicitly]
    public static void OpenSitecoreClient(InstallModulesWizardArgs args)
    {
      InstanceHelperEx.BrowseInstance(args.Instance, args.WizardWindow, "/sitecore", false);
    }

    [UsedImplicitly]
    public static void OpenVisualStudio(InstallModulesWizardArgs args)
    {
      new OpenVisualStudioButton().OnClick(args.WizardWindow.Owner, args.Instance);
    }

    [UsedImplicitly]
    public static void OpenWebsiteFolder(InstallModulesWizardArgs args)
    {
      CoreApp.OpenFolder(args.Instance.WebRootPath);
    }

    [UsedImplicitly]
    public static void LoginAdmin([NotNull] InstallModulesWizardArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      var instance = args.Instance;
      Assert.IsNotNull(instance, nameof(instance));

      InstanceHelperEx.OpenInBrowserAsAdmin(instance, MainWindow.Instance);
    }

    [UsedImplicitly]
    public static void PublishSite(InstallModulesWizardArgs args)
    {
      new PublishButton().OnClick(MainWindow.Instance, args.Instance);
    }
  }
}