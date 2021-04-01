using JetBrains.Annotations;
using MIGA.ContainerInstaller;
using MIGA.Pipelines.Processors;
using Sitecore.Diagnostics.Base;

namespace MIGA.Pipelines.Install.Containers
{
  [UsedImplicitly]
  public class GenerateSqlAdminPasswordProcessor : Processor
  {
    private readonly IGenerator _generator = new SqlAdminPasswordGenerator();
    protected override void Process([NotNull] ProcessorArgs arguments)
    {
      Assert.ArgumentNotNull(arguments, "arguments");

      InstallContainerArgs args = (InstallContainerArgs)arguments;

      if (string.IsNullOrEmpty(args.EnvModel.SqlAdminPassword))
      {
        args.EnvModel.SqlAdminPassword = this._generator.Generate();
      }
    }
  }
}
