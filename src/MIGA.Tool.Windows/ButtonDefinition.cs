using MIGA.Tool.Base.Plugins;

namespace MIGA.Tool.Windows
{
  using JetBrains.Annotations;

  using MIGA.Tool.Base.Plugins;

  public class ButtonDefinition
  {
    public IMainWindowButton Handler { get; set; }

    public string Label { get; set; }

    public string Image { get; set; }

    [ItemCanBeNull]
    public ButtonDefinition[] Buttons { get; set; }

    public string Width { get; set; }
  }
}