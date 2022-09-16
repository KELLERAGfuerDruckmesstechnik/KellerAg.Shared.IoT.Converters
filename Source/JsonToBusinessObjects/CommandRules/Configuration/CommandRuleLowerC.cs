namespace JsonToBusinessObjects.CommandRules.Configuration
{
    using System;
    using DataContainers;
    using DataContainers.Configuration;
    using Infrastructure;
    using Infrastructure.Logging;
    using Newtonsoft.Json.Linq;

    internal class CommandRuleLowerC : CommandRuleBase, ICommandRule
    {
        private class CommandModificationLowerC : ICommandModification
        {
            private readonly MeasurementSettings _measurementSettings;

            public CommandModificationLowerC(MeasurementSettings measurementSettings)
            {
                this._measurementSettings = measurementSettings;
            }

            public void ApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                if (this.CanApplyToBusinessObjectRoot(businessObjectRoot) == false)
                {
                    throw new InvalidOperationException($"Cannot apply command rule {nameof(CommandModificationLowerC)} to business object.");
                }

                if (businessObjectRoot.Configuration == null)
                {
                    businessObjectRoot.Configuration = new ConfigurationContainer();
                }

                businessObjectRoot.Configuration.MeasurementSettings = this._measurementSettings;
            }

            public bool CanApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                return businessObjectRoot.Configuration?.MeasurementSettings == null;
            }
        }

        public CommandRuleLowerC(ILogger logger) : base(logger)
        {
        }

        public char HandledCommandCharacter => 'c';

        public ICommandModification CreateModificationObject(JToken variables)
        {
            MeasurementSettings measurementSettings = new MeasurementSettings();

            variables.ExecuteIfAvailable("a", (uint value) => measurementSettings.Timer0Measure = value, this.Logger);
            variables.ExecuteIfAvailable("b", (uint value) => measurementSettings.Timer1Alarm = value, this.Logger);
            variables.ExecuteIfAvailable("c", (uint value) => measurementSettings.Timer2Info = value, this.Logger);
            variables.ExecuteIfAvailable("d", (uint value) => measurementSettings.Timer3Check = value, this.Logger);
            variables.ExecuteIfAvailable("e", (uint value) => measurementSettings.Timer4DataConnection = value, this.Logger);

            variables.ExecuteIfAvailable("g", (uint value) => measurementSettings.Interval0Measure = value, this.Logger);
            variables.ExecuteIfAvailable("h", (uint value) => measurementSettings.Interval1Alarm = value, this.Logger);
            variables.ExecuteIfAvailable("i", (uint value) => measurementSettings.Interval2Info = value, this.Logger);
            variables.ExecuteIfAvailable("j", (uint value) => measurementSettings.Interval3Check = value, this.Logger);
            variables.ExecuteIfAvailable("k", (uint value) => measurementSettings.Interval4DataConnection = value, this.Logger);

            variables.ExecuteIfAvailable("m", (byte value) => measurementSettings.MeasureAndSaveCH0_7 = value, this.Logger);

            variables.ExecuteIfAvailable("o", (byte value) => measurementSettings.SendSmsAfterXMeasurements = value, this.Logger);
            variables.ExecuteIfAvailable("p", (byte value) => measurementSettings.MeasureAndSaveCH8_15 = value, this.Logger);
            variables.ExecuteIfAvailable("q", (byte value) => measurementSettings.SendMailAfterXMeasurements = value, this.Logger);
            variables.ExecuteIfAvailable("r", (byte value) => measurementSettings.AlarmChannelNumber = value, this.Logger);
            variables.ExecuteIfAvailable("s", (byte value) => measurementSettings.AlarmType = value, this.Logger);
            variables.ExecuteIfAvailable("t", (byte value) => measurementSettings.SendAlarmXTimes = value, this.Logger);
            
            variables.ExecuteIfAvailable("v", (byte value) => measurementSettings.ResolutionPressureChannels = value, this.Logger);
            variables.ExecuteIfAvailable("w", (byte value) => measurementSettings.ResolutionTemperatureChannels = value, this.Logger);
            variables.ExecuteIfAvailable("x", (byte value) => measurementSettings.LockTimerOnlyCheck = value, this.Logger); //Will not be used as this will be sent down to the ARC
            variables.ExecuteIfAvailable("y", (byte value) => measurementSettings.LockTimersWithoutCheck = value, this.Logger);
            variables.ExecuteIfAvailable("z", (byte value) => measurementSettings.SendSmsEmail = value, this.Logger);
            variables.ExecuteIfAvailable("0", (byte value) => measurementSettings.ModemProtocol = value, this.Logger);
            variables.ExecuteIfAvailable("1", (byte value) => measurementSettings.AccountSetting = value, this.Logger);
            variables.ExecuteIfAvailable("2", (byte value) => measurementSettings.ServerConfig = value, this.Logger);
            variables.ExecuteIfAvailable("3", (byte value) => measurementSettings.OflFormType = value, this.Logger);
            variables.ExecuteIfAvailable("4", (byte value) => measurementSettings.PowerExternalDevice = value, this.Logger);
            variables.ExecuteIfAvailable("5", (byte value) => measurementSettings.SupportedConnectionTypes = value, this.Logger);
            variables.ExecuteIfAvailable("6", (byte value) => measurementSettings.ConnectionType = value, this.Logger);
            variables.ExecuteIfAvailable("7", (byte value) => measurementSettings.ConfigBytes0 = value, this.Logger);
            variables.ExecuteIfAvailable("8", (byte value) => measurementSettings.CalcChannels = value, this.Logger);
            variables.ExecuteIfAvailable("9", (byte value) => measurementSettings.CalcConversionTo = value, this.Logger);
            variables.ExecuteIfAvailable("A", (byte value) => measurementSettings.SslTlsFtpEnable = value, this.Logger);
            variables.ExecuteIfAvailable("B", (byte value) => measurementSettings.FtpMode = value, this.Logger);

            return new CommandModificationLowerC(measurementSettings);
        }
    }
}