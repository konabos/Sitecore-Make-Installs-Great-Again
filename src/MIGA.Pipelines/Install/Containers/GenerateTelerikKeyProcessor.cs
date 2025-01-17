﻿using JetBrains.Annotations;
using MIGA.ContainerInstaller;
using MIGA.Pipelines.Processors;
using Sitecore.Diagnostics.Base;

namespace MIGA.Pipelines.Install.Containers
{
  [UsedImplicitly]
  public class GenerateTelerikKeyProcessor : Processor
  {
    private readonly IGenerator _generator = new TelerikKeyGenerator();
    protected override void Process([NotNull] ProcessorArgs arguments)
    {
      Assert.ArgumentNotNull(arguments, "arguments");

      InstallContainerArgs args = (InstallContainerArgs)arguments;

      args.EnvModel.TelerikKey = this._generator.Generate();
    }
  }
}