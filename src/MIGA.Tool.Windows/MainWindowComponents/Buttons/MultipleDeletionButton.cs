using System.Collections.Generic;
using System.Windows;
using JetBrains.Annotations;
using MIGA.Pipelines.MultipleDeletion;
using MIGA.Tool.Base.Wizards;
using MIGA.Tool.Windows.UserControls.MultipleDeletion;

namespace MIGA.Tool.Windows.MainWindowComponents.Buttons
{
  [UsedImplicitly]
  public class MultipleDeletionButton : WindowOnlyButton
  {
    #region Protected methods

    protected override void OnClick(Window mainWindow)
    {
      WizardPipelineManager.Start("multipleDeletion", mainWindow, new MultipleDeletionArgs(new List<string>()), null, ignore => OnWizardCompleted(), () => new MultipleDeletionWizardArgs());
    }

    #endregion

    #region Private methods

    private static void OnWizardCompleted()
    {
      MainWindowHelper.SoftlyRefreshInstances();
    }

    #endregion
  }
}