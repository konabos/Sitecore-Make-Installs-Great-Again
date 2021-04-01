using System.Windows;
using JetBrains.Annotations;
using MIGA.Instances;
using MIGA.Pipelines.Export;
using MIGA.Tool.Base.Wizards;
using MIGA.Tool.Windows.UserControls.Export;

namespace MIGA.Tool.Windows.MainWindowComponents.Buttons
{
  [UsedImplicitly]
  public class ExportInstanceButton : InstanceOnlyButton
  {
    #region Public methods

    public override void OnClick(Window mainWindow, Instance instance)
    {
      if (instance != null)
      {
        var id = MainWindowHelper.GetListItemID(instance.ID);
        WizardPipelineManager.Start("export", mainWindow, new ExportArgs(instance, false, true, true, true, false, false, false, false, false), null, ignore => MainWindowHelper.MakeInstanceSelected(id), () => new ExportWizardArgs(instance, string.Empty));
      }
    }

    #endregion
  }
}