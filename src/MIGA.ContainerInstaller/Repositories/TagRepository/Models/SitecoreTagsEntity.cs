using System.Collections.Generic;

namespace MIGA.ContainerInstaller.Repositories.TagRepository.Models
{
  public class SitecoreTagsEntity
  {
    public string Name { get; set; }

    public string Namespace { get; set; }

    public IEnumerable<TagEntity> Tags { get; set; }
  }
}