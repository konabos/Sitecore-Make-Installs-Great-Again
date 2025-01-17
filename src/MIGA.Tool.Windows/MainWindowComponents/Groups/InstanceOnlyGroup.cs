﻿using System.Linq;
using System.Windows;
using MIGA.Instances;
using MIGA.Products;
using MIGA.SitecoreEnvironments;
using MIGA.Tool.Base.Plugins;

namespace MIGA.Tool.Windows.MainWindowComponents.Groups
{
  public class InstanceOnlyGroup : IMainWindowGroup
  {
    #region Fields

    private string label;

    #endregion

    #region Public methods

    public virtual bool IsVisible(Window mainWindow, Instance instance)
    {
      if (instance != null)
      {
        if (this.IsSitecoreContainer(instance))
        {
          return ButtonsConfiguration.Instance.SitecoreContainersGroups.Contains(this.label);
        }
        if (this.IsSitecoreMember(instance))
        {
          return ButtonsConfiguration.Instance.Sitecore9AndLaterMemberGroups.Contains(this.label);
        }
        if (this.IsSitecore9AndLater(instance))
        {
          return ButtonsConfiguration.Instance.Sitecore9AndLaterGroups.Contains(this.label);
        }
        if (this.IsSitecore8AndEarlier(instance))
        {
          return ButtonsConfiguration.Instance.Sitecore8AndEarlierGroups.Contains(this.label);
        }
      }

      return false;
    }

    #endregion

    #region Protected methods

    public InstanceOnlyGroup()
    {
      this.label = this.GetType().Name;
    }

    protected bool IsSitecoreContainer(Instance selectedInstance)
    {
      if (SitecoreEnvironmentHelper.GetExistingSitecoreEnvironment(selectedInstance.Name)?.EnvType == SitecoreEnvironment.EnvironmentType.Container)
      {
        return true;
      }

      return false;
    }

    protected bool IsSitecoreMember(Instance selectedInstance)
    {
      if (selectedInstance.Product == Product.Undefined)
      {
        return true;
      }

      return false;
    }

    protected bool IsSitecore9AndLater(Instance selectedInstance)
    {
      int version = this.GetSitecoreVersion(selectedInstance);

      if (version != default(int) && version >= 90)
      {
        return true;
      }

      return false;
    }

    protected bool IsSitecore8AndEarlier(Instance selectedInstance)
    {
      int version = this.GetSitecoreVersion(selectedInstance);

      if (version != default(int) && version < 90)
      {
        return true;
      }

      return false;
    }

    protected int GetSitecoreVersion(Instance selectedInstance)
    {
      int version;

      if (selectedInstance.Product.Release != null)
      {
        version = selectedInstance.Product.Release.Version.MajorMinorInt;
      }
      else
      {
        int.TryParse(selectedInstance.Product.ShortVersion, out version);
      }

      return version;
    }

    #endregion
  }
}
