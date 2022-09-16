namespace JsonToBusinessObjects.CommandRules
{
    using System;
    using DataContainers;
    using Infrastructure;
    using Infrastructure.Logging;
    using Newtonsoft.Json.Linq;

    internal class CommandRuleCapitalG : CommandRuleBase, ICommandRule
    {
        private class CommandModificationCapitalG : ICommandModification
        {
            private readonly TextContainer _textContainer;

            public CommandModificationCapitalG(TextContainer textContainer)
            {
                this._textContainer = textContainer;
            }

            public void ApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                if (this.CanApplyToBusinessObjectRoot(businessObjectRoot) == false)
                {
                    throw new InvalidOperationException($"Cannot apply command rule {nameof(CommandModificationCapitalG)} to business object.");
                }

                businessObjectRoot.Texts = this._textContainer;
            }

            public bool CanApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                return businessObjectRoot.Texts == null;
            }
        }

        public CommandRuleCapitalG(ILogger logger) : base(logger)
        {
        }

        public char HandledCommandCharacter => 'G';

        public ICommandModification CreateModificationObject(JToken variables)
        {
            TextContainer textContainer = new TextContainer();

            variables.ExecuteIfAvailable("a", (string value) => textContainer.MeasuringValues = value, this.Logger);
            variables.ExecuteIfAvailable("b", (string value) => textContainer.Alarm = value, this.Logger);
            variables.ExecuteIfAvailable("c", (string value) => textContainer.Answer = value, this.Logger);

            return new CommandModificationCapitalG(textContainer);
        }
    }
}