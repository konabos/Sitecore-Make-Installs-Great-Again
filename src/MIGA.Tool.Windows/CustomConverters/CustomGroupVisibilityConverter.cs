using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Sitecore.Diagnostics.Base;
using JetBrains.Annotations;
using MIGA.Instances;
using MIGA.Tool.Base.Plugins;
using Sitecore.Diagnostics.Logging;

namespace MIGA.Tool.Windows.CustomConverters
{
  public class CustomGroupVisibilityConverter : IValueConverter
  {
    #region Fields

    [NotNull]
    private IMainWindowGroup Group { get; }

    #endregion

    #region Constructors

    public CustomGroupVisibilityConverter([NotNull] IMainWindowGroup mainWindowGroup)
    {
      Assert.ArgumentNotNull(mainWindowGroup, nameof(mainWindowGroup));

      this.Group = mainWindowGroup;
    }

    #endregion

    #region Public methods

    [CanBeNull]
    public object Convert([CanBeNull] object value, [CanBeNull] Type targetType, [CanBeNull] object parameter, [CanBeNull] CultureInfo culture)
    {
      using (new ProfileSection("Checking if group is visible", this))
      {
        ProfileSection.Argument("this.group", this.Group.GetType().FullName);
        return this.Group.IsVisible(MainWindow.Instance, value as Instance) ? Visibility.Visible : Visibility.Collapsed;
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
