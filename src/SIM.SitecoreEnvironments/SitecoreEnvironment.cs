﻿namespace SIM.SitecoreEnvironments
{
  using System;
  using System.Collections.Generic;

  public class SitecoreEnvironment
  {
    private EnvironmentType _type;
    public SitecoreEnvironment()
    {
      this._type = EnvironmentType.OnPrem;
    }

    public SitecoreEnvironment(string instanceName) : this(instanceName, EnvironmentType.OnPrem) { }
    public SitecoreEnvironment(string instanceName, EnvironmentType type)
    {
      ID = Guid.NewGuid();
      Name = instanceName;
      this._type = type;
    }

    public Guid ID { get; set; }

    public string Name { get; set; }

    public List<SitecoreEnvironmentMember> Members { get; set; }

    public string UnInstallDataPath { get; set; }

    public EnvironmentType EnvType { get; set; }
      
    public enum EnvironmentType { OnPrem, Container}
  }
}
