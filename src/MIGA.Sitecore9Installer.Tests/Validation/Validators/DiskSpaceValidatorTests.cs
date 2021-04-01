using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MIGA.Sitecore9Installer.Validation;
using MIGA.Sitecore9Installer.Validation.Validators;
using NSubstitute;
using Xunit;
using Task = MIGA.Sitecore9Installer.Tasks.Task;

namespace MIGA.Sitecore9Installer.Tests.Validation.Validators
{
  public class DiskSpaceValidatorTests
  {
    [Theory]
    [InlineData(@"C:\inetpub\wwwroot", 7368709120, ValidatorState.Success, "Hard disk has enough free space to continue the installation.")]
    [InlineData(@"C:\inetpub\wwwroot\", 5368709119, ValidatorState.Warning, @"Hard disk 'C:\' has a little free space.")]
    [InlineData(@"D:\inetpub\", 3221225470, ValidatorState.Error, @"Hard disk 'D:\' does not have enough free space to continue installation.")]
    [InlineData(@"D:\inetpub\wwwroot", -1, ValidatorState.Error, @"Hard disk 'D:\' has not been found.")]

    public void ReturnsValidValidationResults(string deployRoot, long freeSpace, ValidatorState expectedResult, string resultMessage)
    {
      //Arrange
      Tasks.Task task = Substitute.For<Tasks.Task>("", 0, null, null, new Dictionary<string, string>());
      GlobalParameters globals = new GlobalParameters();
      task.LocalParams.Returns(new LocalParameters(new List<InstallParam>(),globals));
      task.LocalParams.AddOrUpdateParam("DeployRoot",deployRoot,InstallParamType.String);

      List <Tasks.Task> tasks = Substitute.For<List<Tasks.Task>>();
      tasks.Add(task);
      
      DiskSpaceValidator val = Substitute.ForPartsOf<DiskSpaceValidator>();
      val.GetHardDriveFreeSpace(string.Empty).ReturnsForAnyArgs(freeSpace);

      val.Data["HardDriveWarningLimit"] = "5368709120";
      val.Data["HardDriveErrorLimit"] = "3221225472";
      val.Data["DeployRoot"] = "DeployRoot";
      //Act
      IEnumerable<ValidationResult> results = val.Evaluate(tasks);

      //Assert
      Assert.Contains(results, r => r.State == expectedResult && r.Message == resultMessage);
    }
  }
}
