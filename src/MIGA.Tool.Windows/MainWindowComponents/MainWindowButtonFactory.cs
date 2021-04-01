using MIGA.Instances;
using MIGA.Tool.Base.Plugins;
using MIGA.Tool.Windows.MainWindowComponents.Buttons;

namespace MIGA.Tool.Windows.MainWindowComponents
{
  public static class MainWindowButtonFactory
  {
    public static IMainWindowButton GetBrowseButton(Instance instance)
    {
      if (instance != null && instance.Type == Instance.InstanceType.SitecoreContainer)
      {
        return new BrowseSitecoreContainerWebsiteButton();
      }

      return new BrowseHomePageButton();
    }
  }
}
