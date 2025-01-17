﻿namespace MIGA.Pipelines.Install
{
  using JetBrains.Annotations;

  public static class Settings
  {
    #region Fields

    [NotNull]
    public static readonly AdvancedProperty<string> AppMongoConnectionString = AdvancedSettings.Create("App/Mongo/ConnectionString", @"mongodb://localhost:27017/");

    [NotNull]
    public static readonly AdvancedProperty<bool> CoreDeleteMongoDatabases = AdvancedSettings.Create("Core/Delete/MongoDatabases", true);

    [NotNull]
    public static readonly AdvancedProperty<string> CoreExportTempFolderLocation = AdvancedSettings.Create("Core/Export/TempFolder/Location", string.Empty);

    [NotNull]
    public static readonly AdvancedProperty<int> CoreExportZipCompressionLevel = AdvancedSettings.Create("Core/Export/ZipCompressionLevel", 9);
    [NotNull]
    public static readonly AdvancedProperty<bool> CoreInstallDictionaries = AdvancedSettings.Create("Core/Install/Defaults/Dictionaries", true);
    [NotNull]
    public static readonly AdvancedProperty<bool> CoreInstallCreateServerTxt = AdvancedSettings.Create("Core/Install/CreateServerTxt", false);

    [NotNull]
    public static readonly AdvancedProperty<bool> CoreInstallRenameSqlFiles = AdvancedSettings.Create("Core/Install/RenameSqlFiles", false);

    [NotNull]
    public static readonly AdvancedProperty<string> CoreInstallMailServerAddress = AdvancedSettings.Create("Core/Install/MailServer/Address", string.Empty);

    [NotNull]
    public static readonly AdvancedProperty<string> CoreInstallMailServerCredentials = AdvancedSettings.Create("Core/Install/MailServer/Credentials", string.Empty);

    [NotNull]
    public static readonly AdvancedProperty<bool> CoreInstallNotFoundTransfer = AdvancedSettings.Create("Core/Install/NotFoundError/UseTransfer", false);

    [NotNull]
    public static readonly AdvancedProperty<bool> CoreInstallRadControls = AdvancedSettings.Create("Core/Install/Defaults/RadControls", true);

    [NotNull]
    public static readonly AdvancedProperty<string> CoreInstallTempFolderLocation = AdvancedSettings.Create("Core/Install/TempFolder/Location", string.Empty);

    [NotNull]
    public static readonly AdvancedProperty<string> CoreInstallWebServerIdentity = AdvancedSettings.Create("Core/Install/WebServer/Identity", "NetworkService");

    [NotNull]
    public static readonly AdvancedProperty<string> CoreInstallWebServerIdentityPassword = AdvancedSettings.Create("Core/Install/WebServer/Identity/Password", string.Empty);

    public static readonly AdvancedProperty<string> CoreInstallHttpRuntimeExecutionTimeout = AdvancedSettings.Create("Core/Install/HttpRuntime/ExecutionTimeout", string.Empty);

    #endregion
  }
}