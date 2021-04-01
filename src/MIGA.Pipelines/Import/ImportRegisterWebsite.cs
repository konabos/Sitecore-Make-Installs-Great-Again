﻿using MIGA.Adapters.WebServer;
using MIGA.Instances;

namespace MIGA.Pipelines.Import
{
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using System.Xml;
  using MIGA.Adapters.WebServer;
  using MIGA.Instances;
  using JetBrains.Annotations;
  using MIGA.Extensions;

  [UsedImplicitly]
  internal class ImportRegisterWebsite : ImportProcessor
  {
    #region Public methods

    public string CreateNewAppPoolName(string oldName)
    {
      List<string> poolsNames = new List<string>();
      foreach (var appPool in WebServerManager.CreateContext().ApplicationPools)
      {
        poolsNames.Add(appPool.Name);
      }

      bool flag = false;
      while (!flag)
      {
        if ((from t in poolsNames
          where t == oldName
          select t).FirstOrDefault() == null)
        {
          return oldName;
        }
        else
        {
          oldName += "_imported";
        }
      }

      return null;
    }

    public long? CreateNewID(long? oldID)
    {
      using (WebServerManager.WebServerContext context = WebServerManager.CreateContext())
      {
        var instances = context.Sites;
        return oldID == null || instances.Any(x => x.Id == oldID) ? instances.Max(x => x.Id) + 1 : oldID;
      }
    }

    public string CreateNewName(string[] names, string oldName)
    {
      bool flag = false;
      while (!flag)
      {
        if ((from t in names
          where t == oldName
          select t).FirstOrDefault() == null)
        {
          return oldName;
        }
        else
        {
          oldName += "_imported";
        }
      }

      return null;
    }

    public string CreateNewSiteName(IEnumerable<Instance> instances, string oldName)
    {
      List<string> sitesNames = new List<string>();
      foreach (var instance in instances)
      {
        sitesNames.Add(instance.Name);
      }

      bool flag = false;
      while (!flag)
      {
        if ((from t in sitesNames
          where t == oldName
          select t).FirstOrDefault() == null)
        {
          return oldName;
        }
        else
        {
          oldName += "_imported";
        }
      }

      return null;
    }

    #endregion

    #region Protected methods

    protected override void Process(ImportArgs args)
    {
      // var websiteName = args.Instance.Name;
      // var appPoolName = WebServerManager.CreateContext(string.Empty).Sites[websiteName].ApplicationDefaults.ApplicationPoolName;        
      ChangeAppPoolSettingsIfNeeded(args._TemporaryPathToUnpack.PathCombine(ImportArgs.AppPoolSettingsFileName), args);
      ChangeWebsiteSettingsIfNeeded(args._TemporaryPathToUnpack.PathCombine(ImportArgs.WebsiteSettingsFileName), args);
      var importAppPoolSettingsCommand = $@"%windir%\system32\inetsrv\appcmd add apppool /in < {args._TemporaryPathToUnpack.PathCombine(ImportArgs.AppPoolSettingsFileName) + ".fixed.xml"}";
      var importWebsiteSettingsCommand = $@"%windir%\system32\inetsrv\appcmd add site /in < {args._TemporaryPathToUnpack.PathCombine(ImportArgs.WebsiteSettingsFileName) + ".fixed.xml"}";

      ExecuteCommand(importAppPoolSettingsCommand);
      ExecuteCommand(importWebsiteSettingsCommand);
    }

    #endregion

    #region Private methods

    private static void ExecuteCommand(string command)
    {
      var procStartInfo = new ProcessStartInfo("cmd", $"/c {command}")
      {
        UseShellExecute = false, 
        CreateNoWindow = true
      };

      var proc = new Process
      {
        StartInfo = procStartInfo
      };
      proc.Start();
      proc.WaitForExit();
    }

