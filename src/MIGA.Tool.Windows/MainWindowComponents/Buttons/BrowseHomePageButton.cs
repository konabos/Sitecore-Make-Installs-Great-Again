using JetBrains.Annotations;

namespace MIGA.Tool.Windows.MainWindowComponents.Buttons
{
  [UsedImplicitly]
  public class BrowseHomePageButton : BrowseButton
  {
    public BrowseHomePageButton() : base("/")
    {
    }
  }
}