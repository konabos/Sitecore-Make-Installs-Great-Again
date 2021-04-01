using MIGA.Tool.Base.Plugins;

namespace MIGA.Tool.Windows
{
  public class GroupDefinition
  {
    public IMainWindowGroup Handler { get; set; }

    public ButtonDefinition[] Buttons { get; set; }

    public string Name { get; set; }
  }
}