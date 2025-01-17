﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using MIGA.Sitecore9Installer;
using MIGA.Tool.Base.Pipelines;
using MIGA.Tool.Base.Profiles;

namespace MIGA.Tool.Windows.Pipelines.Solr
{
  public static class AddSolrToSimConfig
  {
    public static void Run(Install9WizardArgs args)
    {
      BaseParameters solrParams = args.Tasker.Tasks.First(t => t.Name == "Solr").LocalParams;
      string solrDomain = solrParams.First(p => p.Name == "SolrDomain").Value;
      string solrInstallRoot = solrParams.First(p => p.Name == "SolrInstallRoot").Value;
      string solrPort = solrParams.First(p => p.Name == "SolrPort").Value;
      string solrVersion = solrParams.First(p => p.Name == "SolrVersion").Value;
      string solrService = $"solr-{solrVersion}-{solrDomain}-{solrPort}";
      SolrDefinition solr = new SolrDefinition()
      {
        Name = solrService,
        Root = Path.Combine(solrInstallRoot, solrService),
        Service = solrService,
        Url = "https://" + solrDomain + ":" + solrPort+"/solr"
      };
      ProfileManager.Profile.Solrs.Add(solr);
      ProfileManager.SaveChanges(ProfileManager.Profile);
    }
  }
}
