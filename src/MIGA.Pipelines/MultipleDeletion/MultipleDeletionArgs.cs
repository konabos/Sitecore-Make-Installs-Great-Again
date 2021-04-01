using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines.MultipleDeletion
{
  using System.Collections.Generic;
  using System.Data.SqlClient;
  using MIGA.Pipelines.Processors;

  public class MultipleDeletionArgs : ProcessorArgs
  {
    #region Fields

    public SqlConnectionStringBuilder _ConnectionString;

    private readonly List<string> _Instances;

    #endregion

    #region Constructors

    public MultipleDeletionArgs(List<string> instances)
    {
      _Instances = instances;
    }

    #endregion

    #region Public properties

    public List<string> Instances
    {
      get
      {
        return _Instances;
      }
    }

    #endregion
  }
}