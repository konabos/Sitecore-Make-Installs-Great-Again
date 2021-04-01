﻿using MIGA.Instances;
using MIGA.Pipelines.InstallModules;
using MIGA.Pipelines.Processors;
using MIGA.Products;
using MIGA.Tool.Base.Profiles;
using MIGA.Tool.Base.Wizards;

namespace MIGA.Tool.Base.Pipelines
{
  using System;
  using System.Collections.Generic;
  using MIGA.Instances;
  using MIGA.Pipelines.InstallModules;
  using MIGA.Pipelines.Processors;
  using MIGA.Products;
  using MIGA.Tool.Base.Profiles;
  using MIGA.Tool.Base.Wizards;
  using JetBrains.Annotations;

  [UsedImplicitly]
  public class InstallModulesWizardArgs : WizardArgs
  {
    #region Fields

    public Instance Instance { get; }

    public readonly List<Product> _Modules = new List<Product>();

    private string _WebRootPath;

    #endregion

    #region Constructors

    public InstallModulesWizardArgs()
    { 
    }

    public InstallModulesWizardArgs(Instance instance)
    {
      Instance = instance;
      if (instance != null)
      {
        WebRootPath = instance.WebRootPath;
      }
    }

    #endregion

    #region Properties

    public Product ExtraPackage { get; set; }

    [UsedImplicitly]
    public string InstanceName
    {
      get
      {
        return Instance != null ? Instance.Name : string.Empty;
      }
    }

    [CanBeNull]
    public virtual Product Product
    {
      get
      {
        return Instance != null ? Instance.Product : null;
      }

      set
      {
        throw new NotImplementedException();
      }
    }

    #endregion

    #region Public Methods

    public override ProcessorArgs ToProcessorArgs()
    {
      var connectionString = ProfileManager.GetConnectionString();
      var products = _Modules;
      var product = ExtraPackage;
      if (product != null)
      {
        products.Add(product);
      }

      return new InstallModulesArgs(Instance, products, connectionString);
    }

    #endregion

    #region Public properties

    public string WebRootPath
    {
      get
      {
        return _WebRootPath ?? Instance.WebRootPath;
      }

      set
      {
        _WebRootPath = value;
      }
    }

    #endregion
  }
}