﻿using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace MIGA.ContainerInstaller.DockerCompose.Model
{
  public class DockerComposeModel
  {
    [YamlMember(ScalarStyle = ScalarStyle.DoubleQuoted)]
    public string Version { get; set; }
    public Dictionary<string, object> Services { get; set; }
  }
}
