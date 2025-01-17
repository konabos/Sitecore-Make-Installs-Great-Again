using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using JetBrains.Annotations;
using MIGA.Instances;
using MIGA.Tool.Base;

namespace MIGA.Tool.Windows.MainWindowComponents.Buttons
{
  [UsedImplicitly]
  public class CollectMemoryDumpButton : InstanceOnlyButton
  {
    #region Public methods

    public override void OnClick(Window mainWindow, Instance instance)
    {
      if (instance == null)
      {
        var bitness = WindowHelper.AskForSelection("Managed Gc Aware Dump", null, "Choose version of the tool", new[] { "x86", "x64" }, mainWindow);
        if (bitness == null)
        {
          return;
        }

        var options = WindowHelper.Ask("Please specify params for Managed Gc Aware Dump", string.Empty, mainWindow);
        var executableFilePath = ApplicationManager.GetEmbeddedFile("ManagedGcAwareDump.zip", "MIGA.Tool.Windows", "ManagedGcAwareDump_" + bitness + ".exe");
        Process.Start(new ProcessStartInfo(executableFilePath, options)).WaitForExit();

        return;
      }

      var ids = instance.ProcessIds.ToArray();
      if (ids.Length == 0)
      {
        WindowHelper.HandleError("No running w3wp processes for this Sitecore instance", false);
        return;
      }

      var bit = instance.Is32Bit ? "x86" : "x64";
      foreach (var id in ids)
      {
        var defaultValue = id.ToString(CultureInfo.InvariantCulture);
        var options = WindowHelper.Ask("Please specify params for Managed Gc Aware Dump", defaultValue, mainWindow);
        if (string.IsNullOrEmpty(options))
        {
          return;
        }
        
        var executableFilePath = ApplicationManager.GetEmbeddedFile("ManagedGcAwareDump.zip", "MIGA.Tool.Windows", "ManagedGcAwareDump_" + bit + ".exe");
        Process.Start(executableFilePath).WaitForExit();
      }
    }

    #endregion
  }
}