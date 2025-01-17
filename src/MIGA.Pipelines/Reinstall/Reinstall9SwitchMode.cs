﻿using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines.Reinstall
{
  public class Reinstall9SwitchMode : Processor
  {
    protected override void Process([NotNull] ProcessorArgs args)
    {
      Reinstall9Args arguments = (Reinstall9Args)args;
      arguments.Tasker.UnInstall = false;
    }
  }
}
