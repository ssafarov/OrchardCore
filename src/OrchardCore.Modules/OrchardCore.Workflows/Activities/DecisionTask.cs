using System.Collections.Generic;
using Microsoft.Extensions.Localization;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;

namespace OrchardCore.Workflows.Activities
{
    // TODO: Turns this into a more advanced activity like the one in Orchard 1, allowing custom outcomes and a script that yields one or more outcomes.
    public class DecisionTask : TaskActivity
    {
        public DecisionTask(IStringLocalizer<DecisionTask> localizer)
        {
            T = localizer;
        }

        private IStringLocalizer T { get; }

        public override string Name => nameof(DecisionTask);
        public override LocalizedString Category => T["Control Flow"];
        public override LocalizedString Description => T["Evaluates the specified condition and continues execution based on the outcome."];

        /// <summary>
        /// An expression evaluating to either true or false.
        /// </summary>
        public WorkflowExpression<bool> ConditionExpression
        {
            get => GetProperty(() => new WorkflowExpression<bool>());
            set => SetProperty(value);
        }

        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            return Outcomes(T["True"], T["False"]);
        }

        public override IEnumerable<string> Execute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            var result = workflowContext.Evaluate(ConditionExpression);
            yield return result ? "True" : "False";
        }
    }
}