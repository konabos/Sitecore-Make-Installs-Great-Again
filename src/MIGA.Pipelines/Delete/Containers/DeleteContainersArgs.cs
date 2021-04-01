using System;
using MIGA.ContainerInstaller;
using MIGA.Loggers;
using MIGA.Pipelines.Processors;
using Sitecore.Diagnostics.Base;

namespace MIGA.Pipelines.Delete.Containers
{
  public class DeleteContainersArgs : ProcessorArgs
  {
    public string DestinationFolder { get; }

    public EnvModel Env { get; }

    public Guid EnvironmentId { get; }

    public ILogger Logger { get; }

    public DeleteContainersArgs(string destinationFolder, EnvModel env, Guid environmentId, ILogger logger)
    {
      Assert.ArgumentNotNullOrEmpty(destinationFolder, "destinationFolder");
      Assert.ArgumentNotNull(env, "env");

      this.DestinationFolder = destinationFolder;
      this.Env = env;
      this.EnvironmentId = environmentId;
      this.Logger = logger;
    }
  }
}
