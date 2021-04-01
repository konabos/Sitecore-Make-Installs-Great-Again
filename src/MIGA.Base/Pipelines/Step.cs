﻿using MIGA.Pipelines.Processors;

namespace MIGA.Pipelines
{
  #region

  using System.Collections.Generic;
  using MIGA.Pipelines.Processors;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;
  using MIGA.Extensions;

  #endregion

  public class Step
  {
    #region Fields

    [CanBeNull]
    public string ArgsName { get; }

    [NotNull]
    public readonly List<Processor> _Processors;

    #endregion

    #region Constructors

    public Step([NotNull] List<Processor> processors, [CanBeNull] string argsName)
    {
      Assert.ArgumentNotNull(processors, nameof(processors));

      _Processors = processors;
      ArgsName = argsName;
    }

    #endregion

    #region Public Methods

    [NotNull]
    public static List<Step> CreateSteps([NotNull] List<StepDefinition> stepDefinitions, [NotNull] ProcessorArgs args, [CanBeNull] IPipelineController controller = null)
    {
      Assert.ArgumentNotNull(stepDefinitions, nameof(stepDefinitions));
      Assert.ArgumentNotNull(args, nameof(args));

      return new List<Step>(CreateStepsPrivate(stepDefinitions, args, controller));
    }

    #endregion

    #region Methods

    [NotNull]
    private static IEnumerable<Step> CreateStepsPrivate([NotNull] IEnumerable<StepDefinition> steps, [NotNull] ProcessorArgs args, [CanBeNull] IPipelineController controller = null)
    {
      Assert.ArgumentNotNull(steps, nameof(steps));
      Assert.ArgumentNotNull(args, nameof(args));

      foreach (StepDefinition stepDefinition in steps)
      {
        var argsName = stepDefinition.ArgsName.EmptyToNull();
        Step step = new Step(ProcessorManager.CreateProcessors(stepDefinition._ProcessorDefinitions, args, controller), argsName);
        Assert.IsNotNull(step, "Can't instantiate step");
        yield return step;
      }
    }

    #endregion
  }
}