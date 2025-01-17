﻿using System.Collections.Generic;
using System.Linq;

namespace MIGA.Sitecore9Installer.Tasks
{
  //The class is only applicable  for 9.0 Initial Release XP0/XP1
  public class xConnectTaskWithPostSteps : SitecoreTask
  {
    private readonly string scriptTemaplate =
      "Invoke-Sqlcmd -ServerInstance \"$(ServerInstance)\" -Username \"$(SqlAdminUser)\" -Password \"$(SqlAdminPassword)\" -Query \"\nGO \nIF(SUSER_ID('$(UserName)') IS NULL) \nBEGIN \nCREATE LOGIN[$(UserName)] WITH PASSWORD = '$(Password)'; \nEND; \nGO \nUSE[$(DatabasePrefix)$(ShardMapManagerDatabaseNameSuffix)] \nIF NOT EXISTS(SELECT* FROM sys.database_principals WHERE name = N'$(UserName)') \nBEGIN \nCREATE USER[$(UserName)] FOR LOGIN[$(UserName)] \nGRANT SELECT ON SCHEMA::__ShardManagement TO[$(UserName)] \nGRANT EXECUTE ON SCHEMA :: __ShardManagement TO[$(UserName)] \nEND; \nGO \nUSE[$(DatabasePrefix)$(Shard0DatabaseNameSuffix)] \nIF NOT EXISTS(SELECT* FROM sys.database_principals WHERE name = N'$(UserName)') \nBEGIN \nCREATE USER[$(UserName)] FOR LOGIN[$(UserName)] \nEXEC[xdb_collection].[GrantLeastPrivilege] @UserName = '$(UserName)' \nEND; \nGO \nUSE[$(DatabasePrefix)$(Shard1DatabaseNameSuffix)] \nIF NOT EXISTS(SELECT* FROM sys.database_principals WHERE name = N'$(UserName)') \nBEGIN CREATE USER[$(UserName)] FOR LOGIN[$(UserName)] \nEXEC[xdb_collection].[GrantLeastPrivilege] @UserName ='$(UserName)' \nEND; \nGO\"";

    public xConnectTaskWithPostSteps(string taskName, int executionOrder, GlobalParameters globalParams, LocalParameters localParams,
      Dictionary<string, string> taskOptions) :
      base(taskName, executionOrder, globalParams, localParams, taskOptions)
    {
    }

    public string UserName { get; set; }
    public string Password { get; set; }
    public string DatabasePrefix { get; set; }
    public string ServerInstance { get; private set; }
    public string SqlAdminUser { get; private set; }
    public string SqlAdminPassword { get; private set; }
    public string ShardMapManagerDatabaseNameSuffix { get; set; }
    public string Shard0DatabaseNameSuffix { get; set; }
    public string Shard1DatabaseNameSuffix { get; set; }

    protected override string GetScript()
    {
      this.EvaluateLocalParams();
      ShardMapManagerDatabaseNameSuffix = TaskOptions["ShardMapManagerDatabaseNameSuffix"];
      Shard0DatabaseNameSuffix = TaskOptions["Shard0DatabaseNameSuffix"];
      Shard1DatabaseNameSuffix = TaskOptions["Shard1DatabaseNameSuffix"];
      UserName = LocalParams["SqlCollectionUser"]?.Value;
      Password = LocalParams["SqlCollectionPassword"]?.Value;
      DatabasePrefix = LocalParams["SqlDbPrefix"]?.Value;
      ServerInstance = GlobalParams["SqlServer"]?.Value;
      SqlAdminUser = GlobalParams["SqlAdminUser"]?.Value;
      SqlAdminPassword = GlobalParams["SqlAdminPassword"]?.Value;

      string baseScript = base.GetScript();
      if (UnInstall) return baseScript;

      //This script isn't invoked during uninstall. It's Ok.
      return baseScript += scriptTemaplate
        .Replace("$(ServerInstance)",ServerInstance)
        .Replace("$(SqlAdminUser)", SqlAdminUser)
        .Replace("$(SqlAdminPassword)", SqlAdminPassword)
        .Replace("$(UserName)", UserName)
        .Replace("$(Password)", Password)
        .Replace("$(DatabasePrefix)", DatabasePrefix)
        .Replace("$(ShardMapManagerDatabaseNameSuffix)", ShardMapManagerDatabaseNameSuffix)
        .Replace("$(Shard0DatabaseNameSuffix)", Shard0DatabaseNameSuffix)
        .Replace("$(Shard1DatabaseNameSuffix)", Shard1DatabaseNameSuffix);
    }
  }
}