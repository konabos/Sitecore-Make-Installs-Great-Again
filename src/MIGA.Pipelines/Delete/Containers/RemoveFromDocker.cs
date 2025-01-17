﻿using System;
using JetBrains.Annotations;
using MIGA.Pipelines.Install.Containers;
using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines.Delete.Containers
{
  [UsedImplicitly]
  public class RemoveFromDocker : RunCommandBaseProcessor
  {
    protected override string GetDestination(ProcessorArgs arguments)
    {
      DeleteContainersArgs args = (DeleteContainersArgs)arguments;

      string destinationFolder = args?.DestinationFolder;

      if (string.IsNullOrEmpty(destinationFolder))
      {
        throw new InvalidOperationException($"destinationFolder is null or empty in {this.GetType().Name}");
      }

      return destinationFolder;
    }

    protected override string GetCommand(ProcessorArgs arguments)
    {
      return "docker-compose.exe down";
    }
  }
}