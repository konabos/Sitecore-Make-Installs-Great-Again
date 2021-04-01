﻿using MIGA.Adapters.MongoDb;
using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines.Import
{
  using System.Collections.Generic;
  using System.Data.SqlClient;
  using MIGA.Adapters.MongoDb;
  using MIGA.Pipelines.Processors;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;
  using MIGA.Extensions;

  public class ImportArgs : ProcessorArgs
  {
    #region Constants

    public const string AppPoolSettingsFileName = "AppPoolSettings.xml";
    public const string WebsiteSettingsFileName = "WebsiteSettings.xml";

    #endregion

    // Site settings
    // public List<string> siteBindingsHostnames = new List<string>();
    #region Fields

    public string _AppPoolName = string.Empty;
    public Dictionary<string, int> _Bindings = new Dictionary<string, int>();
    public SqlConnectionStringBuilder _ConnectionString;
    public int _DatabaseNameAppend = -1;
    public string _OldSiteName = string.Empty;
    public string _PathToLicenseFile = string.Empty;
    public string _RootPath = string.Empty;
    public long? _SiteID = 0;
    public string _SiteName = string.Empty;
    public string _TemporaryPathToUnpack = string.Empty;
    public bool _UpdateLicense = false;
    public string _VirtualDirectoryPath = string.Empty;
    public string _VirtualDirectoryPhysicalPath = string.Empty;

    #endregion

    #region Properties

    public string PathToExportedInstance { get; set; }
    public List<MongoDbDatabaseImport> ExtractedMongoDatabases { get; set; }

    #endregion

    #region Constructors

    public ImportArgs([NotNull] string pathToExportedInstance, [NotNull] SqlConnectionStringBuilder connectionString)
    {
      Assert.ArgumentNotNull(pathToExportedInstance, nameof(pathToExportedInstance));
      PathToExportedInstance = pathToExportedInstance;
      _ConnectionString = connectionString;
    }

    public ImportArgs([NotNull] string pathToExportedInstance, [NotNull] string siteName, [NotNull] SqlConnectionStringBuilder connectionString)
    {
      Assert.ArgumentNotNull(pathToExportedInstance, nameof(pathToExportedInstance));
      PathToExportedInstance = pathToExportedInstance;
      _SiteName = siteName;
      _ConnectionString = connectionString;
    }

    public ImportArgs([NotNull] string pathToExportedInstance, [NotNull] string siteName, [NotNull] string temporaryPathToUnpack, [NotNull] string rootPath, [NotNull] SqlConnectionStringBuilder connectionString, bool updateLicense, [CanBeNull] string pathToLicenseFile, [NotNull] Dictionary<string, int> bindings)
    {
      Assert.ArgumentNotNull(pathToExportedInstance, nameof(pathToExportedInstance));
      PathToExportedInstance = pathToExportedInstance;
      _SiteName = siteName;
      _TemporaryPathToUnpack = temporaryPathToUnpack;
      _RootPath = rootPath;
      _VirtualDirectoryPhysicalPath = _RootPath.PathCombine("Website");
      _ConnectionString = connectionString;
      _UpdateLicense = updateLicense;
      _PathToLicenseFile = pathToLicenseFile;
      _Bindings = bindings;
    }

    #endregion
  }
}