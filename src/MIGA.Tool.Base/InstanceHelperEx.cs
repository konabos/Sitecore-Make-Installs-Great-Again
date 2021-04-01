using MIGA.Core;
using MIGA.Instances;

namespace MIGA.Tool.Base
{
  using System;
  using System.IO;
  using System.Linq;
  using System.Threading;
  using System.Windows;
  using Microsoft.Web.Administration;
  using MIGA.Instances;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;
  using Sitecore.Diagnostics.Logging;
  using MIGA.Core;
  using MIGA.Extensions;

  public static class InstanceHelperEx
  {
    #region Public methods

    public static void BrowseInstance([NotNull] Instance instance, [NotNull] Window owner, [NotNull] string virtualPath, bool isFrontEnd, [CanBeNull] string browser = null, [CanBeNull] string[] parameters = null)
    {
      Assert.ArgumentNotNull(instance, nameof(instance));
      Assert.ArgumentNotNull(owner, nameof(owner));
      Assert.ArgumentNotNull(virtualPath, nameof(virtualPath));

      if (!EnsureAppPoolState(instance, owner))
      {
        return;
      }

      Browse(instance, virtualPath, isFrontEnd, browser, parameters);
    }

    public static void Browse(Instance instance, string virtualPath, bool isFrontEnd, string browser, string[] parameters)
    {
      var url = instance.GetUrl();
      if (!string.IsNullOrEmpty(url))
      {
        url += '/' + virtualPath.TrimStart('/');
        CoreApp.OpenInBrowser(url, isFrontEnd, browser, parameters);
      }
    }

    public static void OpenCurrentLogFile([NotNull] Instance instance, [NotNull] Window owner, [CanBeNull] string logFileType = null)
    {
      Assert.ArgumentNotNull(instance, nameof(instance));
      Assert.ArgumentNotNull(owner, nameof(owner));

      var dataFolderPath = instance.DataFolderPath;
      FileSystem.FileSystem.Local.Directory.AssertExists(dataFolderPath, "The data folder ({0}) of the {1} instance doesn't exist".FormatWith(dataFolderPath, instance.Name));

      var logsFolderPath = instance.LogsFolderPath;
      var logFilePrefix = logFileType ?? GetLogFileTypes(owner, logsFolderPath);
      if (logFilePrefix == null)
      {
        Action waitForLogs = delegate
        {
          while (logFilePrefix == null)
          {
            logFilePrefix = GetLogFileTypes(owner, logsFolderPath);
            Thread.Sleep(100);
          }
        };

        WindowHelper.LongRunningTask(waitForLogs, "Waiting for log files", owner, null, "Waiting for log files to be created in the \"{0}\" folder.".FormatWith(logsFolderPath));
      }

      var logFilePattern = logFilePrefix + "*.txt";
      var files = FileSystem.FileSystem.Local.Directory.GetFiles(logsFolderPath, logFilePattern) ?? new string[0];
      var logFilePath = files.Where(x => Path.GetFileNameWithoutExtension(x).StartsWith(logFilePrefix + ".")).OrderByDescending(FileSystem.FileSystem.Local.File.GetCreationTimeUtc).FirstOrDefault();
      if (string.IsNullOrEmpty(logFilePath))
      {
        Action waitForLogs = delegate
        {
          while (string.IsNullOrEmpty(logFilePath))
          {
            var files2 = FileSystem.FileSystem.Local.Directory.GetFiles(logsFolderPath, logFilePattern) ?? new string[0];
            logFilePath = files2.OrderByDescending(FileSystem.FileSystem.Local.File.GetCreationTimeUtc).FirstOrDefault();
            Thread.Sleep(100);
          }
        };

        WindowHelper.LongRunningTask(waitForLogs, "Waiting for log files", owner, null, "Waiting for log files to be created in the \"{0}\" folder.".FormatWith(logsFolderPath));
      }

      var logViewer = GetLogViewer();
      if (string.IsNullOrEmpty(logViewer))
      {
        return;
      }

      var fileSystemWatcher = new FileSystemWatcher(logsFolderPath)
      {
        Filter = logFilePattern, 
        IncludeSubdirectories = false
      };

      var reopenLogViewer = false;

      var currentProcess = CoreApp.RunApp(logViewer, logFilePath);
      if (currentProcess == null)
      {
        return;
      }
      
      // we need to stop all this magic when application closes
      currentProcess.Exited += delegate
      {
        // but shouldn't if it is initiated by this magic
        if (reopenLogViewer)
        {
          reopenLogViewer = false;
          return;
        }

        fileSystemWatcher.EnableRaisingEvents = false;
      };

      fileSystemWatcher.Created += (sender, args) =>
      {
        try
        {
          if (args.ChangeType != WatcherChangeTypes.Created)
          {
            return;
          }

          var filePath = args.FullPath;
          if (!Path.GetFileNameWithoutExtension(filePath).StartsWith(logFilePrefix + "."))
          {
            return;
          }

          // indicate that magic begins
          reopenLogViewer = true;

          // magic begins
          currentProcess.Kill();

          currentProcess = CoreApp.RunApp(logViewer, filePath);

          // we need to stop all this magic when application closes
          currentProcess.Exited += delegate
          {
            // but shouldn't if it is initiated by this magic
            if (reopenLogViewer)
            {
              reopenLogViewer = false;
              return;
            }

            fileSystemWatcher.EnableRaisingEvents = false;
          };         
        }
        catch (Exception ex)
        {
          fileSystemWatcher.EnableRaisingEvents = false;
          Log.Error(ex, "Unhandled error happened while reopening log file");
        }
      };

      fileSystemWatcher.EnableRaisingEvents = true;
    }

