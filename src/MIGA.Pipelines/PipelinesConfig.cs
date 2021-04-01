namespace MIGA.Pipelines
{
  public static class PipelinesConfig
  {
    public const string Contents = @"<pipelines>
<installSolr title=""Installing the solr"">
    <step>
      <hive type=""MIGA.Pipelines.Install.RunPSTasksProcessor, MIGA.Pipelines"" />
    </step>   
</installSolr>
<reinstall9 title=""Reinstalling the instance"">
    <step>
      <hive type=""MIGA.Pipelines.Install.RunPSTasksProcessor, MIGA.Pipelines"" param=""uninstall"" />
    </step>
    <step>
       <processor type=""MIGA.Pipelines.Reinstall.Reinstall9SwitchMode, MIGA.Pipelines"" title=""Switch mode to install"" />
    </step>
    <step>
      <hive type=""MIGA.Pipelines.Install.RunPSTasksProcessor, MIGA.Pipelines"" param=""install"" />
    </step>
</reinstall9>

<installContainer title=""Deploying new container environment"">
    <step>
      <processor type=""MIGA.Pipelines.Install.Containers.InstallDockerToolsProcessor, MIGA.Pipelines"" title=""Install 'SitecoreDockerTools' ps module""/>      
      <processor type=""MIGA.Pipelines.Install.Containers.CopyFilesToDestination, MIGA.Pipelines"" title=""Copy files to destination folder""/>      
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Install.Containers.ConvertLicenseProcessor, MIGA.Pipelines"" title=""Convert sitecore license""/>
      <processor type=""MIGA.Pipelines.Install.Containers.GenerateIdEnvValuesProcessor, MIGA.Pipelines"" title=""Generate 'SITECORE_ID*' .env values""/>
      <processor type=""MIGA.Pipelines.Install.Containers.GenerateSqlAdminPasswordProcessor, MIGA.Pipelines"" title=""Generate 'SQL_SA_PASSWORD' .env value""/>
      <processor type=""MIGA.Pipelines.Install.Containers.GenerateTelerikKeyProcessor, MIGA.Pipelines"" title=""Generate 'TELERIK_ENCRYPTION_KEY' .env value""/>
      <processor type=""MIGA.Pipelines.Install.Containers.GenerateReportingApiKeyProcessor, MIGA.Pipelines"" title=""Generate 'REPORTING_API_KEY' .env value""/>
      <processor type=""MIGA.Pipelines.Install.Containers.GenerateCertificatesProcessor, MIGA.Pipelines"" title=""Generate certificates""/>
      <processor type=""MIGA.Pipelines.Install.Containers.AddHostsProcessor, MIGA.Pipelines"" title=""Update hosts file""/>
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Install.Containers.WriteEnvFileProcessor, MIGA.Pipelines"" title=""Write .env file""/>
      <processor type=""MIGA.Pipelines.Install.Containers.GenerateEnvironmentData, MIGA.Pipelines"" title=""Add Sitecore environment data""/>
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Install.Containers.RunDockerProcessor, MIGA.Pipelines"" title=""Run docker""/>
    </step>
</installContainer>
<deleteContainer title=""Uninstalling container environment"">
    <step>
            <processor type=""MIGA.Pipelines.Delete.Containers.RemoveFromDocker, MIGA.Pipelines"" title=""Remove environment from Docker""/>
    </step>
    <step>
            <processor type=""MIGA.Pipelines.Delete.Containers.RemoveHostsProcessor, MIGA.Pipelines"" title=""Update hosts file""/>
    </step>
    <step>
            <processor type=""MIGA.Pipelines.Delete.Containers.RemoveEnvironmentFolder, MIGA.Pipelines"" title=""Remove environment folder""/>
    </step>
    <step>
            <processor type=""MIGA.Pipelines.Delete.Containers.CleanupEnvironmentData, MIGA.Pipelines"" title=""Cleanup environment data""/>
    </step>
</deleteContainer>
<reinstallContainer title=""Reinstalling container environment"">
    <step>
            <processor type=""MIGA.Pipelines.Reinstall.Containers.RemoveFromDockerProcessor, MIGA.Pipelines"" title=""Remove environment from docker""/>
    </step>
    <step>
            <processor type=""MIGA.Pipelines.Reinstall.Containers.CleanupSolrDataProcessor, MIGA.Pipelines"" title=""Remove Solr data""/>
            <processor type=""MIGA.Pipelines.Reinstall.Containers.CleanupSqlDataProcessor, MIGA.Pipelines"" title=""Remove SQL data""/>
    </step>
    <step>
            <processor type=""MIGA.Pipelines.Reinstall.Containers.RunDockerProcessor, MIGA.Pipelines"" title=""Start environment in Docker""/>            
    </step>

</reinstallContainer>

<install9 title=""Installing the instance"">
    <step>
      <processor type=""MIGA.Pipelines.Install.GenerateUnInstallParameters, MIGA.Pipelines"" title=""Generate Uninstall data"" />
      <processor type=""MIGA.Pipelines.Install.GenerateSitecoreEnvironmentData, MIGA.Pipelines"" title=""Generate Sitecore environment data"" />
      <hive type=""MIGA.Pipelines.Install.RunPSTasksProcessor, MIGA.Pipelines""  />
    </step>
  </install9>
<delete9 title=""UnInstalling the instance"">
    <step>
      <hive type=""MIGA.Pipelines.Install.RunPSTasksProcessor, MIGA.Pipelines"" param=""uninstall"" />
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Delete.CleanUp, MIGA.Pipelines"" title=""Clean Up"" />
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Delete.DeleteSitecoreEnvironmentData, MIGA.Pipelines"" title=""Delete Sitecore environment data"" />
    </step>
  </delete9>
  <install title=""Installing the {InstanceName} instance"">
    <step>
      <processor type=""MIGA.Pipelines.Install.CheckPackageIntegrity, MIGA.Pipelines"" title=""Validating install package"" />
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Install.GrantPermissions, MIGA.Pipelines"" title=""Granting permissions"" />
      <processor type=""MIGA.Pipelines.Install.Extract, MIGA.Pipelines"" title=""Extracting files"">
        <processor type=""MIGA.Pipelines.Install.CopyLicense, MIGA.Pipelines"" title=""Copying license"" />
        <processor type=""MIGA.Pipelines.Install.SetupWebsite, MIGA.Pipelines"" title=""Configuring IIS website"" />
        <processor type=""MIGA.Pipelines.Install.UpdateWebConfig, MIGA.Pipelines"" title=""Setting data folder"" />
        <processor type=""MIGA.Pipelines.Install.AddServerTxt, MIGA.Pipelines"" title=""Adding server.txt file"" />
      </processor>
      <processor type=""MIGA.Pipelines.Install.UpdateHosts, MIGA.Pipelines"" title=""Updating hosts file"" />
      <processor type=""MIGA.Pipelines.Install.InstallRoles, MIGA.Pipelines"" title=""Installing configuration roles"" />
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Install.AttachDatabases, MIGA.Pipelines"" title=""Attaching databases"" />
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Install.Modules.InstallActions, MIGA.Pipelines"" param=""archive""
                  title=""Modules: installing archive-based modules"" />
      <processor type=""MIGA.Pipelines.Install.Modules.CopyAgentFiles, MIGA.Pipelines""
                  title=""Modules: copying agent files"">
        <processor type=""MIGA.Pipelines.Install.Modules.CopyPackages, MIGA.Pipelines"" title=""Modules: copying packages"">
          <processor type=""MIGA.Pipelines.Install.Modules.InstallActions, MIGA.Pipelines"" param=""package|before""
                      title=""Modules: performing pre-install actions"">
            <processor type=""MIGA.Pipelines.Install.Modules.StartInstance, MIGA.Pipelines""
                        title=""Modules: starting instance"">
              <processor type=""MIGA.Pipelines.Install.Modules.InstallPackages, MIGA.Pipelines""
                          title=""Modules: installing packages"">
                <processor type=""MIGA.Pipelines.Install.Modules.StartInstance, MIGA.Pipelines""
                            title=""Modules: starting instance (again)"">
                  <processor type=""MIGA.Pipelines.Install.Modules.PerformPostStepActions, MIGA.Pipelines""
                              title=""Modules: performing post-step actions"">
                    <processor type=""MIGA.Pipelines.Install.Modules.InstallActions, MIGA.Pipelines""
                                param=""package|after"" title=""Modules: performing post-install actions"">
                      <processor type=""MIGA.Pipelines.Install.Modules.DeleteAgentPages, MIGA.Pipelines""
                                  title=""Modules: agent files"" />
                    </processor>
                  </processor>
                </processor>
              </processor>
            </processor>
          </processor>
        </processor>
      </processor>
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Install.Modules.StartInstance, MIGA.Pipelines""
                  title=""Starting instance"" param=""nowait"" />
    </step>
  </install>
  <multipleDeletion title=""Multiple deletion"">
    <step>
      <processor type=""MIGA.Pipelines.MultipleDeletion.MultipleDeletion, MIGA.Pipelines""
                  title=""Deleting the selected instances"" />
    </step>
  </multipleDeletion>
  <delete title=""Deleting the {InstanceName} instance"">
    <step>
      <processor type=""MIGA.Pipelines.Delete.InitializeArgs, MIGA.Pipelines"" title=""Initializing arguments"" />
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Delete.DeleteRegistryKey, MIGA.Pipelines"" title=""Deleting registry key"" />
      <processor type=""MIGA.Pipelines.Delete.StopInstance, MIGA.Pipelines"" title=""Stopping application"" />
      <processor type=""MIGA.Pipelines.Delete.DeleteDataFolder, MIGA.Pipelines"" title=""Deleting data folder"" />
      <processor type=""MIGA.Pipelines.Delete.DeleteDatabases, MIGA.Pipelines"" title=""Deleting databases"" />
      <processor type=""MIGA.Pipelines.Delete.DeleteMongoDatabases, MIGA.Pipelines"" title=""Deleting databases"" />
      <processor type=""MIGA.Pipelines.Delete.DeleteWebsiteFolder, MIGA.Pipelines"" title=""Deleting website folder"" />
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Delete.DeleteRootFolder, MIGA.Pipelines"" title=""Deleting root folder"" />
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Delete.DeleteWebsite, MIGA.Pipelines"" title=""Deleting website"" />
      <processor type=""MIGA.Pipelines.Delete.UpdateHosts, MIGA.Pipelines"" title=""Updating the hosts file"" />
    </step>
  </delete>
  <reinstall title=""Reinstalling the {InstanceName} instance"">
    <step>
      <processor type=""MIGA.Pipelines.Reinstall.CheckPackageIntegrity, MIGA.Pipelines"" title=""Validating package"" />
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Reinstall.StopInstance, MIGA.Pipelines"" title=""Stopping application"" />
      <processor type=""MIGA.Pipelines.Reinstall.DeleteDataFolder, MIGA.Pipelines"" title=""Deleting data folder"" />
      <processor type=""MIGA.Pipelines.Reinstall.DeleteDatabases, MIGA.Pipelines"" title=""Deleting databases"" />
      <processor type=""MIGA.Pipelines.Reinstall.DeleteWebsite, MIGA.Pipelines"" title=""Deleting IIS website"" />
      <processor type=""MIGA.Pipelines.Reinstall.DeleteWebsiteFolder, MIGA.Pipelines"" title=""Deleting website folder"" />
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Reinstall.DeleteRootFolder, MIGA.Pipelines"" title=""Recreating root folder"" />
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Reinstall.Extract, MIGA.Pipelines"" title=""Extracting files"">
        <processor type=""MIGA.Pipelines.Reinstall.CopyLicense, MIGA.Pipelines"" title=""Copying license"" />
        <processor type=""MIGA.Pipelines.Reinstall.SetupWebsite, MIGA.Pipelines"" title=""Configuring IIS website"" />
        <processor type=""MIGA.Pipelines.Reinstall.UpdateWebConfig, MIGA.Pipelines"" title=""Setting the data folder"" />  
        <processor type=""MIGA.Pipelines.Reinstall.AddServerTxt, MIGA.Pipelines"" title=""Adding server.txt file"" />
        <processor type=""MIGA.Pipelines.Reinstall.DeleteTempFolder, MIGA.Pipelines"" title=""Collecting garbage"" />
      </processor>
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Reinstall.AttachDatabases, MIGA.Pipelines"" title=""Attaching databases"" />
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Reinstall.StartInstance, MIGA.Pipelines"" title=""Starting instance"" param=""nowait"" />
    </step>
  </reinstall>
  <installmodules title=""Installing modules to the {InstanceName} instance"">
    <processor type=""MIGA.Pipelines.InstallModules.InstallActions, MIGA.Pipelines"" param=""archive""
                title=""Installing archive-based modules"" />
    <processor type=""MIGA.Pipelines.InstallModules.CopyAgentFiles, MIGA.Pipelines"" title=""Copying agent files"">
      <processor type=""MIGA.Pipelines.InstallModules.CopyPackages, MIGA.Pipelines""
                  title=""Copying packages into Website folder"">
        <processor type=""MIGA.Pipelines.InstallModules.InstallActions, MIGA.Pipelines"" param=""package|before""
                    title=""Performing pre-install actions"">
          <processor type=""MIGA.Pipelines.InstallModules.StartInstance, MIGA.Pipelines"" title=""Starting the instance"">
            <processor type=""MIGA.Pipelines.InstallModules.InstallPackages, MIGA.Pipelines""
                        title=""Installing the packages"">
              <processor type=""MIGA.Pipelines.InstallModules.StartInstance, MIGA.Pipelines""
                          title=""Starting the instance (again)"">
                <processor type=""MIGA.Pipelines.InstallModules.PerformPostStepActions, MIGA.Pipelines""
                            title=""Performing post-step actions"">
                  <processor type=""MIGA.Pipelines.InstallModules.InstallActions, MIGA.Pipelines"" param=""package|after""
                              title=""Performing post-install actions"">
                    <processor type=""MIGA.Pipelines.InstallModules.DeleteAgentPages, MIGA.Pipelines""
                                title=""Deleting agent files"">
                      <processor type=""MIGA.Pipelines.InstallModules.StartInstance, MIGA.Pipelines""
                          title=""Starting instance"" param=""nowait"" />
                    </processor>
                  </processor>
                </processor>
              </processor>
            </processor>
          </processor>
        </processor>
      </processor>
    </processor>
  </installmodules>
  <backup title=""Backing up the {InstanceName} instance"">
    <processor type=""MIGA.Pipelines.Backup.BackupDatabases, MIGA.Pipelines"" title=""Backing up databases"" />
    <processor type=""MIGA.Pipelines.Backup.BackupMongoDatabases, MIGA.Pipelines"" title=""Backing up MongoDB databases"" />
    <processor type=""MIGA.Pipelines.Backup.BackupFiles, MIGA.Pipelines"" title=""Backing up files"" />
  </backup>
  <restore title=""Restoring the {InstanceName} instance"">
    <processor type=""MIGA.Pipelines.Restore.RestoreDatabases, MIGA.Pipelines"" title=""Restoring databases"" />
    <processor type=""MIGA.Pipelines.Restore.RestoreMongoDatabases, MIGA.Pipelines"" title=""Restoring MongoDB databases"" />
    <processor type=""MIGA.Pipelines.Restore.DeleteFiles, MIGA.Pipelines"" title=""Deleting files"">
      <processor type=""MIGA.Pipelines.Restore.RestoreFiles, MIGA.Pipelines"" title=""Restoring files"" />
    </processor>
  </restore>
  <export title=""Exporting the {InstanceName} instance"">
    <step>
      <processor type=""MIGA.Pipelines.Export.ExportDatabases, MIGA.Pipelines"" title=""Exporting databases"" />
      <processor type=""MIGA.Pipelines.Export.ExportMongoDatabases, MIGA.Pipelines"" title=""Exporting MongoDB databases"" />
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Export.ExportFiles, MIGA.Pipelines"" title=""Exporting files"" />
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Export.ExportSettings, MIGA.Pipelines"" title=""Exporting settings"" />
    </step>
    <step>
      <processor type=""MIGA.Pipelines.Export.ExportPostActions, MIGA.Pipelines"" title=""Assembling zip package"" />
    </step>
  </export>
  <import title=""Importing instance"">
    <processor type=""MIGA.Pipelines.Import.ImportInitialization, MIGA.Pipelines"" title=""Initialization"">
      <processor type=""MIGA.Pipelines.Import.ImportRestoreDatabases, MIGA.Pipelines"" title=""Restore databases"" />
      <processor type=""MIGA.Pipelines.Import.ImportRestoreMongoDatabases, MIGA.Pipelines""
                  title=""Restore MongoDB databases"" />
      <processor type=""MIGA.Pipelines.Import.ImportUnpackSolution, MIGA.Pipelines"" title=""Unpack solution"">
        <processor type=""MIGA.Pipelines.Import.UpdateConnectionStrings, MIGA.Pipelines""
                    title=""Update connection strings"" />
        <processor type=""MIGA.Pipelines.Import.UpdateDataFolder, MIGA.Pipelines"" title=""Update data folder"" />
        <processor type=""MIGA.Pipelines.Import.UpdateLicense, MIGA.Pipelines"" title=""Update license"" />
      </processor>
      <processor type=""MIGA.Pipelines.Import.ImportRegisterWebsite, MIGA.Pipelines"" title=""Update IIS metabase"" />
      <processor type=""MIGA.Pipelines.Import.ImportHostNames, MIGA.Pipelines"" title=""Update hosts file"" />
    </processor>
  </import>
</pipelines>";
  }
}
