namespace JsonToBusinessObjects.CommandRules.Configuration
{
    using System;
    using DataContainers;
    using DataContainers.Configuration;
    using Infrastructure;
    using Infrastructure.Logging;
    using Newtonsoft.Json.Linq;

    internal class CommandRuleLowerF : CommandRuleBase, ICommandRule
    {
        private class CommandModificationLowerF : ICommandModification
        {
            private readonly MeasurementSettings2 _measurementSettings2;

            public CommandModificationLowerF(MeasurementSettings2 measurementSettings2)
            {
                this._measurementSettings2 = measurementSettings2;
            }

            public void ApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                if (this.CanApplyToBusinessObjectRoot(businessObjectRoot) == false)
                {
                    throw new InvalidOperationException($"Cannot apply command rule {nameof(CommandModificationLowerF)} to business object.");
                }

                if (businessObjectRoot.Configuration == null)
                {
                    businessObjectRoot.Configuration = new ConfigurationContainer();
                }

                businessObjectRoot.Configuration.MeasurementSettings2 = this._measurementSettings2;
            }

            public bool CanApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                return businessObjectRoot.Configuration?.MeasurementSettings2 == null;
            }
        }

        public CommandRuleLowerF(ILogger logger) : base(logger)
        {
        }

        public char HandledCommandCharacter => 'f';

        public ICommandModification CreateModificationObject(JToken variables)
        {
            MeasurementSettings2 measurementSettings2 = new MeasurementSettings2();

            variables.ExecuteIfAvailable("a", (uint value) => measurementSettings2.TimerEvent = value, this.Logger);
            variables.ExecuteIfAvailable("g", (uint value) => measurementSettings2.IntervalEventCheck = value, this.Logger);
            variables.ExecuteIfAvailable("h", (uint value) => measurementSettings2.IntervalEventMeasure = value, this.Logger);
            variables.ExecuteIfAvailable("m", (byte value) => measurementSettings2.EventChannel = value, this.Logger);
            variables.ExecuteIfAvailable("n", (byte value) => measurementSettings2.EventType = value, this.Logger);

            variables.ExecuteIfAvailable("o", (byte value) => measurementSettings2.SendAfterYFilesWithRecordData = value, this.Logger);
            variables.ExecuteIfAvailable("q", (byte value) => measurementSettings2.SendToFtpAfterXCollectedMeasurements = value, this.Logger);
            variables.ExecuteIfAvailable("z", (byte value) => measurementSettings2.SendTypeFtp = value, this.Logger);
            variables.ExecuteIfAvailable("3", (byte value) => measurementSettings2.HardwarePreOnTimeInSeconds = value, this.Logger);
            
            return new CommandModificationLowerF(measurementSettings2);
        }
    }
}