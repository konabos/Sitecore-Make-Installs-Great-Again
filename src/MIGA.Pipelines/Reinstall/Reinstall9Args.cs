using MIGA.Pipelines.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIGA.Pipelines.Install;
using MIGA.Sitecore9Installer;

namespace MIGA.Pipelines.Reinstall
{
  public class Reinstall9Args: Install9Args
  {
    public Reinstall9Args(Tasker tasker) : base(tasker)
    {
      this.PipelineName = "reinstall9";
    }
  }
}
