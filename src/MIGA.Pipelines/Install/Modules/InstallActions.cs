﻿using MIGA.Instances;
using MIGA.Products;

namespace MIGA.Pipelines.Install.Modules
{
  using System.Collections.Generic;
  using System.Linq;
  using MIGA.Instances;
  using MIGA.Products;
  using Sitecore.Diagnostics.Base;
  using MIGA.Extensions;

  #region

  #endregion

  public class InstallActions : InstallProcessor
  {
    #region Fields

    private readonly List<Product> _Done = new List<Product>();

    #endregion

    #region Methods

    protected override bool IsRequireProcessing(InstallArgs args)
    {
      return !ProcessorDefinition.Param.EqualsIgnoreCase("archive") || (args._Modules != null && args._Modules.Any(m => m != null && m.IsArchive));
    }

    protected override void Process(InstallArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));
      Assert.IsNotNull(args.Instance, "Instance");

      Instance instance = args.Instance;
      IEnumerable<Product> modules = args._Modules;
      var param = ProcessorDefinition.Param;
      ConfigurationActions.ExecuteActions(instance, modules.ToArray(), _Done, param, args.ConnectionString, Controller);
    }

    #endregion
  }
}