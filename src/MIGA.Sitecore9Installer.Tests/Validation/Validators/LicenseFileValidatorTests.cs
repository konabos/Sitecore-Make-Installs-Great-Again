using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIGA.Sitecore9Installer.Tasks;
using MIGA.Sitecore9Installer.Validation;
using MIGA.Sitecore9Installer.Validation.Validators;
using Xunit;

namespace MIGA.Sitecore9Installer.Tests.Validation.Validators
{
  public class LicenseFileValidatorTests
  {
    [Theory]
    [InlineData(true, ValidatorState.Success)]
    [InlineData(false, ValidatorState.Error)]
    public void ReturnsValidValidationResults(bool fileExists, ValidatorState expectedResult)
    {
      //Arrange
      Task task = Substitute.For<Task>("", 0, null, null, new Dictionary<string, string>());
      GlobalParameters globals = new GlobalParameters();
      task.LocalParams.Returns(new LocalParameters(new List<InstallParam>(),globals));

      task.LocalParams.AddOrUpdateParam("LicenseFile", @"C:\license.xml",InstallParamType.String);

      List<Task> tasks = Substitute.For<List<Task>>();
      tasks.Add(task);

      LicenseFileValidator val = Substitute.ForPartsOf<LicenseFileValidator>();
      val.FileExists(string.Empty).ReturnsForAnyArgs(fileExists);

      val.Data["LicenseFileVariable"] = "LicenseFile";

      //Act
      IEnumerable<ValidationResult> results = val.Evaluate(tasks);

      //Assert
      Assert.Contains(results, r => r.State == expectedResult);
    }
  }
}
