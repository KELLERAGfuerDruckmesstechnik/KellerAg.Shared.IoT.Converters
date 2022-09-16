namespace JsonToBusinessObjects.CommandRules
{
    using System;
    using System.Linq;
    using DataContainers;
    using Infrastructure;
    using Infrastructure.Logging;
    using Newtonsoft.Json.Linq;

    internal class CommandRuleCapitalM : CommandRuleBase, ICommandRule
    {
        private class CommandModificationCapitalM : ICommandModification
        {
            private readonly float[] _currentValuesOfSelectedChannels;

            public CommandModificationCapitalM(float[] currentValuesOfSelectedChannels)
            {
                this._currentValuesOfSelectedChannels = currentValuesOfSelectedChannels;
            }

            public void ApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                if (this.CanApplyToBusinessObjectRoot(businessObjectRoot) == false)
                {
                    throw new InvalidOperationException($"Cannot apply command rule {nameof(CommandModificationCapitalM)} to business object.");
                }

                businessObjectRoot.CurrentValuesOfSelectedChannels = this._currentValuesOfSelectedChannels;
            }

            public bool CanApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                return businessObjectRoot.CurrentValuesOfSelectedChannels == null;
            }
        }

        public CommandRuleCapitalM(ILogger logger) : base(logger)
        {
        }

        public char HandledCommandCharacter => 'M';

        public ICommandModification CreateModificationObject(JToken variables)
        {
            float[] currentValuesOfSelectedChannels = null;

            variables.ExecuteIfAvailable("a", (string value) => currentValuesOfSelectedChannels = this.ParseCurrentValuesOfSelectedChannels(value), this.Logger);
            variables.ExecuteIfAvailable("b", (string value) => currentValuesOfSelectedChannels = this.ParseCurrentValuesOfSelectedChannels(value), this.Logger);

            return new CommandModificationCapitalM(currentValuesOfSelectedChannels);
        }

        private float[] ParseCurrentValuesOfSelectedChannels(string value)
        {
            string[] floatValues = value.Replace("+", "|+").Replace("-", "|-").Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);
            float[] floats = floatValues.Select(s => s.Equals("+OFL", StringComparison.OrdinalIgnoreCase) ? float.NaN : float.Parse(s)).ToArray();

            return floats;
        }
    }
}