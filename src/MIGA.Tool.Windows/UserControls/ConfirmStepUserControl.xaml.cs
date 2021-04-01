using MIGA.Sitecore9Installer;
using MIGA.Tool.Base;
using MIGA.Tool.Base.Pipelines;
using MIGA.Tool.Base.Profiles;
using MIGA.Tool.Base.Wizards;
using System.IO;
using System.Linq;

namespace MIGA.Tool.Windows.UserControls
{
  /// <summary>
  ///   The confirm step user control.
  /// </summary>
  public partial class ConfirmStepUserControl
  {
    #region Constructors

    public ConfirmStepUserControl(string param)
    {
      InitializeComponent();
      TextBlock.Text = param;
    } 
    #endregion
  }
}