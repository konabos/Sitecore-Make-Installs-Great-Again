using System;
using JetBrains.Annotations;
using MIGA.Pipelines.Processors;
using MIGA.SitecoreEnvironments;
using Sitecore.Diagnostics.Base;
using MIGA.Pipelines.Install.Containers;

namespace MIGA.Pipelines.Delete.Containers
{
  [UsedImplicitly]
  public class CleanupEnvironmentData : Processor
  {
    protected override void Process([NotNull] ProcessorArgs arguments)
    {
      Assert.ArgumentNotNull(arguments, "arguments");

      DeleteContainersArgs args = (DeleteContainersArgs)arguments;
      Assert.ArgumentNotNull(args, "args");

      Guid environmentId = args.EnvironmentId;

      SitecoreEnvironment environment;
      if (!SitecoreEnvironmentHelper.TryGetEnvironmentById(environmentId, out environment))
      {
        // TODO: log warn message if the env cannot be resolved from the environments.json
        return;
      }

      SitecoreEnvironmentHelper.SitecoreEnvironments.Remove(environment);
      SitecoreEnvironmentHelper.SaveSitecoreEnvironmentData(SitecoreEnvironmentHelper.SitecoreEnvironments);

      return;
    }
  }
}