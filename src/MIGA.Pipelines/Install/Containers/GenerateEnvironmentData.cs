﻿using System;
using System.IO;
using JetBrains.Annotations;
using Sitecore.Diagnostics.Base;
using System.Collections.Generic;
using System.Linq;
using MIGA.ContainerInstaller.DockerCompose;
using MIGA.Pipelines.Processors;
using MIGA.SitecoreEnvironments;
using MongoDB.Bson.Serialization.Conventions;
using MIGA.ContainerInstaller.DockerCompose.Model;

namespace MIGA.Pipelines.Install.Containers
{
  [UsedImplicitly]
  public class GenerateEnvironmentData : Processor
  {
    private IList<string> siteTypes => new List<string>()
    {
      "id",
      "cm",
      "cd",
      "xconnect",
      "prc",
      "rep",
      "xdbcollection",
      "xdbsearch"
    };

    private IList<string> serviceTypes => new List<string>()
    {
      "xdbsearchworker",
      "xdbautomationworker",
      "cortexprocessingworker",
      "xdbautomation",
      "xdbautomationrpt",
      "cortexprocessing",
      "cortexreporting",
      "xdbrefdata",
      "xdbsearchworker",
      "xdbautomationworker",
      "cortexprocessingworker"
    };

    protected override void Process([NotNull] ProcessorArgs arguments)
    {
      Assert.ArgumentNotNull(arguments, "ProcessorArgs:arguments");

      InstallContainerArgs args = (InstallContainerArgs)arguments;

      string destinationFolder = args.Destination;

      Assert.ArgumentNotNullOrEmpty(destinationFolder, "destinationFolder");

      if (!Directory.Exists(destinationFolder))
      {
        throw new InvalidOperationException("'args.Destination' points to non-existing Folder.");
      }

      string environmentName = args.EnvModel.ProjectName;
      Assert.ArgumentNotNullOrEmpty(environmentName, "projectName");

      this.AddEnvironment(this.CreateEnvironment(environmentName, destinationFolder));
    }

    private SitecoreEnvironment CreateEnvironment(string environmentName, string destinationFolder)
    {
      SitecoreEnvironment environment = new SitecoreEnvironment(environmentName, SitecoreEnvironment.EnvironmentType.Container);
      environment.UnInstallDataPath = destinationFolder;
      environment.Members = GetEnvironmentMembers(environmentName, destinationFolder).ToList();

      return environment;
    }

    private IEnumerable<SitecoreEnvironmentMember> GetEnvironmentMembers(
      string environmentName, 
      string destinationFolder, 
      string fileName = "docker-compose.yml"
      )
    {
      string pathToComposeFile = Path.Combine(destinationFolder, fileName);

      IRepository<object> repository = new DockerComposeRepository(pathToComposeFile);

      IDictionary<string, object> services = repository.GetServices();

      foreach (var serviceName in services.Keys)
      {
        string memberType = GetMemberType(serviceName);

        if (!string.IsNullOrEmpty(memberType))
        {
          string memberName = $"{environmentName}-{serviceName}";

          yield return new SitecoreEnvironmentMember(memberName, memberType, isContainer: true);
        }
      }
    }

    private string GetMemberType(string serviceName)
    {
      if (siteTypes.Contains(serviceName))
      {
        return SitecoreEnvironmentMember.Types.Site.ToString();
      }

      if (serviceTypes.Contains(serviceName))
      {
        return SitecoreEnvironmentMember.Types.Service.ToString();
      }

      return null;
    }

    private void AddEnvironment(SitecoreEnvironment environment)
    {
      foreach (SitecoreEnvironment se in SitecoreEnvironmentHelper.SitecoreEnvironments)
      {
        if (se.Name == environment.Name)
        {
          return;
        }
      }

      SitecoreEnvironmentHelper.SitecoreEnvironments.Add(environment);
      SitecoreEnvironmentHelper.SaveSitecoreEnvironmentData(SitecoreEnvironmentHelper.SitecoreEnvironments);
    }
  }
}
