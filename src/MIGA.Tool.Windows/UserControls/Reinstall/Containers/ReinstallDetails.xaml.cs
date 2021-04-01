using System;
using System.Text;
using JetBrains.Annotations;
using System.Collections.Generic;
using MIGA.SitecoreEnvironments;
using MIGA.Tool.Base.Pipelines;
using MIGA.Tool.Base.Wizards;

namespace MIGA.Tool.Windows.UserControls.Reinstall.Containers
{
  [UsedImplicitly]
  public partial class ReinstallDetails : IWizardStep, IFlowControl
  {
    public ReinstallDetails()
    {
      InitializeComponent();
    }

    public bool OnMovingBack(WizardArgs wizardArg)
    {
      return true;
    }

    public bool OnMovingNext(WizardArgs wizardArgs)
    {
      return true;
    }

    void IWizardStep.InitializeStep(WizardArgs wizardArgs)
    {
      DeleteContainersWizardArgs args = (DeleteContainersWizardArgs)wizardArgs;

      StringBuilder displayText = new StringBuilder();

      SitecoreEnvironment environment;
      if (!SitecoreEnvironmentHelper.TryGetEnvironmentById(args.EnvironmentId, out environment))
      {
        throw new InvalidOperationException($"Could not resolve environment by ID'{args.EnvironmentId}'");
      }

      this.ListHeader.Text = string.Format("Reinstalling '{0}':", environment.Name);

      List<SitecoreEnvironmentMember> members = environment.Members;

      foreach (var member in members)
      {
        displayText.AppendLine(string.Format(" -{0}", member.Name));
      }

      this.TextBlock.Text = displayText.ToString();
    }

    bool IWizardStep.SaveChanges(WizardArgs wizardArgs)
    {
      return true;
    }
  }
}