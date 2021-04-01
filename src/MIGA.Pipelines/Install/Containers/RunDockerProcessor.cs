using JetBrains.Annotations;
using MIGA.Loggers;
using MIGA.Pipelines.BaseProcessors;
using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines.Install.Containers
{
  [UsedImplicitly]
  public class RunDockerProcessor : RunCmdCommandBaseProcessor
  {
    protected override string GetCommand(ProcessorArgs procArgs)
    {
      return "docker-compose.exe up -d";
    }

    protected override string GetExecutionFolder(ProcessorArgs procArgs)
    {
      InstallContainerArgs args = (InstallContainerArgs)procArgs;

      return args.Destination;
    }

    protected override ILogger GetLogger(ProcessorArgs procArgs)
    {
      InstallContainerArgs args = (InstallContainerArgs)procArgs;

      return args.Logger;
    }
  }
}