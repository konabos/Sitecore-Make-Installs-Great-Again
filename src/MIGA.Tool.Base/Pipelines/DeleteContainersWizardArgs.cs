using System;
using MIGA.ContainerInstaller;
using MIGA.Pipelines.Delete.Containers;
using MIGA.Pipelines.Processors;
using MIGA.Tool.Base.Wizards;

namespace MIGA.Tool.Base.Pipelines
{
  public class DeleteContainersWizardArgs : WizardArgs
  {
    public string DestinationFolder { get; set; }

    public Guid EnvironmentId { get; set; }

    public EnvModel Env { get; set; }

    public override ProcessorArgs ToProcessorArgs()
    {
      return new DeleteContainersArgs(
        this.DestinationFolder,
        this.Env,
        this.EnvironmentId,
        this.Logger
        );
    }
  }
}
