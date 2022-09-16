namespace JsonToBusinessObjects.CommandRules
{
    using DataContainers;
    using Infrastructure;
    using Infrastructure.Logging;
    using Newtonsoft.Json.Linq;

    internal class CommandRuleCapitalO : CommandRuleBase, ICommandRule
    {
        private class CommandModificationCapitalO : ICommandModification
        {
            private readonly int? _ackNumber;

            public CommandModificationCapitalO(int? ackNumber)
            {
                this._ackNumber = ackNumber;
            }

            /// <inheritdoc />
            public bool CanApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                return true;
            }

            public void ApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                businessObjectRoot.AckNumber = this._ackNumber;
            }
        }

        public CommandRuleCapitalO(ILogger logger) : base(logger)
        {
        }

        public char HandledCommandCharacter => 'O';

        public ICommandModification CreateModificationObject(JToken variables)
        {
            int? ackNumber = null;

            //variables.ExecuteIfAvailable("f",   will be used to send settings down to ARC
            variables.ExecuteIfAvailable("g", (int value) => ackNumber = value, this.Logger);

            return new CommandModificationCapitalO(ackNumber);
        }

    }
}