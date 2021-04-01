﻿namespace MIGA.Pipelines.Agent
{
  public static class PublishAgentFiles
  {
    #region Publish

    public const string PublishContents = @"
<%@ Page Language=""C#"" AutoEventWireup=""true"" %>
<%@ Import Namespace=""System.IO"" %>
<%@ Import Namespace=""Sitecore"" %>
<%@ Import Namespace=""Sitecore.Configuration"" %>
<%@ Import Namespace=""Sitecore.Data.Engines"" %>
<%@ Import Namespace=""Sitecore.Data.Proxies"" %>
<%@ Import Namespace=""Sitecore.Diagnostics"" %>
<%@ Import Namespace=""Sitecore.Globalization"" %>
<%@ Import Namespace=""Sitecore.Publishing"" %>
<%@ Import Namespace=""Sitecore.SecurityModel"" %>
<%@ Import Namespace=""Sitecore.Security.Accounts"" %>

<script runat=""server"">

#region Methods

  protected override void OnInitComplete(EventArgs e)
  {
    try
    {
      Sitecore.Context.SetActiveSite(""shell"");
      using (new UserSwitcher(""sitecore\\admin"", true))
      {
        using (new ProxyDisabler())
        {
          using (new SyncOperationContext())
          {
            var language = Language.Parse(EmptyToNull(this.Request.QueryString[""lang""]) ?? EmptyToNull(this.Request.QueryString[""language""]) ?? ""en"");
            var languages = new [] { language };
            var target = Factory.GetDatabase(EmptyToNull(this.Request.QueryString[""target""]) ?? ""web"");
            var source = Factory.GetDatabase(EmptyToNull(this.Request.QueryString[""source""]) ?? ""master"");
            var targets = new [] { target };
            var id = PublishManager.PublishSmart(source, targets, languages);
            this.UpdateStatus(@""Started: publish "" + id);            
          }
        }
      }
    }
    catch (Exception ex)
    {
      var inn = string.Empty;
      if (ex.InnerException != null)
      {
        inn = ""\n\nInner Exception:\n"" + ex.InnerException;
      }

      this.Finish(@""Error: "" + ex + inn);
    }
  }

  private void Finish(string message)
  {
    Log.Info(@""[MIGA] " + PublishFileName + @": "" + message, this);
    this.UpdateStatus(message);
    this.Response.Write(message);
  }

  private void UpdateStatus(string message)
  {
    var installedTemp = this.Server.MapPath(Path.Combine(Settings.TempFolderPath, ""MIGA.status""));
    File.WriteAllText(installedTemp, message);
  }

  private string GetFilePath(string name)
  {
    Assert.ArgumentNotNullOrEmpty(name, ""name"");

    var packageFolderPath = Sitecore.Configuration.Settings.PackagePath;
    Assert.IsNotNullOrEmpty(packageFolderPath, ""packageFolderPath"");

    // if path is virtual i.e. not C:\something then do a map path
    if (packageFolderPath.Length < 2 || packageFolderPath[1] != ':')
    {
      packageFolderPath = packageFolderPath.TrimStart('/');
      var prefix = ""~/"";
      if (packageFolderPath.StartsWith(prefix))
      {
        packageFolderPath = packageFolderPath.Substring(prefix.Length);
      }

      packageFolderPath = Server.MapPath(prefix + packageFolderPath);
    }
      
    return Path.Combine(packageFolderPath, name);      
  }

  private string EmptyToNull(string str)
  {
    return string.IsNullOrEmpty(str) ? null : str;
  }

#endregion

</script>";
    public const string PublishFileName = @"Publish.aspx";

    #endregion

    #region Status

    public const string StatusContents = @"<%@ Page Language=""C#"" AutoEventWireup=""true"" %>
<%@ Import Namespace=""System.IO"" %>
<%@ Import Namespace=""Sitecore"" %>
<%@ Import Namespace=""Sitecore.Configuration"" %>
<%@ Import Namespace=""Sitecore.Diagnostics"" %>
<%@ Import Namespace=""Sitecore.Publishing"" %>
<%@ Import Namespace=""Sitecore.Jobs"" %>
<script runat=""server"">

#region Methods

  protected override void OnInitComplete(EventArgs e)
  {
    var installedTemp = this.Server.MapPath(Path.Combine(Settings.TempFolderPath, ""MIGA.status""));
    var message = GetMessage(installedTemp);
    
    Log.Info(@""[MIGA] " + AgentFiles.StatusFileName + @": "" + message, this);
    this.Response.Write(message);
  }

  private string GetMessage(string installedTemp)
  {
    if (File.Exists(installedTemp))
    {
      var message = File.ReadAllText(installedTemp);
      var pos = message.LastIndexOf(' ');
      if(pos<0) throw new Exception(""CANT BE"");
      var id = message.Substring(pos).Trim();
      var state = PublishManager.GetStatus(Handle.Parse(id));
      message = GetState(state) + "": publish "" + id;      
      return message;
    }
    else
    {
      return @""Pending: no information"";
    }
  }

  private string GetState(PublishStatus state)
  {
    if (state == null)
    {
      return ""Completed""; // needs adjusting
    }

    if (state.IsDone)
    {
      return ""Completed"";
    }
      
    if (state.Failed)
    {
      return ""Failed"";
    }
      
    if (state.State == JobState.Running)
    {
      return ""Running"";
    }
      
    if (state.State == JobState.Initializing)
    {
      return ""Initializing"";
    }
      
    if (state.State == JobState.Queued)
    {
      return ""Queued"";
    }

    return ""Unknown"";
  }

#endregion

</script>";
    public const string StatusFileName = @"Status.aspx";

    #endregion
  }
}