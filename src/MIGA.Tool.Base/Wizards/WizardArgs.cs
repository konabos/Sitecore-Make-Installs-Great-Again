﻿using MIGA.Loggers;
using MIGA.Pipelines.Processors;

namespace MIGA.Tool.Base.Wizards
{
  using System;
  using System.Windows;
  using MIGA.Loggers;
  using MIGA.Pipelines.Processors;

  #region

  #endregion

  public abstract class WizardArgs : IDisposable
  {
    private ILogger _logger;

    #region Public properties

    public Window WizardWindow { get; set; }

    public ILogger Logger
    {
      get
      {
        if (this._logger == null)
        {
          return this._logger = new EmptyLogger();
        }

        return this._logger;
      }
      set
      {
        if (value != null)
        {
          this._logger = value;
        }
      }
    }

    //Indicates if the installation has been completed
    public bool ShouldRefreshInstancesList { get; set; }

    #endregion

    #region Public Methods

    public virtual ProcessorArgs ToProcessorArgs()
    {
      return null;
    }

    #endregion

    public virtual void Dispose()
    {
    }
  }
}