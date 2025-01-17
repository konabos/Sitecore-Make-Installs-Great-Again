﻿using MIGA.Sitecore9Installer.Tasks;
using Sitecore.Diagnostics.Base;
using System.Linq;
using System.Windows;
using MIGA.Sitecore9Installer;
using MIGA.Tool.Base;
using MIGA.Tool.Base.Pipelines;
using MIGA.Tool.Base.Wizards;
using MIGA.Tool.Windows.UserControls.Install.ParametersEditor;
using TaskDialogInterop;

namespace MIGA.Tool.Windows.UserControls.Install
{
  /// <summary>
  /// Interaction logic for Instance9SelectTasks.xaml
  /// </summary>
  public partial class Instance9SelectTasks : IWizardStep, IFlowControl, ICustomButton
  {
    private Window owner;
    private Tasker tasker;
    public Instance9SelectTasks()
    {
      InitializeComponent();
    }

    public void InitializeStep(WizardArgs wizardArgs)
    {
      Assert.ArgumentNotNull(wizardArgs, nameof(wizardArgs));
      Install9WizardArgs args = (Install9WizardArgs)wizardArgs;
      this.owner = args.WizardWindow;
      this.tasker = args.Tasker;
      this.TasksList.DataContext = args.Tasker.Tasks.Where(t=>((t.SupportsUninstall()&&t.UnInstall)||!t.UnInstall)&&t.ExecutionOrder>=0);
    }    

    public bool OnMovingBack(WizardArgs wizardArgs)
    {
      return true;
    }

    public bool OnMovingNext(WizardArgs wizardArgs)
    {
      Assert.ArgumentNotNull(wizardArgs, nameof(wizardArgs));
      Install9WizardArgs args = (Install9WizardArgs)wizardArgs;
      if (!args.Tasker.Tasks.Any(t => t.ShouldRun))
      {
        MessageBox.Show("At least one task must be selected");
        return false;
      }

      var title = args.Tasker.UnInstall ? "Preparing for uninstall" : "Preparing for install";
      TaskDialogResult result= WindowHelper.LongRunningTask(() => this.RunLowLevelTasks(args.Tasker),title,this.owner);
      if (result == null)
      {
        return false;
      } 
      
      return true;
    }

    public bool SaveChanges(WizardArgs wizardArgs)
    {
      return true;
    }

    public string CustomButtonText { get=>"Advanced..."; }
    public void CustomButtonClick()
    {
      WindowHelper.ShowDialog<Install9ParametersEditor>(this.tasker, this.owner);
    }

    private void RunLowLevelTasks(Tasker tasker)
    {
      tasker.RunLowlevelTasks();
    }
  }
}
