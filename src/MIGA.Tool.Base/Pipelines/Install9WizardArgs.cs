using MIGA.Adapters.SqlServer;
using MIGA.IO;
using MIGA.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIGA.Instances;
using MIGA.Pipelines.Install;
using MIGA.Pipelines.Processors;
using MIGA.Sitecore9Installer;

namespace MIGA.Tool.Base.Pipelines
{
  public class Install9WizardArgs : InstallWizardArgs
  {
    public Install9WizardArgs() : base()
    {

    }

    public Install9WizardArgs(Instance instance) : base(instance)
    {

    }

    public override ProcessorArgs ToProcessorArgs()
    {
      Install9Args args= new Install9Args(this.Tasker);
      args.ScriptsOnly = this.ScriptsOnly;
      return args;
    }
    public string SolrUrl { get; set; }
    public string SorlRoot { get; set; }
    public string ScriptRoot { get; set; }
    public Tasker Tasker { get; set; }
    public bool ScriptsOnly { get; set; }
    public bool Validate { get; set; }
  }
}
