﻿using Sitecore.Diagnostics.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using MIGA.ContainerInstaller;
using MIGA.ContainerInstaller.Repositories.TagRepository;
using MIGA.Core;
using MIGA.Tool.Base;
using MIGA.Tool.Base.Pipelines;
using MIGA.Tool.Base.Profiles;
using MIGA.Tool.Base.Wizards;
using MIGA.Tool.Windows.Dialogs;
using Sitecore.Diagnostics.InfoService.Client.Helpers;

namespace MIGA.Tool.Windows.UserControls.Install.Containers
{
  /// <summary>
  /// Interaction logic for Instance9SelectTasks.xaml
  /// </summary>
  public partial class SelectTag : IWizardStep, IFlowControl, ICustomButton
  {
    private Window owner;
    private string productVersion;
    private string lastRegistry;
    private EnvModel envModel;
    private readonly ITagRepository tagRepository;
    private string defaultProjectName;

    public SelectTag()
    {
      InitializeComponent();
      this.tagRepository = GitHubTagRepository.GetInstance();
    }

    public void InitializeStep(WizardArgs wizardArgs)
    {
      Assert.ArgumentNotNull(wizardArgs, nameof(wizardArgs));
      InstallContainerWizardArgs args = (InstallContainerWizardArgs)wizardArgs;
      this.owner = args.WizardWindow;
      this.productVersion = args.Product.TwoVersion;
      string[] envFiles = Directory.GetFiles(args.FilesRoot, ".env", SearchOption.AllDirectories);
      string topologiesFolder = Directory.GetParent(envFiles[0]).Parent.FullName;
      this.Topologies.DataContext = Directory.GetDirectories(topologiesFolder).Where(d => File.Exists(Path.Combine(d, ".env"))).Select(d => new NameValueModel(Path.GetFileName(d), d));
      this.Topologies.SelectedIndex = 0;
      this.defaultProjectName = args.InstanceName;
      this.ProjectName.IsChecked = true;
    }

    public bool OnMovingBack(WizardArgs wizardArgs)
    {
      return true;
    }

    public bool OnMovingNext(WizardArgs wizardArgs)
    {
      Assert.ArgumentNotNull(wizardArgs, nameof(wizardArgs));
      InstallContainerWizardArgs args = (InstallContainerWizardArgs)wizardArgs;
      args.Tag = (string)this.Tags.SelectedValue;
      args.DockerRoot = ((NameValueModel)this.Topologies.SelectedItem).Value;
      this.envModel.SitecoreVersion = args.Tag;
      args.EnvModel = this.envModel;
      args.Topology = ((NameValueModel)this.Topologies.SelectedItem).Name.ToString();

      return true;
    }

    public bool SaveChanges(WizardArgs wizardArgs)
    {
      return true;
    }

    private string[] GetTags(string productVersion, string tagNameSpace)
    {
      return this.tagRepository.GetSortedShortSitecoreTags(productVersion, tagNameSpace).ToArray(); ;
    }

    private class NameValueModel
    {
      public NameValueModel(string name, string value)
      {
        this.Name = name;
        this.Value = value;
      }

      public string Name { get; }
      public string Value { get; }
    }

    private void Topologies_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      if (this.Topologies.SelectedItem == null)
      {
        return;
      }

      NameValueModel topology = (NameValueModel)this.Topologies.SelectedItem;
      string envPath = Path.Combine(topology.Value, ".env");
      this.envModel = this.CreateModel(envPath);
      if (this.lastRegistry == this.envModel.SitecoreRegistry)
      {
        this.UpdateProjectName();
        this.UpdateHosts();
        return;
      }

      this.lastRegistry = this.envModel.SitecoreRegistry;
      Uri registry = new Uri("https://" + this.envModel.SitecoreRegistry, UriKind.Absolute);
      this.Tags.DataContext = this.GetTags(this.productVersion, registry.LocalPath.Trim('/'));
      this.Tags.SelectedIndex = 0;
    }

    private EnvModel CreateModel(string envPath)
    {
      EnvModel model = EnvModel.LoadFromFile(envPath);

      if (string.IsNullOrWhiteSpace(model.SitecoreAdminPassword))
      {
        model.SitecoreAdminPassword = CoreAppSettings.AppLoginAsAdminNewPassword.Value;
      }

      if (string.IsNullOrWhiteSpace(model.SitecoreLicense))
      {
        model.SitecoreLicense = ProfileManager.Profile.License;
      }

      return model;
    }

    private void Tags_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      if (this.Tags.SelectedItem == null)
      {
        return;
      }

      string tag = (string)this.Tags.SelectedItem;
      this.envModel.SitecoreVersion = tag;
      this.UpdateProjectName();
      this.UpdateHosts();
    }

    private void UpdateHosts()
    {
      string hostNameTemplate = "{0}-{1}";
      string hostNameKeyPattern = "([A-Za-z0-9]{1,3})_HOST";

      Regex regex = new Regex(hostNameKeyPattern);

      if (string.IsNullOrEmpty(this.envModel.ProjectName))
      {
        return;
      }

      string[] keys = this.envModel.GetNames().ToArray();

      IEnumerable<string> hostNamesKeys = keys.Where(n => regex.IsMatch(n));

      foreach (var hostNameKey in hostNamesKeys)
      {
        string serviceName = regex.Match(hostNameKey).Groups[1].Value;

        if (string.IsNullOrEmpty(serviceName))
          continue;

        this.envModel[hostNameKey] = string.Format(hostNameTemplate, this.envModel.ProjectName, serviceName.ToLower());
      }
    }

    private void ProjectName_Checked(object sender, RoutedEventArgs e)
    {
      this.UpdateProjectName();
      this.UpdateHosts();
    }

    private void ProjectName_OnUnchecked(object sender, RoutedEventArgs e)
    {
      this.envModel.ProjectName = this.defaultProjectName;
    }

    private void UpdateProjectName()
    {
      if (ProjectName.IsChecked == true && this.Topologies.SelectedItem != null && this.Tags.SelectedItem != null)
      {
        this.envModel.ProjectName =
          $"{this.defaultProjectName}-{((NameValueModel)this.Topologies.SelectedItem).Name}-{this.Tags.SelectedItem}";
      }
    }

    public string CustomButtonText { get => "Advanced..."; }

    public void CustomButtonClick()
    {
      WindowHelper.ShowDialog<ContainerVariablesEditor>(this.envModel.ToList(), this.owner);
      this.UpdateTagsControl(this.envModel.SitecoreVersion);
    }

    private void UpdateTagsControl(string tag)
    {
      if (this.Topologies.SelectedItem != null && !string.Equals((string)this.Tags.SelectedItem, tag, StringComparison.InvariantCultureIgnoreCase))
      {
        foreach (string item in this.Tags.ItemsSource)
        {
          if (string.Equals(item, tag, StringComparison.InvariantCultureIgnoreCase))
          {
            this.Tags.SelectedItem = item;
            return;
          }
        }

        List<string> tagsItems = new List<string>(this.Tags.ItemsSource as IEnumerable<string>)
        {
          tag
        };
        this.Tags.DataContext = tagsItems;
        this.Tags.SelectedIndex = this.Tags.Items.Count - 1;
      }
    }
  }
}