namespace JsonToBusinessObjects.CommandRules.Configuration
{
    using System;
    using DataContainers;
    using DataContainers.Configuration;
    using Infrastructure;
    using Infrastructure.Logging;
    using Newtonsoft.Json.Linq;

    internal class CommandRuleLowerK : CommandRuleBase, ICommandRule
    {
        private class CommandModificationLowerK : ICommandModification
        {
            private readonly FtpSettings _ftpSettings;

            public CommandModificationLowerK(FtpSettings ftpSettings)
            {
                this._ftpSettings = ftpSettings;
            }

            public void ApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                if (this.CanApplyToBusinessObjectRoot(businessObjectRoot) == false)
                {
                    throw new InvalidOperationException($"Cannot apply command rule {nameof(CommandModificationLowerK)} to business object.");
                }

                if (businessObjectRoot.Configuration == null)
                {
                    businessObjectRoot.Configuration = new ConfigurationContainer();
                }

                businessObjectRoot.Configuration.FtpSettings = this._ftpSettings;
            }

            public bool CanApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                return businessObjectRoot.Configuration?.FtpSettings == null;
            }
        }

        public CommandRuleLowerK(ILogger logger) : base(logger)
        {
        }

        public char HandledCommandCharacter => 'k';

        public ICommandModification CreateModificationObject(JToken variables)
        {
            var ftpSettings = new FtpSettings();

            variables.ExecuteIfAvailable("a", (string value) => ftpSettings.FtpServerName             = value, this.Logger);
            variables.ExecuteIfAvailable("b", (string value) => ftpSettings.FtpLoginUserName          = value, this.Logger);
            variables.ExecuteIfAvailable("c", (string value) => ftpSettings.FtpPassword               = value, this.Logger);
            variables.ExecuteIfAvailable("d", (string value) => ftpSettings.FtpAccount                = value, this.Logger);
            variables.ExecuteIfAvailable("e", (string value) => ftpSettings.FtpSourceControlPort      = value, this.Logger);
            variables.ExecuteIfAvailable("f", (string value) => ftpSettings.FtpDestinationControlPort = value, this.Logger);
            variables.ExecuteIfAvailable("g", (string value) => ftpSettings.FtpSourceDataPort         = value, this.Logger);
            variables.ExecuteIfAvailable("h", (string value) => ftpSettings.FtpFilePath               = value, this.Logger);

            return new CommandModificationLowerK(ftpSettings);
        }
    }
}