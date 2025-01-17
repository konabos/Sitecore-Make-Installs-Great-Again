﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MIGA.Sitecore9Installer.Tasks
{
  public class TaskDefinition
  {
    private JProperty _definition;
    public TaskDefinition(JProperty definition)
    {
      this._definition = definition;
      this.TaskFileName = this.ParseTaskFileName();
      this.TaskName = this._definition.Name;
    }

    public string TaskName { get; }
    public string TaskFileName { get; }

    public Task CreateTask(int order, GlobalParameters globalParams, string taskFolder, bool uninstall, PackageMapping mapping)
    {
      JToken overridden = this._definition.Value["Parameters"];
      string realName = this.TaskFileName;
      //Resolves local parameters
      LocalParameters localParams = GetTaskParameters(taskFolder, globalParams, uninstall, mapping);
      if (overridden != null)
        foreach (JProperty newJParam in overridden.Children())
        {
          InstallParam newParam = localParams[newJParam.Name];
          if (newParam != null) newParam.Value = newJParam.Value.ToString();
        }

      //Resolves task options for the task
      Dictionary<string, string> taskOptions = GetTaskOptions();
      //Creates a task based on type which is defined in the json
      string taskType = this._definition.Value["Type"]?.ToString() ?? typeof(SitecoreTask).FullName;
      //Each task should have the same ctor (TaskName(string),Order(int),Tasker,LocalParams(List<InstallParams>),TaskOptions(Dictionary<string,string>))
      return (Task)Activator.CreateInstance(Type.GetType(taskType), this._definition.Name, order, globalParams, localParams, taskOptions);
    }

    private string ParseTaskFileName()
    {
      JToken overridden = this._definition.Value["Parameters"];
      string realName = this._definition.Name;
      if (overridden != null && overridden["RealName"] != null)
      {
        realName = overridden["RealName"]?.ToString();
      }

      return realName;
    }

    private LocalParameters GetTaskParameters(string taskFolder, GlobalParameters globalParams, bool unInstall, PackageMapping mapping)
    {
      string file = Directory.GetFiles(taskFolder, string.Format("{0}.json", this.TaskFileName), SearchOption.AllDirectories)
        .FirstOrDefault();

      if (string.IsNullOrEmpty(file)) return new LocalParameters(new List<InstallParam>(), globalParams);

      List<InstallParam> installParams = new List<InstallParam>();
      JObject doc = JObject.Parse(File.ReadAllText(file));
      foreach (JProperty param in doc["Parameters"].Children())      {
        string dafultValue = param.Value["DefaultValue"]?.ToString();
        string typeName = param.Value["Type"]?.ToString();
        InstallParam p = new InstallParam(param.Name, dafultValue, false, typeName);
        p.Description = param.Value["Description"]?.ToString();
        if (!string.IsNullOrEmpty(globalParams[p.Name]?.Value))
          p.Value = globalParams[p.Name].Value;
        installParams.Add(p);

        if (p.Name == "Package")
        {          
            p.Value = mapping.Map(this.TaskName, taskFolder);
        }
      }

      installParams.Add(new InstallParam("Path", file,false,InstallParamType.String));
      return new LocalParameters(installParams, globalParams);
    }

    private Dictionary<string, string> GetTaskOptions()
    {
      Dictionary<string, string> options = null;
      if (this._definition.Value["TaskOptions"] != null)
        options = JsonConvert.DeserializeObject<Dictionary<string, string>>(this._definition.Value["TaskOptions"]
          .ToString());
      else
        options = new Dictionary<string, string>();

      return options;
    }

  }
}
