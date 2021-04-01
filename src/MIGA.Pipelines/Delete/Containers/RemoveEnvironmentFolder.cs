using JetBrains.Annotations;
using MIGA.Pipelines.Processors;
using MIGA.Pipelines.Install.Containers;
using Sitecore.Diagnostics.Base;

namespace MIGA.Pipelines.Delete.Containers
{
  [UsedImplicitly]
  public class RemoveEnvironmentFolder : Processor
  {
    protected override void Process([NotNull] ProcessorArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      DeleteContainersArgs deleteArgs = (DeleteContainersArgs)args;
      Assert.ArgumentNotNull(deleteArgs, "deleteArgs");

      FileSystem.FileSystem.Local.Directory.DeleteIfExists(deleteArgs.DestinationFolder);
    }
  }
}
