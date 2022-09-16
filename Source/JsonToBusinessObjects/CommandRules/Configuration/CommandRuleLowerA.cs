namespace JsonToBusinessObjects.CommandRules.Configuration
{
    using System;
    using DataContainers;
    using DataContainers.Configuration;
    using Infrastructure;
    using Infrastructure.Logging;
    using Newtonsoft.Json.Linq;

    internal class CommandRuleLowerA : CommandRuleBase, ICommandRule
    {
        private class CommandModificationLowerA : ICommandModification
        {
            private readonly GprsSettings _gprsSettings;

            public CommandModificationLowerA(GprsSettings gprsSettings)
            {
                this._gprsSettings = gprsSettings;
            }

            public void ApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                if (this.CanApplyToBusinessObjectRoot(businessObjectRoot) == false)
                {
                    throw new InvalidOperationException($"Cannot apply command rule {nameof(CommandModificationLowerA)} to business object.");
                }

                if (businessObjectRoot.Configuration == null)
                {
                    businessObjectRoot.Configuration = new ConfigurationContainer();
                }

                businessObjectRoot.Configuration.GprsSettings = this._gprsSettings;
            }

            public bool CanApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                return businessObjectRoot.Configuration?.GprsSettings == null;
            }
        }

        public CommandRuleLowerA(ILogger logger) : base(logger)
        {
        }

        public char HandledCommandCharacter => 'a';

        public ICommandModification CreateModificationObject(JToken variables)
        {
            GprsSettings gprsSettings = new GprsSettings();

            variables.ExecuteIfAvailable("a", (string value) => gprsSettings.GprsAPN = value, this.Logger);
            variables.ExecuteIfAvailable("b", (string value) => gprsSettings.GprsID = value, this.Logger);
            variables.ExecuteIfAvailable("c", (string value) => gprsSettings.GprsPassword = value, this.Logger);
            variables.ExecuteIfAvailable("d", (string value) => gprsSettings.GprsDNS = value, this.Logger);
            variables.ExecuteIfAvailable("e", (string value) => gprsSettings.SmtpShowedName = value, this.Logger);
            variables.ExecuteIfAvailable("f", (string value) => gprsSettings.PopUsername = value, this.Logger);
            variables.ExecuteIfAvailable("g", (string value) => gprsSettings.PopPassword = value, this.Logger);
            variables.ExecuteIfAvailable("h", (string value) => gprsSettings.OptSmtpUsername = value, this.Logger);
            variables.ExecuteIfAvailable("i", (string value) => gprsSettings.OptSmtpPassword = value, this.Logger);
            variables.ExecuteIfAvailable("j", (string value) => gprsSettings.Pop3Server = value, this.Logger);
            variables.ExecuteIfAvailable("k", (string value) => gprsSettings.Pop3Port = value, this.Logger);
            variables.ExecuteIfAvailable("l", (string value) => gprsSettings.SmtpServer = value, this.Logger);
            variables.ExecuteIfAvailable("m", (string value) => gprsSettings.SmtpPort = value, this.Logger);
            variables.ExecuteIfAvailable("n", (string value) => gprsSettings.ReturnAddress = value, this.Logger);

            // Read-Only
            variables.ExecuteIfAvailable("o", (string value) => gprsSettings.CellularModuleId               = value, this.Logger);
            variables.ExecuteIfAvailable("p", (string value) => gprsSettings.CellularModuleRevisionId       = value, this.Logger);
            variables.ExecuteIfAvailable("q", (string value) => gprsSettings.CellularModuleSerialNumberIMEI = value, this.Logger);
            variables.ExecuteIfAvailable("r", (string value) => gprsSettings.CellularSIMCardId              = value, this.Logger);
            variables.ExecuteIfAvailable("s", (string value) => gprsSettings.CellularSIMCardSubscriberId    = value, this.Logger);

            return new CommandModificationLowerA(gprsSettings);
        }
    }
}