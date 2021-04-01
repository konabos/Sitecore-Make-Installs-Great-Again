using System.Collections.Generic;
using MIGA.Sitecore9Installer.Tasks;

namespace MIGA.Sitecore9Installer.Validation
{
  public interface IValidator
  {
    IEnumerable<ValidationResult> Evaluate(IEnumerable<Task> tasks);
    Dictionary<string, string> Data { get; set; }
  }
}
