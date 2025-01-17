﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using Task = MIGA.Sitecore9Installer.Tasks.Task;

namespace MIGA.Sitecore9Installer.Validation.Validators
{
  public class SqlVersionValidator : IValidator
  {
    public SqlVersionValidator()
    {
      this.Data = new Dictionary<string, string>();
    }

    public string Name { get; set; }
    public IEnumerable<ValidationResult> Evaluate(IEnumerable<Tasks.Task> tasks)
    {
      string server = this.Data["Server"];
      string user = this.Data["User"];
      string pass = this.Data["Password"];
      bool errors = false;
      foreach (Tasks.Task task in tasks.Where(t => t.LocalParams.Any(p => p.Name == user)))
      {
        string sereverVersion = string.Empty;
        sereverVersion = this.GetSqlVersion(task.LocalParams.Single(p => p.Name == server).Value,
        task.LocalParams.Single(p => p.Name == user).Value,
        task.LocalParams.Single(p => p.Name == pass).Value);
        string[] versions = Data["Versions"].Split(',');
        if (!versions.Any(v => Regex.Match(sereverVersion, v).Success))
        {
          errors = true;
          yield return new ValidationResult(ValidatorState.Error, "SQL server version is not compatible", null);
        }
      }

      if (!errors)
      {
        yield return new ValidationResult(ValidatorState.Success, this.SuccessMessage, null);
      }
    }

    protected internal virtual string GetSqlVersion(string server, string user, string password)
    {
      string connectionstring = $"Data Source={server};" +
                          $"Initial Catalog=master;User ID={user};" +
                          $"Password={password}";
      SqlConnection conn = new SqlConnection(connectionstring);
      conn.Open();
      string version = conn.ServerVersion;
      conn.Close();
      return version;
    }

    public Dictionary<string, string> Data { get; set; }

    public virtual string SuccessMessage => "The SQL server version is compatible.";
  }
}
