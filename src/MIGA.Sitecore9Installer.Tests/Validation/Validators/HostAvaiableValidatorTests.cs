using AutoFixture.Xunit2;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using MIGA.Sitecore9Installer.Tasks;
using MIGA.Sitecore9Installer.Validation;
using MIGA.Sitecore9Installer.Validation.Validators;
using Xunit;

namespace MIGA.Sitecore9Installer.Tests.Validation.Validators
{
  public class HostAvaiableValidatorTests
  {
    private HostAvaiableValidator CreateValidator(HttpStatusCode returnStatus, string paramNames)
    {
      HostAvaiableValidator val = Substitute.ForPartsOf<HostAvaiableValidator>();
      val.WhenForAnyArgs(v => v.GetResponse(null)).DoNotCallBase();
      HttpWebResponse resp = Substitute.For<HttpWebResponse>();
      resp.StatusCode.Returns(returnStatus);
      val.GetResponse(null).ReturnsForAnyArgs(resp);
      val.Data["ParamNames"] = paramNames;
      return val;
    }

    [Theory]
    [AutoData]
    public void SolrAvailable(string hostName, string hostValue)
    {
      HostAvaiableValidator val = this.CreateValidator(HttpStatusCode.OK, hostName);
      Task task = ValidatorTestSetup.CreateTask("someTask", new string[] { hostName }, new string[] { hostValue });
      IEnumerable<ValidationResult> results= val.Evaluate(new Task[] { task });
      Assert.Contains(results, r => r.State == ValidatorState.Success);
      Assert.DoesNotContain(results, r => r.State == ValidatorState.Error);
    }

    [Theory]
    [AutoData]
    public void SolrNotAvailable(string hostName, string hostValue)
    {
      HostAvaiableValidator val = this.CreateValidator(HttpStatusCode.NotFound, hostName);
      Task task = ValidatorTestSetup.CreateTask("someTask", new string[] { hostName }, new string[] { hostValue });
      IEnumerable<ValidationResult> results = val.Evaluate(new Task[] { task });
      Assert.Contains(results, r => r.State == ValidatorState.Error);
      Assert.DoesNotContain(results, r => r.State == ValidatorState.Success);
    }
  }
}
