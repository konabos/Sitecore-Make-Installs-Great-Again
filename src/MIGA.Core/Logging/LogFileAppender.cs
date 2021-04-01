﻿namespace MIGA.Core.Logging
{
  using System;
  using log4net.Appender;
  using JetBrains.Annotations;
  using MIGA.Extensions;

  [UsedImplicitly]
  public class LogFileAppender : FileAppender
  {
    #region Fields

    protected string _ExpandedFilePath;

    #endregion

    #region Public properties

    [CanBeNull]
    public override string File
    {
      get
      {
        return base.File;
      }

      set
      {
        if (_ExpandedFilePath == null)
        {
          _ExpandedFilePath = value
            .Replace("$(logFolder)", ApplicationManager.LogsFolder)
            .Replace("$(currentFolder)", Environment.CurrentDirectory)
            .PipeTo(t => string.Format(t ?? "", DateTime.Now))            
            .PipeTo(Environment.ExpandEnvironmentVariables);
        }

        base.File = _ExpandedFilePath;
      }
    }

    #endregion     
  }
}