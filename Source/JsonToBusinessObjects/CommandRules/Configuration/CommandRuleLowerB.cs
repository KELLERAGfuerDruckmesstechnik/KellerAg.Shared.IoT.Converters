namespace JsonToBusinessObjects.CommandRules.Configuration
{
    using System;
    using DataContainers;
    using DataContainers.Configuration;
    using Infrastructure;
    using Infrastructure.Logging;
    using Newtonsoft.Json.Linq;

    internal class CommandRuleLowerB : CommandRuleBase, ICommandRule
    {
        private class CommandModificationLowerB : ICommandModification
        {
            private readonly TextEmailSmsLocSettings _textEmailSmsLocSettings;

            public CommandModificationLowerB(TextEmailSmsLocSettings textEmailSmsLocSettings)
            {
                this._textEmailSmsLocSettings = textEmailSmsLocSettings;
            }

            public void ApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                if (this.CanApplyToBusinessObjectRoot(businessObjectRoot) == false)
                {
                    throw new InvalidOperationException($"Cannot apply command rule {nameof(CommandModificationLowerB)} to business object.");
                }

                if (businessObjectRoot.Configuration == null)
                {
                    businessObjectRoot.Configuration = new ConfigurationContainer();
                }

                businessObjectRoot.Configuration.TextEmailSmsLocSettings = this._textEmailSmsLocSettings;
            }

            public bool CanApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                return businessObjectRoot.Configuration?.TextEmailSmsLocSettings == null;
            }
        }

        public CommandRuleLowerB(ILogger logger) : base(logger)
        {
        }

        public char HandledCommandCharacter => 'b';

        public ICommandModification CreateModificationObject(JToken variables)
        {
            TextEmailSmsLocSettings textEmailSmsLocSettings = new TextEmailSmsLocSettings();

            variables.ExecuteIfAvailable("a", (string value) => textEmailSmsLocSettings.EmailAddress1 = value, this.Logger);
            variables.ExecuteIfAvailable("b", (string value) => textEmailSmsLocSettings.EmailAddress2 = value, this.Logger);
            variables.ExecuteIfAvailable("c", (string value) => textEmailSmsLocSettings.EmailAddress3 = value, this.Logger);

            variables.ExecuteIfAvailable("g", (string value) => textEmailSmsLocSettings.PasswordForQuerySms = value, this.Logger);

            variables.ExecuteIfAvailable("j", (string value) => textEmailSmsLocSettings.SimPin = value, this.Logger);
            variables.ExecuteIfAvailable("k", (string value) => textEmailSmsLocSettings.RecallForDataConnection = value, this.Logger);

            variables.ExecuteIfAvailable("m", (string value) => textEmailSmsLocSettings.SmsNumber1Measure = value, this.Logger);
            variables.ExecuteIfAvailable("n", (string value) => textEmailSmsLocSettings.SmsNumber2Alarm = value, this.Logger);
            variables.ExecuteIfAvailable("o", (string value) => textEmailSmsLocSettings.SmsNumber3Info = value, this.Logger);

            variables.ExecuteIfAvailable("q", (string value) => textEmailSmsLocSettings.SmsServiceCenterNr = value, this.Logger);
            variables.ExecuteIfAvailable("r", (string value) => textEmailSmsLocSettings.NetworkName = value, this.Logger);
            variables.ExecuteIfAvailable("s", (string value) => textEmailSmsLocSettings.OwnTelNumber = value, this.Logger);
            variables.ExecuteIfAvailable("t", (string value) => textEmailSmsLocSettings.LocationName = value, this.Logger);
            variables.ExecuteIfAvailable("u", (string value) => textEmailSmsLocSettings.SmsText1Measure = value, this.Logger);
            variables.ExecuteIfAvailable("v", (string value) => textEmailSmsLocSettings.SmsText2Alarm = value, this.Logger);
            variables.ExecuteIfAvailable("w", (string value) => textEmailSmsLocSettings.SmsText3AnswerCheck = value, this.Logger);

            variables.ExecuteIfAvailable("0", (string value) => textEmailSmsLocSettings.LongitudeText = value, this.Logger);
            variables.ExecuteIfAvailable("1", (string value) => textEmailSmsLocSettings.LatitudeText = value, this.Logger);
            variables.ExecuteIfAvailable("2", (string value) => textEmailSmsLocSettings.AltitudeText = value, this.Logger);
            variables.ExecuteIfAvailable("3", (string value) => textEmailSmsLocSettings.CellLocateLongitude = value, this.Logger);
            variables.ExecuteIfAvailable("4", (string value) => textEmailSmsLocSettings.CellLocateLatitude = value, this.Logger);
            variables.ExecuteIfAvailable("5", (string value) => textEmailSmsLocSettings.CellLocateAltitude = value, this.Logger);

            return new CommandModificationLowerB(textEmailSmsLocSettings);
        }
    }
}