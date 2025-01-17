﻿using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines.Install.Containers
{
  public class WriteEnvFileProcessor : Processor
  {
    protected override void Process([NotNull] ProcessorArgs arguments)
    {
      InstallContainerArgs args = (InstallContainerArgs)arguments;
      args.EnvModel.SaveToFile(Path.Combine(args.Destination,".env"));
    }
  }
}
