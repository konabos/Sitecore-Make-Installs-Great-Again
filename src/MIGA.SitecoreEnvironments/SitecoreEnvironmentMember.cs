﻿using Newtonsoft.Json;

namespace MIGA.SitecoreEnvironments
{
  public class SitecoreEnvironmentMember
  {
    public SitecoreEnvironmentMember(string name, string type):this(name,type,false)
    {
    }

    [JsonConstructor]
    public SitecoreEnvironmentMember(string name, string type, bool isContainer)
    {
      this.Name = name;
      this.Type = type;
      this.IsContainer = isContainer;
    }

    public string Name { get; set; }

    public string Type { get; set; }

    public bool IsContainer { get; }

    public enum Types
    {
      Site,
      Service      
    }
  }
}
