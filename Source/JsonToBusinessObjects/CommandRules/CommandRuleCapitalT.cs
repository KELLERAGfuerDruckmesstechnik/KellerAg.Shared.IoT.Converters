namespace JsonToBusinessObjects.CommandRules
{
    using System;
    using DataContainers;
    using Infrastructure;
    using Infrastructure.Logging;
    using Newtonsoft.Json.Linq;

    internal class CommandRuleCapitalT : CommandRuleBase, ICommandRule
    {
        private class CommandModificationCapitalT : ICommandModification
        {
            private readonly TimeContainer _timeContainer;

            public CommandModificationCapitalT(TimeContainer timeContainer)
            {
                this._timeContainer = timeContainer;
            }

            public void ApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                if (this.CanApplyToBusinessObjectRoot(businessObjectRoot) == false)
                {
                    throw new InvalidOperationException($"Cannot apply command rule {nameof(CommandModificationCapitalT)} to business object.");
                }

                businessObjectRoot.Time = this._timeContainer;
            }

            public bool CanApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                return businessObjectRoot.Time == null;
            }
        }

        public CommandRuleCapitalT(ILogger logger) : base(logger)
        {
        }

        public char HandledCommandCharacter => 'T';

        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="year" /> is less than 1 or greater than 9999.-or- <paramref name="month" /> is less than 1 or greater than 12.-or- <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />.</exception>
        public ICommandModification CreateModificationObject(JToken variables)
        {
            var timeContainer = new TimeContainer();

            variables.ExecuteIfAvailable("s", (uint value) => timeContainer.GsmTime = new DateTime(2000,1,1) + TimeSpan.FromSeconds(value), this.Logger);
            variables.ExecuteIfAvailable("m", (uint value) => timeContainer.TimeWhenLastMeasurementWasStored = new DateTime(2000, 1, 1) + TimeSpan.FromSeconds(value), this.Logger);

            return new CommandModificationCapitalT(timeContainer);
        }
    }
}