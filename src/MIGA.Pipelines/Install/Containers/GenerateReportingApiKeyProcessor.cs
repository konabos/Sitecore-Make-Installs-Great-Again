using JetBrains.Annotations;
using MIGA.ContainerInstaller;
using MIGA.Pipelines.Processors;
using Sitecore.Diagnostics.Base;

namespace MIGA.Pipelines.Install.Containers
{
  [UsedImplicitly]
  public class GenerateReportingApiKeyProcessor : Processor
  {
    private const string Key = "REPORTING_API_KEY";

    private readonly IGenerator _generator = new ReportingApiKeyGenerator();
    protected override void Process([NotNull] ProcessorArgs arguments)
    {
      Assert.ArgumentNotNull(arguments, "arguments");

      InstallContainerArgs args = (InstallContainerArgs)arguments;

      if (args.EnvModel.KeyExists(Key))
      {
        args.EnvModel[Key] = this._generator.Generate();
      }
    }
  }
}