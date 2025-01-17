﻿using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MIGA.Adapters.WebServer;
using MIGA.Pipelines.Processors;
using MIGA.Pipelines.Install.Containers;
using Sitecore.Diagnostics.Base;

namespace MIGA.Pipelines.Delete.Containers
{
  [UsedImplicitly]
  public class RemoveHostsProcessor : Processor
  {
    protected void RemoveHosts(IEnumerable<string> hosts)
    {
      Hosts.Remove(hosts);
    }

    protected override void Process([NotNull] ProcessorArgs arguments)
    {
      Assert.ArgumentNotNull(arguments, nameof(arguments));

      DeleteContainersArgs args = (DeleteContainersArgs)arguments;
      Assert.ArgumentNotNull(args, nameof(args));

      IEnumerable<string> hosts = GetHostNames(args);

      RemoveHosts(hosts);
    }

    protected virtual IEnumerable<string> GetHostNames(DeleteContainersArgs args)
    {
      IEnumerable<string> hosts = args.Env.Where(nvp => nvp.Name.EndsWith("_HOST")).Select(nvp => nvp.Value);

      return hosts;
    }
  }
}