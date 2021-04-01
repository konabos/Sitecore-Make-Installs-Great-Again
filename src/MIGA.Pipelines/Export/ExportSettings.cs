﻿using MIGA.Adapters.WebServer;

namespace MIGA.Pipelines.Export
{
  using System.Diagnostics;
  using System.IO;
  using MIGA.Adapters.WebServer;

  internal class ExportSettings : ExportProcessor
  {
    #region Protected methods

    protected override void Process(ExportArgs args)
    {
      var websiteName = args.Instance.Name;
      using (var context = new WebServerManager.WebServerContext())
      {
        var appPoolName = new Website(args.Instance.ID).GetPool(context).Name;

        var websiteSettingsCommand = string.Format(@"%windir%\system32\inetsrv\appcmd list site {0}{1}{0} /config /xml > {2}", '"', websiteName, Path.Combine(args.Folder, "WebsiteSettings.xml"));
        var appPoolSettingsCommand = string.Format(@"%windir%\system32\inetsrv\appcmd list apppool {0}{1}{0} /config /xml > {2}", '"', appPoolName, Path.Combine(args.Folder, "AppPoolSettings.xml"));

        ExecuteCommand(websiteSettingsCommand);
        ExecuteCommand(appPoolSettingsCommand);
      }
    }

    #endregion

    #region Private methods

    private static void ExecuteCommand(string command)
    {
      var procStartInfo = new ProcessStartInfo("cmd", $"/c {command}")
      {
        UseShellExecute = false, 
        CreateNoWindow = true
      };

      var proc = new Process
      {
        StartInfo = procStartInfo
      };
      proc.Start();
      proc.WaitForExit();
    }

    #endregion
  }
}