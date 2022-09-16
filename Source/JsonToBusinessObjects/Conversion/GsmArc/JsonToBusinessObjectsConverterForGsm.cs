using System.Collections.Generic;
using System.Linq;
using JsonToBusinessObjects.CommandRules;
using JsonToBusinessObjects.CommandRules.Configuration;
using JsonToBusinessObjects.DataContainers;
using JsonToBusinessObjects.Infrastructure.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonToBusinessObjects.Conversion.GsmArc
{
    /// <summary>
    /// For GSM and ARC
    /// </summary>
    public class JsonToBusinessObjectsConverterForGsm : IJsonToBusinessObjectsConverter
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<ICommandRule> _commandRules;

        public JsonToBusinessObjectsConverterForGsm()
        {

            _logger = new InternalLogger();

            _commandRules = new ICommandRule[]
            {
                new CommandRuleCapitalB(_logger),
                new CommandRuleCapitalF(_logger),
                new CommandRuleCapitalG(_logger),
                new CommandRuleCapitalI(_logger),
                new CommandRuleCapitalM(_logger),

                new CommandRuleCapitalO(_logger),
                new CommandRuleCapitalT(_logger),

                new CommandRuleLowerA(_logger),
                new CommandRuleLowerB(_logger),
                new CommandRuleLowerC(_logger),
                new CommandRuleLowerD(_logger),
                new CommandRuleLowerF(_logger),
                new CommandRuleLowerK(_logger),
            };
        }

        public ConversionResult Convert(string jsonString)
        {
            return Convert(JObject.Parse(jsonString));
        }

        public ConversionResult Convert(JObject rootObject)
        {
            BusinessObjectRoot businessObjectRoot = new BusinessObjectRoot();

            foreach (JProperty jProperty in rootObject.Properties())
            {
                string jPropertyName = jProperty.Name;
                if (jPropertyName.Length != 1)
                {
                    _logger.Fatal($"The given root object contains a property that has a length other than one character: '{jPropertyName}' (Length {jPropertyName.Length})");
                    businessObjectRoot = null;
                    break;
                }

                char commandCharacter = jPropertyName.Single();

                var concernedCommandRules = _commandRules.Where(rule => rule.HandledCommandCharacter.Equals(commandCharacter)).ToList();

                // TODO: Exception when more than one rule for one commandCharacter?

                if (concernedCommandRules.Count == 0)
                {
                    _logger.Warn($"No command rule found to handle command with commandCharacter '{commandCharacter}' (Data:'{jProperty.Value.ToString(Formatting.None)}')");
                }

                foreach (ICommandRule concernedCommandRule in concernedCommandRules)
                {
                    ICommandModification commandModification = concernedCommandRule.CreateModificationObject(jProperty.Value);

                    if (commandModification.CanApplyToBusinessObjectRoot(businessObjectRoot) == false)
                    {
                        _logger.Error($"could not apply commandModification for {concernedCommandRule.GetType().Name}. Property: {jProperty.ToString(Formatting.None)}");
                        continue;
                    }

                    commandModification.ApplyToBusinessObjectRoot(businessObjectRoot);
                }
            }

            return new ConversionResult(businessObjectRoot, (_logger as InternalLogger)?.ConversionMessages);
        }
    }
}