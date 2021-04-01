using JetBrains.Annotations;

namespace MIGA.Pipelines.Reinstall.Containers
{
  [UsedImplicitly]
  public class CleanupSolrDataProcessor : CleanupDataBaseProcessor
  {
    protected override string DataFolder => "solr-data";
  }
}