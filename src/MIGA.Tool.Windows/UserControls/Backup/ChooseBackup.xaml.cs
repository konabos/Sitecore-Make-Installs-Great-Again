using MIGA.Instances;
using MIGA.Tool.Base.Pipelines;
using MIGA.Tool.Base.Wizards;

namespace MIGA.Tool.Windows.UserControls.Backup
{
  using System.Collections.Generic;
  using MIGA.Instances;
  using MIGA.Tool.Base.Pipelines;
  using MIGA.Tool.Base.Wizards;

  #region

  #endregion

  public partial class ChooseBackup : IWizardStep
  {
    #region Fields

    private readonly List<InstanceBackup> _CheckBoxItems = new List<InstanceBackup>();

    #endregion

    #region Constructors

    public ChooseBackup()
    {
      InitializeComponent();
    }

    #endregion

    #region IWizardStep Members

    void IWizardStep.InitializeStep(WizardArgs wizardArgs)
    {
      var args = (RestoreWizardArgs)wizardArgs;
      _CheckBoxItems.Clear();

      _CheckBoxItems.AddRange(args.Instance.Backups);


      backups.DataContext = _CheckBoxItems;
    }

    bool IWizardStep.SaveChanges(WizardArgs wizardArgs)
    {
      var args = (RestoreWizardArgs)wizardArgs;
      args.Backup = backups.SelectedItem as InstanceBackup;

      return true;
    }

    #endregion
  }
}