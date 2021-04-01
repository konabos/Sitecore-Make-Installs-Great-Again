using MIGA.Pipelines.MultipleDeletion;
using MIGA.Pipelines.Processors;
using MIGA.Tool.Base.Profiles;
using MIGA.Tool.Base.Wizards;

namespace MIGA.Tool.Windows.UserControls.MultipleDeletion
{
  using System.Collections.Generic;
  using MIGA.Pipelines.MultipleDeletion;
  using MIGA.Pipelines.Processors;
  using MIGA.Tool.Base.Profiles;
  using MIGA.Tool.Base.Wizards;

  public class MultipleDeletionWizardArgs : WizardArgs
  {
    #region Fields

    public List<string> _SelectedInstances;

    #endregion

    #region Public methods

    public override ProcessorArgs ToProcessorArgs()
    {
      return new MultipleDeletionArgs(_SelectedInstances)
      {
        _ConnectionString = ProfileManager.GetConnectionString()
      };
    }

    #endregion
  }
}