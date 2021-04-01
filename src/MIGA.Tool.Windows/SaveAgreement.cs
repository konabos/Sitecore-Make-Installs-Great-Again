using System.IO;
using MIGA.Tool.Base.Wizards;

namespace MIGA.Tool.Windows
{
  public class SaveAgreement : IAfterLastWizardPipelineStep
  {
    public void Execute(WizardArgs args)
    {
      var tempFolder = ApplicationManager.TempFolder;
      if (!Directory.Exists(tempFolder))
      {
        Directory.CreateDirectory(tempFolder);
      }

      File.WriteAllText(Path.Combine(tempFolder, "agreement-accepted.txt"), @"agreement accepted");
    }
  }
}
