﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGA.Sitecore9Installer
{
  public class GlobalParameters:BaseParameters
  {
    private List<InstallParam> _parameters;

    public GlobalParameters(JObject globalParamsDoc, string filesRoot)
    {
      List<InstallParam> globalParams = new List<InstallParam>();
      globalParams.Add(new InstallParam("FilesRoot", filesRoot,true,InstallParamType.String));
      foreach (JProperty param in globalParamsDoc["Parameters"].Children())
      {
        InstallParam p = new InstallParam(param.Name, param.Value.ToString(), true,InstallParamType.String);
        if (p.Name == "LicenseFile" && !string.IsNullOrWhiteSpace(filesRoot))
        {
          string license = Path.Combine(filesRoot, "license.xml");
          if (File.Exists(license)) p.Value = license;
        }

        globalParams.Add(p);
      }

      this._parameters = globalParams;
    }

    protected internal GlobalParameters()
    {
      this._parameters = new List<InstallParam>();
    }

    protected override List<InstallParam> Parameters { get => _parameters; }

    protected override void CalculateParameters()
    {
        StringBuilder globalParamsEval = new StringBuilder();
      globalParamsEval.Append("Set-ExecutionPolicy Bypass -Force\n");
      globalParamsEval.AppendFormat("Import-Module SitecoreInstallFramework{0}\n", this.GetSifVersion());
      globalParamsEval.AppendLine("$GlobalParams =@{");
      string paramsScript = GetParamsScript(false);
      globalParamsEval.Append(paramsScript);
      globalParamsEval.Append("}\n");
      globalParamsEval.AppendLine("$GlobalParamsSys =@{");
      globalParamsEval.Append(paramsScript);
      globalParamsEval.Append("}\n$GlobalParamsSys");
      Hashtable evaluatedParams = this.GetEvaluatedParams(globalParamsEval.ToString());

      foreach (var param in this.Parameters)
      {
        if (evaluatedParams[param.Name] == null || param.Value == evaluatedParams[param.Name].ToString()) continue;

        param.Value = (string)evaluatedParams[param.Name];
      }
    }

    public string GetGlobalParamsScript()
    {
      return this.GetParamsScript();
    }

    protected override InstallParam CreateParameter(string name, string value, InstallParamType type)
    {
      return new InstallParam(name, value, true, type);
    }

    private string GetSifVersion()
    {
      string sifVersion = this["SIFVersion"]?.Value ?? string.Empty;
      if (!string.IsNullOrEmpty(sifVersion))
      {
        return string.Format(" -RequiredVersion {0}", sifVersion);
      }
      return string.Empty;
    }
  }
}
