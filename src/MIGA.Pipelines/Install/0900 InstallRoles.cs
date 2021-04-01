using System.Net;
using JetBrains.Annotations;
using MIGA.IO;
using MIGA.IO.Real;
using Sitecore.Diagnostics.Base;

namespace MIGA.Pipelines.Install
{
  using MIGA.Extensions;

  [UsedImplicitly]
  public class InstallRoles : InstallProcessor
  {
    [NotNull]
    private IFileSystem FileSystem { get; } = new RealFileSystem();

    #region Methods

    protected override void Process(InstallArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      var role = args.InstallRoles8.EmptyToNull() ?? args.InstallRoles9;
      if (string.IsNullOrEmpty(role))
      {
        return;
      }

      var websiteDir = FileSystem.ParseFolder(args.WebRootPath);
      var product = args.Product;
      var version = $"{product.TwoVersion}.{Safe.Call(() => $"{product.Update}") ?? "0"}";
      InstallRolesCommandHelper.Install(websiteDir, version, role);
    } 

    #endregion
  }
}