﻿using System;
using Sitecore.Diagnostics.Base;

namespace MIGA.Loggers
{
  public class Logger : ILogger
  {
    internal Action<string> _WriteLogMessage;

    public Logger(Action<string> writeLogMessage)
    {
      Assert.ArgumentNotNull(writeLogMessage);
      this._WriteLogMessage = writeLogMessage;
    }

    public void Info(string message, bool includeSeverityLevel = true)
    {
      DoWriteMessage(message, includeSeverityLevel, "INFO");
    }

    public void Warn(string message, bool includeSeverityLevel = true)
    {
      DoWriteMessage(message, includeSeverityLevel, "WARN");
    }

    public void Error(string message, bool includeSeverityLevel = true)
    {
      DoWriteMessage(message, includeSeverityLevel, "ERROR");
    }

    private void DoWriteMessage(string message, bool includeSeverityLevel = true, string severity = "")
    {
      string time = DateTime.Now.ToString("HH:mm:ss");
      string text = includeSeverityLevel ? $"[{time}] {severity}: {message}" : $"[{time}] {message}";
      this._WriteLogMessage(text);
    }
  }
}