namespace JsonToBusinessObjects.CommandRules
{
    using Infrastructure.Logging;

    internal abstract class CommandRuleBase
    {
        public ILogger Logger { get; }

        protected CommandRuleBase(ILogger logger)
        {
            this.Logger = logger;
        }
    }
}