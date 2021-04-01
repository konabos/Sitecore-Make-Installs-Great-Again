using MIGA.Instances;
using MIGA.Products;

namespace MIGA.Pipelines.InstallModules
{
  using MIGA.Instances;
  using MIGA.Products;

  public interface IPackageInstallActions
  {
    #region Public methods

    void Execute(Instance instance, Product module);

    #endregion
  }
}