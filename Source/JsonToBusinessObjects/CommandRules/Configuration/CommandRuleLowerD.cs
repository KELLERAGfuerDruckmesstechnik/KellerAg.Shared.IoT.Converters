namespace JsonToBusinessObjects.CommandRules.Configuration
{
    using System;
    using DataContainers;
    using DataContainers.Configuration;
    using Infrastructure;
    using Infrastructure.Logging;
    using Newtonsoft.Json.Linq;

    internal class CommandRuleLowerD : CommandRuleBase, ICommandRule
    {
        private class CommandModificationLowerD : ICommandModification
        {
            private readonly FloatingPointMeasurementSettings _floatingPointMeasurementSettings;

            public CommandModificationLowerD(FloatingPointMeasurementSettings floatingPointMeasurementSettings)
            {
                this._floatingPointMeasurementSettings = floatingPointMeasurementSettings;
            }

            public void ApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                if (this.CanApplyToBusinessObjectRoot(businessObjectRoot) == false)
                {
                    throw new InvalidOperationException($"Cannot apply command rule {nameof(CommandModificationLowerD)} to business object.");
                }

                if (businessObjectRoot.Configuration == null)
                {
                    businessObjectRoot.Configuration = new ConfigurationContainer();
                }

                businessObjectRoot.Configuration.FloatingPointMeasurementSettings = this._floatingPointMeasurementSettings;
            }

            public bool CanApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                return businessObjectRoot.Configuration?.FloatingPointMeasurementSettings == null;
            }
        }

        public CommandRuleLowerD(ILogger logger) : base(logger)
        {
        }

        public char HandledCommandCharacter => 'd';

        public ICommandModification CreateModificationObject(JToken variables)
        {
            FloatingPointMeasurementSettings floatingPointMeasurementSettings = new FloatingPointMeasurementSettings();

            variables.ExecuteIfAvailable("a", (float value) => floatingPointMeasurementSettings.AlarmOn = value, this.Logger);
            variables.ExecuteIfAvailable("b", (float value) => floatingPointMeasurementSettings.AlarmOff = value, this.Logger);
            variables.ExecuteIfAvailable("c", (float value) => floatingPointMeasurementSettings.AlarmDelta = value, this.Logger);

            variables.ExecuteIfAvailable("f", (float value) => floatingPointMeasurementSettings.MultiplierTemperatureChannels = value, this.Logger);
            variables.ExecuteIfAvailable("g", (float value) => floatingPointMeasurementSettings.MultiplierPressureChannels = value, this.Logger);

            variables.ExecuteIfAvailable("i", (float value) => floatingPointMeasurementSettings.Val1OnEventLogging = value, this.Logger);
            variables.ExecuteIfAvailable("j", (float value) => floatingPointMeasurementSettings.Val2OffEventLogging = value, this.Logger);
            variables.ExecuteIfAvailable("k", (float value) => floatingPointMeasurementSettings.Val3DeltaEventLogging = value, this.Logger);

            variables.ExecuteIfAvailable("m", (float value) => floatingPointMeasurementSettings.Val100WlcEnabled = value, this.Logger);
            variables.ExecuteIfAvailable("n", (float value) => floatingPointMeasurementSettings.Val101WlcLength = value, this.Logger);
            variables.ExecuteIfAvailable("o", (float value) => floatingPointMeasurementSettings.Val102WlcHeight = value, this.Logger);
            variables.ExecuteIfAvailable("p", (float value) => floatingPointMeasurementSettings.Val103CalcOffset = value, this.Logger);
            variables.ExecuteIfAvailable("q", (float value) => floatingPointMeasurementSettings.Val104WlcDensity = value, this.Logger);
            variables.ExecuteIfAvailable("r", (float value) => floatingPointMeasurementSettings.Val105OflWidth = value, this.Logger);
            variables.ExecuteIfAvailable("s", (float value) => floatingPointMeasurementSettings.Val106OflAngle = value, this.Logger);
            variables.ExecuteIfAvailable("t", (float value) => floatingPointMeasurementSettings.Val107OflFormFactor = value, this.Logger);
            variables.ExecuteIfAvailable("u", (float value) => floatingPointMeasurementSettings.Val108OflMinCalc = value, this.Logger);
            variables.ExecuteIfAvailable("v", (float value) => floatingPointMeasurementSettings.Val109Fu3031Index19 = value, this.Logger);
            variables.ExecuteIfAvailable("w", (float value) => floatingPointMeasurementSettings.Val110Fu3031Index20 = value, this.Logger);

            variables.ExecuteIfAvailable("0", (float value) => floatingPointMeasurementSettings.LongitudeFu3031Index24 = value, this.Logger);
            variables.ExecuteIfAvailable("1", (float value) => floatingPointMeasurementSettings.LatitudeFu3031Index45 = value, this.Logger);
            variables.ExecuteIfAvailable("2", (float value) => floatingPointMeasurementSettings.AltitudeFu3031Index26 = value, this.Logger);

            return new CommandModificationLowerD(floatingPointMeasurementSettings);
        }
    }
}