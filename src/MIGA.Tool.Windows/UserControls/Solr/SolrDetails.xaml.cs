using MIGA.Sitecore9Installer;
using MIGA.Sitecore9Installer.Configuration;
using MIGA.Tool.Base;
using MIGA.Tool.Base.Pipelines;
using MIGA.Tool.Base.Wizards;

namespace MIGA.Tool.Windows.UserControls.Install
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.IO;
  using System.IO.Compression;
  using System.Linq;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Controls.Primitives;
  using System.Xml;
  using MIGA.Adapters.SqlServer;
  using MIGA.Adapters.WebServer;
  using MIGA.Instances;
  using MIGA.Products;
  using MIGA.Tool.Base;
  using MIGA.Tool.Base.Pipelines;
  using MIGA.Tool.Base.Profiles;
  using MIGA.Tool.Base.Wizards;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;
  using Sitecore.Diagnostics.Logging;
  using MIGA.Extensions;
  using MIGA.IO.Real;
  using MIGA.Sitecore9Installer;
  using MIGA.Tool.Windows.Dialogs;
  using MIGA.Tool.Windows.UserControls.Helpers;
  using MIGA.Tool.Windows.UserControls.Install.ParametersEditor;
  using Newtonsoft.Json.Linq;
  using MIGA.Sitecore9Installer.Configuration;

  #region

  #endregion

  [UsedImplicitly]
  public partial class SolrDetails : IWizardStep, IFlowControl
  {
    private Tasker tasker;
    #region Constructors

    public SolrDetails()
    {
      InitializeComponent();

    }

     #endregion

    #region Public Methods

    public bool OnMovingBack(WizardArgs wizardArg)
    {
      return true;
    }

    public bool OnMovingNext(WizardArgs wizardArgs)
    {
      string solrVersion = this.solrVersionSelector.SelectedValue as string;     
      tasker.GlobalParams.First(p => p.Name == "SolrVersion").Value = solrVersion;      
      tasker.GlobalParams.First(p => p.Name == "SolrDomain").Value = this.solrHostSelector.Text;

      if (!string.IsNullOrEmpty(this.solrPortSelector.Text))
      {
        tasker.GlobalParams.First(p => p.Name == "SolrPort").Value = this.solrPortSelector.Text;
      }      

      if (!string.IsNullOrEmpty(this.solrFolderSelector.Text))
      {
        tasker.GlobalParams.First(p => p.Name == "SolrInstallRoot").Value = this.solrFolderSelector.Text;
      }

      string javaHomeVariable = Environment.GetEnvironmentVariable("JAVA_HOME");
      if (string.IsNullOrEmpty(javaHomeVariable))
      {
        MessageBox.Show("Required JAVA_HOME system variable was not found.\nIt seems Java Runtime Environment (JRE) has not been installed.", "Warning");
        return false;
      }
      else if(!Directory.Exists(Path.Combine(javaHomeVariable, "bin")) || Directory.GetFiles(Path.Combine(javaHomeVariable, "bin"),"java.exe").Length==0)
      {
        MessageBox.Show($"The 'JAVA_HOME' system variable does not point to the proper JRE folder: {javaHomeVariable}", "Warning");
        return false;
      }

      tasker.GlobalParams.First(p => p.Name == "JavaHome").Value = javaHomeVariable;
      Install9WizardArgs args = (Install9WizardArgs)wizardArgs;
      args.Tasker = this.tasker;
      return true;
    }
    #endregion

    #region Private Methods
    private void SolrVersionSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      string solrVersion = this.solrVersionSelector.SelectedValue as string;
      this.solrHostSelector.Text = "solr" + solrVersion.Replace(".", string.Empty);
    }
    
    private void LocationBtn_Click(object sender, RoutedEventArgs e)
    {
      WindowHelper.PickFolder("Choose location folder", this.solrFolderSelector, null);
    }

    private void FillSolrVersions()
    {
      this.solrVersionSelector.DataContext = Configuration.Instance.SolrMap.Keys;
    }
    #endregion
    #region IWizardStep Members

    void IWizardStep.InitializeStep(WizardArgs wizardArgs)
    {
      string root = Path.Combine(Directory.GetCurrentDirectory(), "CustomSifConfig/Solr");
      this.tasker = new Tasker(root, "solr", null);
      this.FillSolrVersions();
      this.solrPortSelector.Text = tasker.Tasks.First(t => t.Name == "Solr").LocalParams.First(p => p.Name == "SolrPort").Value;
      this.solrFolderSelector.Text = tasker.Tasks.First(t => t.Name == "Solr").LocalParams.First(p => p.Name == "SolrInstallRoot").Value;
    }    

    bool IWizardStep.SaveChanges(WizardArgs wizardArgs)
    {
      return true;
    }

    #endregion
    
  }
}
