namespace MIGA.Core.Common
{
  using System.Data.SqlClient;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  public static class ProfileHelper
  {
    [NotNull]
    public static SqlConnectionStringBuilder GetValidConnectionString([NotNull] this IProfile profile)
    {
      Assert.ArgumentNotNull(profile, nameof(profile));
      var connectionString = profile.ConnectionString;
      var builder = new SqlConnectionStringBuilder(connectionString);
      Assert.IsNotNullOrEmpty(builder.DataSource, "Profile.ConnectionString.DataSource is null or empty");
      Assert.IsNotNullOrEmpty(builder.UserID, "Profile.ConnectionString.UserID is null or empty");
      Assert.IsNotNullOrEmpty(builder.Password, "Profile.ConnectionString.Password is null or empty");

      return builder;
    }
  }
}