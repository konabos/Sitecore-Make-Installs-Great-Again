using System.Data.SqlClient;
using System.Windows;
using JetBrains.Annotations;
using MIGA.Core.Common;
using MIGA.Instances;
using MIGA.IO.Real;
using MIGA.Pipelines.Restore;
using MIGA.Tool.Base.Pipelines;
using MIGA.Tool.Base.Wizards;

namespace MIGA.Tool.Windows.MainWindowComponents.Buttons
{
  [UsedImplicitly]
  public class RestoreInstanceButton : InstanceOnlyButton
  {
    #region Public methods

    public override void OnClick(Window mainWindow, Instance instance)
    {
      if (instance != null)
      {
        var args = new RestoreArgs(instance, new SqlConnectionStringBuilder(Profile.Read(new RealFileSystem()).ConnectionString));
        var id = MainWindowHelper.GetListItemID(instance.ID);
        WizardPipelineManager.Start("restore", mainWindow, args, null, ignore => MainWindowHelper.MakeInstanceSelected(id), () => new RestoreWizardArgs(instance));
      }
    }

    #endregion
  }
}