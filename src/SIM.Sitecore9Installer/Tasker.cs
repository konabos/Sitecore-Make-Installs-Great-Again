﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SIM.Sitecore9Installer
{
  public class Tasker
  {

    List<SitecoreTask> tasksToRun = new List<SitecoreTask>();
    List<InstallParam> mapping;
    string filesRoot;
    string globalParamsFile;
    JObject doc;
    List<InstallParam> globalParams;

    public static FileInfo ResolveGlobalFile(Product product)
    {
      string packageName = Path.GetFileNameWithoutExtension(product.PackagePath);
      string jsonGlobalFilesMap = string.Empty;
      using(var reader= new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "GlobalFilesMap.json")))
      {
        jsonGlobalFilesMap = reader.ReadToEnd();
      }

      Dictionary<string,string> globalFilesMap= JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonGlobalFilesMap);
      foreach(string pattern in globalFilesMap.Keys)
      {
        if (Regex.IsMatch(packageName, pattern))
        {
          return new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), globalFilesMap[pattern]));
        }
      }

      return null;
    }    

    public List<InstallParam> GlobalParams
    {
      get
      {
        return this.globalParams;
      }
    }

    public List<SitecoreTask> Tasks
    {
      get
      {
        return this.tasksToRun;
      }
    }


    public Tasker(string root, string globalFile)
    {
      this.filesRoot = root;
      this.globalParamsFile = globalFile;
      this.mapping = this.GetPackageMapping();
      this.doc = JObject.Parse(File.ReadAllText(this.globalParamsFile));
      this.globalParams = this.GetGlobalParams();
      this.LoadTasks();
    }

    private List<InstallParam> GetGlobalParams()
    {
      List<InstallParam> list = new List<InstallParam>();
      list.Add(new InstallParam("FilesRoot", this.filesRoot));
      foreach (JProperty param in doc["Parameters"].Children())
      {
        InstallParam p = new InstallParam(param.Name, param.Value.ToString());
        if (p.Name == "LicenseFile")
        {
          string license = Path.Combine(this.filesRoot, "license.xml");
          if (File.Exists(license))
          {
            p.Value = license;
          }
        }

        p.ParamValueUpdated += GlobalParamValueUpdated;
        list.Add(p);
      }

      return list;
    }

    private void GlobalParamValueUpdated(object sender, ParamValueUpdatedArgs e)
    {
      InstallParam updatedParam = (InstallParam)sender;
      foreach (SitecoreTask task in this.Tasks)
      {
        InstallParam param = task.LocalParams.FirstOrDefault(p => p.Name == updatedParam.Name);
        if (param != null)
        {
          param.Value = updatedParam.Value;
        }
      }
    }

    public string RunAllTasks()
    {
      StringBuilder results = new StringBuilder();

      foreach (SitecoreTask task in this.tasksToRun.Where(t=>t.ShouldRun))
      {
        try
        {
          results.AppendLine(task.Run());
          if (task.State == TaskState.Failed)
          {
            break;
          }
        }
        catch (Exception ex)
        {          
          results.AppendLine(ex.ToString());
          break;
        }
      }

      return results.ToString().Trim();
    }

    public void GenerateScripts()
    {
      string path = Path.Combine(this.filesRoot, "generated_scripts");
      Directory.CreateDirectory(path);
      foreach (SitecoreTask task in this.Tasks.Where(t=>t.ShouldRun))
      {
        using (StreamWriter writer = new StreamWriter(Path.Combine(path, string.Format("{0}.ps1", task.Name))))
        {
          writer.Write(task.GetScript());
        }
      }
    }

    private List<InstallParam> GetTaskParameters(string name)
    {
      string file = Directory.GetFiles(this.filesRoot, string.Format("{0}.json", name),SearchOption.AllDirectories).First();

      List<InstallParam> installParams = new List<InstallParam>();
      JObject doc = JObject.Parse(File.ReadAllText(file));
      foreach (JProperty param in doc["Parameters"].Children())
      {
        string dafultValue = param.Value["DefaultValue"]?.ToString();

        InstallParam p = new InstallParam(param.Name, dafultValue);
        p.Description = param.Value["Description"]?.ToString();
        if (this.globalParams.Any(g => g.Name == p.Name && !string.IsNullOrEmpty(g.Value)))
        {
          p.Value = this.globalParams.First(g => g.Name == p.Name).Value;
        }
        installParams.Add(p);

        if (p.Name == "Package")
        {
          InstallParam pack = mapping.FirstOrDefault(g => g.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
          if (!string.IsNullOrEmpty(pack?.Value))
          {
            p.Value = Directory.GetFiles(this.filesRoot, pack.Value).FirstOrDefault();
          }
        }
      }
      installParams.Add(new InstallParam("Path", file));
      return installParams;
    }

    private void LoadTasks()
    {
      foreach (JProperty param in doc["ExecSequense"].Children())
      {
        var overridden = param.Value["Parameters"];
        string realName = param.Name;
        if (overridden != null && overridden["RealName"] != null)
        {
          realName = overridden["RealName"]?.ToString();
        }

        string taskFile= Directory.GetFiles(this.filesRoot, string.Format("{0}.json", realName), SearchOption.AllDirectories).FirstOrDefault();
        if (string.IsNullOrEmpty(taskFile))
        {
          continue;
        }

        //if (!File.Exists(string.Format("{0}.json", Path.Combine(filesRoot, realName))))
        //{
        //  continue;
        //}

        SitecoreTask t = new SitecoreTask(param.Name);
        t.GlobalParams = this.GlobalParams;
        t.LocalParams = this.GetTaskParameters(realName);

        if (overridden != null)
        {
          foreach (JProperty newJParam in overridden.Children())
          {
            InstallParam newParam = t.LocalParams.FirstOrDefault(p => p.Name == newJParam.Name);
            if (newParam != null)
            {
              newParam.Value = newJParam.Value.ToString();
            }
          }
        }

        this.tasksToRun.Add(t);
      }
    }


    private List<InstallParam> GetPackageMapping()
    {
      List<InstallParam> list = new List<InstallParam>();

      string file = this.globalParamsFile;
      JObject doc = JObject.Parse(File.ReadAllText(file));
      foreach (JProperty param in doc["PackageMapping"].Children())
      {
        InstallParam p = new InstallParam(param.Name, param.Value.ToString());
        list.Add(p);
      }

      return list;
    }

  }
}