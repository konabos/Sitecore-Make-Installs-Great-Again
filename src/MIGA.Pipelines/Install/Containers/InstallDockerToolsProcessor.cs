using System.IO;
using JetBrains.Annotations;
using MIGA.ContainerInstaller;
using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines.Install.Containers
{
  [UsedImplicitly]
  public class InstallDockerToolsProcessor : Processor
  {
    protected override void Process(ProcessorArgs args)
    {
      string scriptFile = Path.Combine(Directory.GetCurrentDirectory(), "ContainerFiles/scripts/InstallDockerToolsModuleScript.txt");
      PSFileExecutor ps = new PSFileExecutor(scriptFile, Directory.GetCurrentDirectory());

      ps.Execute();
    }
  }
}
