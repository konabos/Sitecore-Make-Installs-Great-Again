using System.Collections.Generic;

namespace MIGA.ContainerInstaller.DockerCompose
{
  public interface IRepository<T>
    where T : class
  {
    IDictionary<string, T> GetServices(); // Get all services
  }
}