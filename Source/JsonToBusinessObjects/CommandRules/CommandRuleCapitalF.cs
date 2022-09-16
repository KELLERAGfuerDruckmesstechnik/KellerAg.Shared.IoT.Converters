namespace JsonToBusinessObjects.CommandRules
{
    using System;
    using Conversion;
    using DataContainers;
    using Infrastructure;
    using Infrastructure.Logging;
    using Newtonsoft.Json.Linq;

    internal class CommandRuleCapitalF : CommandRuleBase, ICommandRule
    {
        private class CommandModificationCapitalF : ICommandModification
        {
            private readonly MessageType _messageType;

            public CommandModificationCapitalF(MessageType messageType)
            {
                this._messageType = messageType;
            }

            public void ApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                if (this.CanApplyToBusinessObjectRoot(businessObjectRoot) == false)
                {
                    throw new InvalidOperationException($"Cannot apply command rule {nameof(CommandModificationCapitalF)} to business object.");
                }

                businessObjectRoot.MessageType = this._messageType;
            }

            public bool CanApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                return businessObjectRoot.MessageType == MessageType.UnknownMessage;
            }
        }

        public CommandRuleCapitalF(ILogger logger) : base(logger)
        {
        }

        public char HandledCommandCharacter => 'F';

        public ICommandModification CreateModificationObject(JToken variables)
        {
            MessageType messageType = MessageType.UnknownMessage;

            variables.ExecuteIfAvailable("a", (string _) => messageType = MessageType.MeasurementMessage, this.Logger);
            variables.ExecuteIfAvailable("b", (string _) => messageType = MessageType.ConfigurationMessageWithoutAck, this.Logger);
            variables.ExecuteIfAvailable("c", (string _) => messageType = MessageType.AlarmMessage, this.Logger);
            variables.ExecuteIfAvailable("d", (string _) => messageType = MessageType.ConfigurationMessageWithAck, this.Logger);
            variables.ExecuteIfAvailable("e", (string _) => messageType = MessageType.RecordDataMessage, this.Logger);
            variables.ExecuteIfAvailable("f", (string _) => messageType = MessageType.RequestedRecordDataMessage, this.Logger);
            variables.ExecuteIfAvailable("g", (string _) => messageType = MessageType.ErrorReportMessage, this.Logger);

            return new CommandModificationCapitalF(messageType);
        }
    }
}