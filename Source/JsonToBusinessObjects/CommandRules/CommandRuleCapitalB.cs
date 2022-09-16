namespace JsonToBusinessObjects.CommandRules
{
    using System;
    using DataContainers;
    using Infrastructure;
    using Infrastructure.Logging;
    using Newtonsoft.Json.Linq;
    using JsonToBusinessObjects.Conversion;
    using JsonToBusinessObjects.Conversion.GsmArc;

    internal class CommandRuleCapitalB : CommandRuleBase, ICommandRule
    {
        private readonly MeasurementDataDecoder _decoder = new MeasurementDataDecoder();

        private class CommandModificationCapitalB : ICommandModification
        {
            private readonly ChannelDataStorage _channelDataStorage;

            public CommandModificationCapitalB(ChannelDataStorage channelDataStorage)
            {
                this._channelDataStorage = channelDataStorage;
            }

            public void ApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                if (this.CanApplyToBusinessObjectRoot(businessObjectRoot) == false)
                {
                    throw new InvalidOperationException($"Cannot apply command rule {nameof(CommandModificationCapitalB)} to business object.");
                }

                businessObjectRoot.Measurements = this._channelDataStorage;
            }

            public bool CanApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                return businessObjectRoot.Measurements == null;
            }
        }

        public CommandRuleCapitalB(ILogger logger) : base(logger)
        {
        }

        public char HandledCommandCharacter => 'B';

        public ICommandModification CreateModificationObject(JToken variables)
        {
            string base64EncodedContent = null;
            variables.ExecuteIfAvailable("a", (string value) => base64EncodedContent = value, this.Logger);

            if (string.IsNullOrEmpty(base64EncodedContent))
            {
                throw new InvalidOperationException("The '#B' command contained no data in the 'a' parameter");
            }

            byte[] data = Convert.FromBase64String(base64EncodedContent);

            ChannelDataStorage channelDataStorage = this._decoder.Decode(data);

            return new CommandModificationCapitalB(channelDataStorage);
        }
    }
}