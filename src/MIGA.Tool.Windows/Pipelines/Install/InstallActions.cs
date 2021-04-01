using MIGA.Core;
using MIGA.Instances;
using MIGA.Pipelines.Backup;
using MIGA.Tool.Base;
using MIGA.Tool.Base.Pipelines;
using MIGA.Tool.Base.Profiles;
using MIGA.Tool.Base.Wizards;
using MIGA.Tool.Windows.MainWindowComponents.Buttons;
using MIGA.Tool.Windows.UserControls.Backup;

namespace MIGA.Tool.Windows.Pipelines.Install
{
  #region

  using System;

  using MIGA.Pipelines.Backup;
  using MIGA.Tool.Base;
  using MIGA.Tool.Base.Pipelines;
  using MIGA.Tool.Windows.MainWindowComponents.Buttons;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;
  using MIGA.Core;
  using MIGA.Extensions;
  using MIGA.Instances;
  using MIGA.Tool.Base.Wizards;
  using MIGA.Tool.Windows.UserControls.Backup;
  using MIGA.Tool.Base.Profiles;

  #endregion

  [UsedImplicitly]
  public static class InstallActions
  {
    #region Public methods

    [UsedImplicitly]
    public static void BackupInstance(InstallModulesWizardArgs args)
    {
      var id = MainWindowHelper.GetListItemID(args.Instance.ID);
      Assert.IsTrue(id >= 0, "id ({0}) should be >= 0".FormatWith(id));
      WizardPipelineManager.Start("backup", args.WizardWindow, new BackupArgs(args.Instance, ProfileManager.GetConnectionString(), null, true, true), null, ignore => MainWindowHelper.MakeInstanceSelected(id), () => new BackupSettingsWizardArgs(args.Instance));
    }

    [UsedImplicitly]
    public static void BackupInstance(InstallWizardArgs args)
    {
      var id = MainWindowHelper.GetListItemID(args.Instance.ID);
      Assert.IsTrue(id >= 0, "id ({0}) should be >= 0".FormatWith(id));
      WizardPipelineManager.Start("backup", args.WizardWindow, new BackupArgs(args.Instance, ProfileManager.GetConnectionString(), null, true, true), null, ignore => MainWindowHelper.MakeInstanceSelected(id), () => new BackupSettingsWizardArgs(args.Instance));
    }

    [UsedImplicitly]
    public static void OpenBrowser(InstallWizardArgs args)
    {
      InstanceHelperEx.BrowseInstance(args.Instance, args.WizardWindow, String.Empty, true);
    }

    [UsedImplicitly]
    public static void OpenSitecoreClient(InstallWizardArgs args)
    {
      InstanceHelperEx.BrowseInstance(args.Instance, args.WizardWindow, "/sitecore", false);
    }

    [UsedImplicitly]
    public static void OpenVisualStudio(InstallWizardArgs args)
    {
      new OpenVisualStudioButton().OnClick(args.WizardWindow.Owner, args.Instance);
    }

    [UsedImplicitly]
    public static void OpenWebsiteFolder(InstallWizardArgs args)
    {
      CoreApp.OpenFolder(args.InstanceWebRootPath);
    }

    [UsedImplicitly]
    public static void LoginAdmin([NotNull] InstallWizardArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      var instance = args.Instance;
      Assert.IsNotNull(instance, nameof(instance));

      InstanceHelperEx.OpenInBrowserAsAdmin(instance, MainWindow.Instance);
    }

    [UsedImplicitly]
    public static void PublishSite(InstallWizardArgs args)
    {
      MainWindowHelper.RefreshInstances();
      var instance = InstanceManager.Default.GetInstance(args.InstanceName);
      new PublishButton().OnClick(MainWindow.Instance, instance);
    }

    #endregion
  }
}