    private void ChangeAppPoolSettingsIfNeeded(string path, ImportArgs args)
    {
      // should be executed before ChangeWebsiteSettingsIfNeeded
      // need to change AppName
      XmlDocumentEx appPoolSettings = new XmlDocumentEx();
      appPoolSettings.Load(path);
      args._AppPoolName = CreateNewAppPoolName(args._AppPoolName);
      appPoolSettings.SetElementAttributeValue("/appcmd/APPPOOL", "APPPOOL.NAME", args._AppPoolName);
      appPoolSettings.SetElementAttributeValue("appcmd/APPPOOL/add", "name", args._AppPoolName);

      appPoolSettings.Save(appPoolSettings.FilePath + ".fixed.xml");
    }

    private void ChangeRootFolderIfNeeded(string path)
    {
    }

    private void ChangeWebsiteSettingsIfNeeded(string path, ImportArgs args)
    {
      XmlDocumentEx websiteSettings = new XmlDocumentEx();
      websiteSettings.Load(path);
      args._SiteName = CreateNewSiteName(InstanceManager.Default.Instances, args._SiteName);
      websiteSettings.SetElementAttributeValue("/appcmd/SITE", "SITE.NAME", args._SiteName);
      websiteSettings.SetElementAttributeValue("/appcmd/SITE/site", "name", args._SiteName);

      websiteSettings.SetElementAttributeValue("/appcmd/SITE", "bindings", $"http/*:80:{args._SiteName}");

      // need to change site ID
      args._SiteID = CreateNewID(args._SiteID);
      websiteSettings.SetElementAttributeValue("/appcmd/SITE", "SITE.ID", args._SiteID.ToString());
      websiteSettings.SetElementAttributeValue("/appcmd/SITE/site", "id", args._SiteID.ToString());

      // change apppool name
      websiteSettings.SetElementAttributeValue("/appcmd/SITE/site/application", "applicationPool", args._AppPoolName);
      websiteSettings.SetElementAttributeValue("/appcmd/SITE/site/applicationDefaults", "applicationPool", args._AppPoolName);

      // change root folder
      websiteSettings.SetElementAttributeValue("/appcmd/SITE/site/application/virtualDirectory", "physicalPath", $"{args._RootPath}\\Website");

      // TODO: need to change bindings in right way(maybe with the UI dialog)
      // websiteSettings.SetElementAttributeValue("/appcmd/SITE/site/bindings/binding[@bindingInformation='*:80:" + args.oldSiteName + "']", "bindingInformation", "*:80:" + args.siteName);
      XmlElement bindingsElement = websiteSettings.SelectSingleElement("/appcmd/SITE/site/bindings");
      if (bindingsElement != null)
      {
        bindingsElement.InnerXml = string.Empty;

        // it's a fucking HACK, I can't work with xml nodes
        foreach (var key in args._Bindings.Keys)
        {
          bindingsElement.InnerXml += "<binding protocol=\"http\" bindingInformation=\"*:{1}:{0}\" />".FormatWith(key, args._Bindings[key].ToString());
        }

        // foreach (XmlElement bindingElement in bindingsElement.ChildNodes)
        // {

        // //if (bindingElement.Attributes["bindingInformation"].Value.Split(':').Last() != null && bindingElement.Attributes["bindingInformation"].Value.Split(':').Last() != "")
        // //    args.siteBindingsHostnames.Add(bindingElement.Attributes["bindingInformation"].Value.Split(':').Last());
        // //if(bindingElement.Attributes["bindingInformation"].Value.Split(':').Last() != null && bindingElement.Attributes["bindingInformation"].Value.Split(':').Last() != "")
        // //bindingElement.Attributes["bindingInformation"].Value = bindingElement.Attributes["bindingInformation"].Value.Replace(bindingElement.Attributes["bindingInformation"].Value.Split(':').Last(), args.siteName);
        // }
      }

      websiteSettings.Save(websiteSettings.FilePath + ".fixed.xml");
    }

    #endregion


  }
}