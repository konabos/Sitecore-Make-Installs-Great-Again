﻿using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;
using MIGA.Tool.Base.Pipelines;
using MIGA.Tool.Base.Wizards;
using Sitecore.Diagnostics.Logging;

namespace MIGA.Tool.Windows.UserControls.Install
{
  using System;
  using MIGA.Tool.Base.Pipelines;
  using MIGA.Tool.Base.Wizards;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  public partial class InstanceRole : IWizardStep
  {
    public InstanceRole()
    {
      InitializeComponent();
    }

    public void InitializeStep([NotNull] WizardArgs wizardArgs)
    {
      Assert.ArgumentNotNull(wizardArgs, nameof(wizardArgs));

      InstallRoles.IsEnabled = false;
      InstallRoles.IsChecked = false;

      var args = (InstallWizardArgs)wizardArgs;
      try
      {
        var ver = args.Product.TwoVersion.Replace(".", "");
        if (ver.Length <= 2)
        {
          ver += Safe.Call(() => $"{args.Product.Update}") ?? "0";
        }
       
        var txt = int.Parse(ver);

        if (txt >= 813 && txt < 900)
        {
          InstallRoles.IsEnabled = true;
          if (!string.IsNullOrEmpty(args.InstallRoles8))
          {
            InstallRoles.IsChecked = true;
            var radio = (RadioButton)RoleName.FindName(args.InstallRoles8);
            Assert.IsNotNull(radio, $"{args.InstallRoles8} is not supported");

            radio.IsChecked = true;
          }
        }
        else
        {
          foreach (var radio in RoleName.Children.OfType<RadioButton>())
          {
            radio.IsChecked = false;
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(e, "Something is wrong");
      }
    }

    public bool SaveChanges([NotNull] WizardArgs wizardArgs)
    {
      Assert.ArgumentNotNull(wizardArgs, nameof(wizardArgs));

      var args = (InstallWizardArgs)wizardArgs;
      if (InstallRoles.IsChecked != true)
      {
        args.InstallRoles8 = "";
        InstallWizardArgs.SaveLastTimeOption(nameof(args.InstallRoles8), args.InstallRoles8);

        return true;
      }

      var role = RoleName.Children.OfType<RadioButton>().FirstOrDefault(x => x.IsChecked == true).IsNotNull("role").Name;

      args.InstallRoles8 = role;
      InstallWizardArgs.SaveLastTimeOption(nameof(args.InstallRoles8), args.InstallRoles8);

      return true;
    }

    private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
    {
      Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
      e.Handled = true;
    }
  }
}
