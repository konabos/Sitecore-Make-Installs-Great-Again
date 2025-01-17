﻿using System;
using System.Collections.Generic;
using System.Linq;
using MIGA.Sitecore9Installer.Tasks;

namespace MIGA.Sitecore9Installer.Validation.Validators
{
  public class CmDdsPatchSiteNameValidator : IValidator
  {
    protected virtual string DdsPatchValidationResultMessage =>
      "Value of the '{0}' parameter differs in the '{1}' and '{2}' tasks. To fix: set the same value for each mentioned task in Advanced installation parameters.";

    public Dictionary<string, string> Data { get; set; }

    public virtual string SuccessMessage => "'SiteName' value was set properly for 'sitecore-xp1-cm' and 'sitecore-XP1-cm-dds-patch' installation tasks.";

    public CmDdsPatchSiteNameValidator()
    {
      this.Data = new Dictionary<string, string>();
    }

    public IEnumerable<ValidationResult> Evaluate(IEnumerable<Task> tasks)
    {
      string sitecoreXp1Cm = this.Data["SitecoreXp1Cm"];
      string sitecoreXp1CmDdsPatch = this.Data["SitecoreXp1CmDdsPatch"];
      string siteName = this.Data["SiteName"];
      bool errors = false;
      Task cmTask = tasks.FirstOrDefault(t => t.Name.Equals(sitecoreXp1Cm, StringComparison.InvariantCultureIgnoreCase) &&
                                      t.LocalParams.Any(p => p.Name.Equals(siteName, StringComparison.InvariantCultureIgnoreCase)));

      if (cmTask != null)
      {
        Task ddsPatchTask = tasks.FirstOrDefault(t => t.Name.Equals(sitecoreXp1CmDdsPatch, StringComparison.InvariantCultureIgnoreCase) &&
                                              t.LocalParams.Any(p => p.Name.Equals(siteName, StringComparison.InvariantCultureIgnoreCase)));

        if (ddsPatchTask != null)
        {
          string cmSiteName = cmTask.LocalParams.Single(p => p.Name.Equals(siteName, StringComparison.InvariantCultureIgnoreCase)).Value;
          string ddsPatchSiteName = ddsPatchTask.LocalParams.Single(p => p.Name.Equals(siteName, StringComparison.InvariantCultureIgnoreCase)).Value;

          if (!cmSiteName.Equals(ddsPatchSiteName, StringComparison.InvariantCultureIgnoreCase))
          {
            errors = true;
            yield return new ValidationResult(ValidatorState.Error,
              string.Format(DdsPatchValidationResultMessage, siteName, cmTask.Name, ddsPatchTask.Name), null);
          }
        }
      }

      if (!errors)
      {
        yield return new ValidationResult(ValidatorState.Success, this.SuccessMessage, null);
      }
    }
  }
}
