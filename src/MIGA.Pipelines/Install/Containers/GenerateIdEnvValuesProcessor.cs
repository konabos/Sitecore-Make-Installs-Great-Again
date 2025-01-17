﻿using JetBrains.Annotations;
using MIGA.ContainerInstaller;
using MIGA.Pipelines.Processors;
using Sitecore.Diagnostics.Base;

namespace MIGA.Pipelines.Install.Containers
{
  [UsedImplicitly]
  public class GenerateIdEnvValuesProcessor : Processor
  {
    private readonly IIdentityServerValuesGenerator _generator = new IdentityServerValuesGenerator();
    protected override void Process([NotNull] ProcessorArgs arguments)
    {
      Assert.ArgumentNotNull(arguments, "arguments");

      InstallContainerArgs args = (InstallContainerArgs)arguments;

      string idServerSecret;
      string idServerCert;
      string idServerCertPassword;

      this._generator.Generate(args.Destination,
        out idServerSecret,
        out idServerCert,
        out idServerCertPassword
        );

      args.EnvModel.IdServerSecret = idServerSecret;
      args.EnvModel.IdServerCert = idServerCert;
      args.EnvModel.IdServerCertPassword = idServerCertPassword;
    }
  }
}