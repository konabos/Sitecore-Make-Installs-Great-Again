using System.Collections.Generic;

namespace MIGA.ContainerInstaller.Repositories.TagRepository
{
  public interface ITagRepository
  {
    IEnumerable<string> GetTags();

    IEnumerable<string> GetSortedShortSitecoreTags(string sitecoreVersionParam, string namespaceParam);
  }
}