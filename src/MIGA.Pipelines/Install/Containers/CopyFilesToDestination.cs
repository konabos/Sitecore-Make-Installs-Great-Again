using JetBrains.Annotations;
using System.IO;
using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines.Install.Containers
{
  public class CopyFilesToDestination : Processor
  {
    protected override void Process([NotNull] ProcessorArgs arguments)
    {
      InstallContainerArgs args = (InstallContainerArgs)arguments;
      FileSystem.FileSystem.Local.Directory.Copy(args.FilesRoot, args.Destination, true);
      if (FileSystem.FileSystem.Local.File.Exists(args.EnvModel.SitecoreLicense))
      {
        string licensePath = Path.Combine(args.Destination, "license.xml");
        FileSystem.FileSystem.Local.File.Copy(args.EnvModel.SitecoreLicense, licensePath);
      }
    }
  }
}
