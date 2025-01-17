﻿using MIGA.Adapters.WebServer;
using MIGA.SitecoreEnvironments;

namespace MIGA.Instances
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Microsoft.Web.Administration;
  using MIGA.Adapters.WebServer;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;
  using Sitecore.Diagnostics.Base.Extensions.EnumerableExtensions;
  using Sitecore.Diagnostics.Logging;
  using MIGA.SitecoreEnvironments;

  #region

  #endregion

  public class InstanceManager
  {
    #region Fields

    private IDictionary<string, Instance> _CachedInstances;
    private IEnumerable<Instance> _Instances;
    private string _InstancesFolder;

    #endregion

    #region Properties

    #region Delegates

    public event EventHandler InstancesListUpdated;

    #endregion

    #region Public properties

    [CanBeNull]
    public IEnumerable<Instance> Instances
    {
      get
      {
        return _Instances;
      }

      private set
      {
        _Instances = value;
        OnInstancesListUpdated();
      }
    }

    [CanBeNull]
    public IDictionary<string, Instance> PartiallyCachedInstances
    {
      get
      {
        // I believe that this check is redundant because the this list is filled before Instances list is actually filled.
        if (_CachedInstances != null)
        {
          return _CachedInstances;
        }

        if (Instances == null)
        {
          return null;
        }

        _CachedInstances = new Dictionary<string, Instance>();
        Instances.ForEach(x => { _CachedInstances.Add(x.Name, new PartiallyCachedInstance((int)x.ID)); });

        return _CachedInstances;
      }

      private set
      {
        if (_CachedInstances != null)
        {
          foreach (var cachedInstance in _CachedInstances.Values.OfType<IDisposable>())
          {
            if (cachedInstance != null)
            {
              cachedInstance.Dispose();
            }
          }
        }

        _CachedInstances = value;
      }
    }

    public static InstanceManager Default { get; } = new InstanceManager();

    #endregion

    #endregion

    #region Public Methods

    #region Public methods

    [CanBeNull]
    public Instance GetInstance([NotNull] string name)
    {
      Assert.ArgumentNotNull(name, nameof(name));
      Log.Debug($"InstanceManager:GetInstance('{name}')");

      if (Instances == null)
      {
        Initialize();
      }

      if (Instances == null)
      {
        return null;
      }

      return PartiallyCachedInstances?[name];
    }

    public void Initialize([CanBeNull] string defaultRootFolder = null)
    {
      SitecoreEnvironmentHelper.RefreshEnvironments();

      List<Instance> instances = new List<Instance>();

      if(ApplicationManager.IsIisRunning) {instances.AddRange(GetIISInstances());}

      if(ApplicationManager.IsDockerRunning) {instances.AddRange(GetContainerizedInstances());}

      Dictionary<string, Instance> partiallyCachedInstances = instances.ToDictionary(i => i.Name);

      PartiallyCachedInstances = partiallyCachedInstances;

      Instances = instances;
    }

    private IEnumerable<Instance> GetContainerizedInstances()
    {
      List<SitecoreEnvironment> environments =
        SitecoreEnvironmentHelper.SitecoreEnvironments.
          Where(e => e.EnvType == SitecoreEnvironment.EnvironmentType.Container).ToList();

      List<Instance> instances = environments.SelectMany(e => e.Members).Select(m => MemberToInstance(m)).ToList();

      return instances;
    }

    private Instance MemberToInstance(SitecoreEnvironmentMember member)
    {
      Instance instance = new ContainerizedInstance(member.Name);

      return instance;
    }

    private IEnumerable<Instance> GetIISInstances([CanBeNull] string defaultRootFolder = null)
    {
      using (WebServerManager.WebServerContext context = WebServerManager.CreateContext())
      {
        ProfileSection.Argument("defaultRootFolder", defaultRootFolder);

        IEnumerable<Site> sites = GetOperableSites(context, defaultRootFolder);

        return GetPartiallyCachedInstances(sites);
      }
    }
    
    public void InitializeWithSoftListRefresh([CanBeNull] string defaultRootFolder = null)
    {
      SitecoreEnvironmentHelper.RefreshEnvironments();

      using (new ProfileSection("Initialize with soft list refresh"))
      {
        // Add check that this isn't an initial initialization
        if (Instances == null)
        {
          Initialize(defaultRootFolder);
        }

        using (WebServerManager.WebServerContext context = WebServerManager.CreateContext())
        {
          IEnumerable<Site> sites = GetOperableSites(context, defaultRootFolder);

          // The trick is in reused PartiallyCachedInstances. We use site ID as identificator that cached instance may be reused. If we can't fetch instance from cache, we create new.
          PartiallyCachedInstances = sites.Select(site =>
              PartiallyCachedInstances?.Values.FirstOrDefault(cachedInst => cachedInst.ID == site.Id) ??
              GetPartiallyCachedInstance(site))
            .Where(IsSitecoreOrSitecoreEnvironmentMember)
            .Where(IsNotHidden).ToDictionary(value => value.Name);

          Instances = PartiallyCachedInstances?.Values.Select(x => GetInstance(x.ID)).ToArray();
        }
      }
    }

    #endregion

    #region Private methods

    private IEnumerable<Instance> GetInstances()
    {
      using (new ProfileSection("Get instances"))
      {
        var array = PartiallyCachedInstances?.Values.Select(x => GetInstance(x.ID)).ToArray();

        return ProfileSection.Result(array);
      }
    }

    private IEnumerable<Instance> GetPartiallyCachedInstances(IEnumerable<Site> sites)
    {
      using (new ProfileSection("Getting partially cached Sitecore instances"))
      {
        ProfileSection.Argument("sites", sites);

        IEnumerable<Instance> partiallyCachedInstances = sites.Select(GetPartiallyCachedInstance)
          .Where(IsSitecoreOrSitecoreEnvironmentMember)
          .Where(IsNotHidden);

        return partiallyCachedInstances;
      }
    }

    #endregion

    #endregion

    #region Methods

    private IEnumerable<Site> GetOperableSites([NotNull] WebServerManager.WebServerContext context, [CanBeNull] string defaultRootFolder = null)
    {
      Assert.IsNotNull(context, "Context cannot be null");

      using (new ProfileSection("Getting operable sites"))
      {
        ProfileSection.Argument("context", context);
        ProfileSection.Argument("defaultRootFolder", defaultRootFolder);

        IEnumerable<Site> sites = context.Sites;
        if (defaultRootFolder != null)
        {
          _InstancesFolder = defaultRootFolder.ToLower();
          sites = sites.Where(s => WebServerManager.GetWebRootPath(s).ToLower().Contains(_InstancesFolder));
        }

        return ProfileSection.Result(sites);
      }
    }

    [NotNull]
    private Instance GetPartiallyCachedInstance([NotNull] Site site)
    {
      Assert.ArgumentNotNull(site, nameof(site));
      var id = (Int32)site.Id;
      Log.Debug($"InstanceManager:GetPartiallyCachedInstance(Site: {site.Id})");
      return new PartiallyCachedInstance(id);
    }

    private bool IsSitecoreOrSitecoreEnvironmentMember([CanBeNull] Instance instance)
    {
      return instance != null && (instance.IsSitecore || instance.IsSitecoreEnvironmentMember);
    }

    private bool IsNotHidden([CanBeNull] Instance instance)
    {
      return instance != null && !instance.IsHidden;
    }

    private void OnInstancesListUpdated()
    {
      EventHandler handler = InstancesListUpdated;
      if (handler != null)
      {
        handler(null, EventArgs.Empty);
      }
    }

    #endregion

    #region Public methods

    public Instance GetInstance(long id)
    {
      using (new ProfileSection("Get instance by id"))
      {
        ProfileSection.Argument("id", id);

        var instance = new Instance((int)id);

        return ProfileSection.Result(instance);
      }
    }

    #endregion
  }
}
