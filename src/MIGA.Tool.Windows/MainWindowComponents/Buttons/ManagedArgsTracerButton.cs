using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using JetBrains.Annotations;
using MIGA.Instances;
using MIGA.Tool.Base;
using Sitecore.Diagnostics.Base;

namespace MIGA.Tool.Windows.MainWindowComponents.Buttons
{
  [UsedImplicitly]
  public class ManagedArgsTracerButton : InstanceOnlyButton
  {
    #region Public methods

    public override void OnClick(Window mainWindow, Instance instance)
    {
      Assert.ArgumentNotNull(mainWindow, nameof(mainWindow));

      if (instance == null)
      {
        Run(mainWindow, string.Empty);
        return;
      }

      var ids = (instance.ProcessIds ?? new int[0]).ToArray();
      if (ids.Length == 0)
      {
        WindowHelper.HandleError("No running w3wp processes for this Sitecore instance", false);
        return;
      }

      foreach (var id in ids)
      {
        var defaultValue = id.ToString(CultureInfo.InvariantCulture);
        if (Run(mainWindow, defaultValue))
        {
          return;
        }
      }
    }

    #endregion

    #region Private methods

    private static bool Run(Window mainWindow, string defaultValue)
    {
      var options = WindowHelper.Ask("Please specify params for Managed Args Tracer", defaultValue, mainWindow);
      if (string.IsNullOrEmpty(options))
      {
        return true;
      }

      Process.Start(new ProcessStartInfo("cmd.exe", "/K \"" + ApplicationManager.GetEmbeddedFile("ManagedArgsTracer.zip", "MIGA.Tool.Windows", "ManagedArgsTracer.exe") + " " + options + "\""));
      return false;
    }

    #endregion
  }
}