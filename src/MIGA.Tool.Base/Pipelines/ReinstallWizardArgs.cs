using MIGA.Instances;
using MIGA.Pipelines.Install;
using MIGA.Pipelines.Processors;
using MIGA.Pipelines.Reinstall;
using MIGA.Sitecore9Installer;
using MIGA.Tool.Base.Wizards;

namespace MIGA.Tool.Base.Pipelines
{
  using System.Data.SqlClient;

  using MIGA.Instances;
  using MIGA.Pipelines.Install;
  using MIGA.Pipelines.Processors;
  using MIGA.Pipelines.Reinstall;
  using MIGA.Sitecore9Installer;
  using MIGA.Tool.Base.Wizards;

  public class ReinstallWizardArgs : WizardArgs
  {
    public ReinstallWizardArgs(Instance instance, SqlConnectionStringBuilder connectionString, string license)
    {
      Instance = instance;
      InstanceName = instance.Name;
      ConnectionString = connectionString;
      License = license;
    }

    public Instance Instance { get; }

    public string InstanceName { get; set; }

    public SqlConnectionStringBuilder ConnectionString { get; }

    private string License { get; }

    public Tasker Tasker { get; set; }

    public override ProcessorArgs ToProcessorArgs()
    {
      if (int.Parse(this.Instance.Product.ShortVersion) >= 90)
      {
        return new Reinstall9Args(this.Tasker);
      }

      return new ReinstallArgs(this.Instance, this.ConnectionString, this.License, Settings.CoreInstallWebServerIdentity.Value, Settings.CoreInstallNotFoundTransfer.Value);
    }
  }
}