    private static string GetLogViewer()
    {
      var logviewer = WinAppSettings.AppToolsLogViewer.Value;
      if (string.IsNullOrEmpty(logviewer))
      {
        return null;
      }

      if (logviewer != "logview.exe")
      {
        return logviewer;
      }

      return ApplicationManager.GetEmbeddedFile("logview.zip", "MIGA.Tool.Windows", "logview.exe");
    }

    public static void OpenInBrowserAsAdmin([NotNull] Instance instance, [NotNull] Window owner, [CanBeNull] string pageUrl = null, [CanBeNull] string browser = null, [CanBeNull] string[] parameters = null)
    {
      Assert.ArgumentNotNull(instance, nameof(instance));
      Assert.ArgumentNotNull(owner, nameof(owner));

      AuthenticationHelper.LoginAsAdmin(instance, owner, pageUrl, browser, parameters);
    }

    public static bool PreheatInstance(Instance instance, Window mainWindow)
    {
      if (!EnsureAppPoolState(instance, mainWindow))
      {
        return false;
      }

      if (!WinAppSettings.AppPreheatEnabled.Value)
      {
        return true;
      }

      // Check if the instance is responsive now
      if (!InstanceHelper.IsInstanceResponsive(instance, "fast"))
      {
        // It is not responsive so we need to preheat it
        // i.e. request with larger timeout and with the 
        // progress bar shown to the user to avoid UI lag
        Exception ex = null;
        var res = WindowHelper.LongRunningTask(() => PreheatInstance(instance, out ex), "Starting Sitecore", mainWindow, 
          "Sitecore is being initialized", 
          "It may take up to a few minutes on large solutions or slow machines.", 
          true, true, true);
        if (res == null)
        {
          return false;
        }

        // if error happened
        if (ex != null)
        {
          const string Cancel = "Cancel";
          const string OpenLog = "Open MIGA log file";
          const string OpenSitecoreLog = "Open Sitecore log file";
          const string OpenAnyway = "Open in browser";
          var message = "The instance returned an error. \n\n" + ex.Message;
          Log.Error(ex, message);
          var result = WindowHelper.AskForSelection("Running instance failed", null, message, 
            new[]
            {
              Cancel, OpenLog, OpenSitecoreLog, OpenAnyway
            }, mainWindow);
          switch (result)
          {
            case OpenLog:
              CoreApp.OpenFile(ApplicationManager.LogsFolder);
              return false;
            case OpenSitecoreLog:
              OpenCurrentLogFile(instance, mainWindow);
              return false;
            case OpenAnyway:
              return true;
            default:
              return false;
          }
        }
      }

      return true;
    }

    #endregion

    #region Private methods

    private static bool EnsureAppPoolState([NotNull] Instance instance, [NotNull] Window mainWindow)
    {
      Assert.ArgumentNotNull(instance, nameof(instance));
      Assert.ArgumentNotNull(mainWindow, nameof(mainWindow));

      var state = instance.ApplicationPoolState;
      if (state == ObjectState.Stopped || state == ObjectState.Stopping)
      {
        const string Cancel = "Cancel";
        const string Start = "Start";
        const string Skip = "Skip, open anyway";

        var result = WindowHelper.AskForSelection("Instance is stopped", null, "The selected Sitecore instance is stopped. Would you like to start it first?", new[]
        {
          Cancel, Start, Skip
        }, mainWindow, Start);

        if (result == null || result == Cancel)
        {
          return false;
        }

        if (result == Start)
        {
          instance.Start();
        }
      }

      return true;
    }

    // public static void ToggleFavorite(Window mainWindow, Instance instance)
    // {
    // FavoriteManager.ToggleFavorite(instance.Name);
    // }
    [CanBeNull]
    private static string GetLogFileTypes(Window owner, string logsFolderPath)
    {
      const string Suffix = ".txt";
      const string Pattern = "*" + Suffix;
      var files = FileSystem.FileSystem.Local.Directory.GetFiles(logsFolderPath, Pattern);

      var groups = InstanceHelper.GetLogGroups(files);

      if (groups.Any())
      {
        return WindowHelper.AskForSelection("Open current log file", "Choose log file type", "There are several types of log files in Sitecore. Please choose what type do you need?", groups, owner, groups.First(), false, true);
      }
      
      return null;
    }

    private static void PreheatInstance(Instance instance, out Exception exception)
    {
      try
      {
        InstanceHelper.StartInstance(instance, null, "long");
        exception = null;
      }
      catch (Exception ex)
      {
        exception = ex;
      }
    }

    #endregion
  }
}