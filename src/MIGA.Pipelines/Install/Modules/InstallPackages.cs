using MIGA.Pipelines.Agent;
using MIGA.Products;

namespace MIGA.Pipelines.Install.Modules
{
  using System.Collections.Generic;
  using System.Linq;
  using MIGA.Pipelines.Agent;
  using MIGA.Products;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  #region

  #endregion

  [UsedImplicitly]
  public class InstallPackages : InstallProcessor
  {
    #region Fields

    private readonly List<Product> _Done = new List<Product>();

    #endregion

    #region Methods

    protected override void Process([NotNull] InstallArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      Assert.IsNotNull(args.Instance, "Instance");

      foreach (var module in args._Modules.Where(m => m.IsPackage))
      {
        if (_Done.Contains(module))
        {
          continue;
        }

        AgentHelper.InstallPackage(args.Instance, module);

        _Done.Add(module);
      }
    }

    #endregion
  }
}