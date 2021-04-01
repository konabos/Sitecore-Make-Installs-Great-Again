using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGA
{
  public interface IValidateable
  {
    string ValidateAndGetError();
    bool HasAnyValuesInTheFields();
  }
}
