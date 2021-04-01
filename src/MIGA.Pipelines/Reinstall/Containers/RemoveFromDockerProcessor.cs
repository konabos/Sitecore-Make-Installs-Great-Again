using JetBrains.Annotations;
using MIGA.Loggers;
using MIGA.Pipelines.BaseProcessors;
using MIGA.Pipelines.Delete.Containers;
using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines.Reinstall.Containers
{
  [UsedImplicitly]
  public class RemoveFromDockerProcessor : RunCmdCommandBaseProcessor
  {
    protected override string GetCommand(ProcessorArgs procArgs)
    {
      return "docker-compose.exe down";
    }

    protected override string GetExecutionFolder(ProcessorArgs procArgs)
    {
      DeleteContainersArgs args = (DeleteContainersArgs)procArgs;

      return args.DestinationFolder;
    }

    protected override ILogger GetLogger(ProcessorArgs procArgs)
    {
      DeleteContainersArgs args = (DeleteContainersArgs)procArgs;

      return args.Logger;
    }
  }
}