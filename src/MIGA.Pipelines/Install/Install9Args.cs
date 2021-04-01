using JetBrains.Annotations;
using MIGA.IO;
using MIGA.Pipelines.Install;
using MIGA.Products;
using Sitecore.Diagnostics.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIGA.Pipelines.Processors;
using MIGA.Sitecore9Installer;

namespace MIGA.Pipelines.Install
{
  public class Install9Args : ProcessorArgs
  {    
   public Install9Args(Tasker tasker)
    {
      Assert.ArgumentNotNull(tasker, nameof(tasker));
      this.Tasker = tasker;
    }

    public bool ScriptsOnly { get; set; }
    public Tasker Tasker { get; }
    public string UnInstallDataPath { get; set; }
    
  }
}
