using System;
using System.Globalization;
using System.Windows.Data;
using Sitecore.Diagnostics.Base;
using JetBrains.Annotations;
using MIGA.Instances;
using MIGA.Tool.Base.Plugins;
using Sitecore.Diagnostics.Logging;

namespace MIGA.Tool.Windows.CustomConverters
{
  public class CustomButtonEnabledConverter : IValueConverter
  {
    #region Fields

    [NotNull]
    private IMainWindowButton Button { get; }

    #endregion

    #region Constructors

    public CustomButtonEnabledConverter([NotNull] IMainWindowButton mainWindowButton)
    {
      Assert.ArgumentNotNull(mainWindowButton, nameof(mainWindowButton));

      this.Button = mainWindowButton;
    }

    #endregion

    #region Public methods

    [CanBeNull]
    public object Convert([CanBeNull] object value, [CanBeNull] Type targetType, [CanBeNull] object parameter, [CanBeNull] CultureInfo culture)
    {
      using (new ProfileSection("Checking if button is enabled", this))
      {
        ProfileSection.Argument("this.button", this.Button.GetType().FullName);
        return this.Button.IsEnabled(MainWindow.Instance, value as Instance);
      }
    }

    [CanBeNull]
    public object ConvertBack([CanBeNull] object value, [CanBeNull] Type targetType, [CanBeNull] object parameter, [CanBeNull] CultureInfo culture)
    {
      throw new NotSupportedException();
    }

    #endregion
  }
}