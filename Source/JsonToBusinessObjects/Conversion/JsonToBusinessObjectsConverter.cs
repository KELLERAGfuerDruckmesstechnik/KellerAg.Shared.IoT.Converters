namespace JsonToBusinessObjects.Conversion
{
    using System.Collections.Generic;
    using System.Linq;
    using CommandRules;
    using CommandRules.Configuration;
    using DataContainers;
    using Infrastructure.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class JsonToBusinessObjectsConverter
    {
        private readonly ILogger _logger;

        private readonly IEnumerable<ICommandRule> _commandRules;

        public JsonToBusinessObjectsConverter()
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

        // bei #M/b Messungen: 1) 'Messinterval' und 'angewählte Kanäle' übergeben um #M/b Messungen interpertieren zu können --> Zuerst BASE64 Codierung 
        //                     2) +SelektierterKanal1+SelektierterKanal2-SelektierterKanal3
        // MOJ über Slack 8.6.17: In Absprache mit Marcel müssen wir "M/b"-Inhalte nicht unterstützen, [..][jedoch] einen Hinweis sehen (auf dem SPA) wenn solche 
        //                        Nachrichten kommen. Dann muss der Benutzer das Gerät umkonfigurieren.
        // MOJ über Slack 8.6.17: "M/a"-Inhalte können wir vorerst ignorieren. (Vielleicht/hoffentlich für immer)
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

    // ReSharper disable InconsistentNaming
    public enum GsmDeviceType
    {
        Type0_RS485 = 0,
        Type1_RS485_2DI = 1,
        Type2_RS485_BP1P2_1DI = 2,
        Type3_RS485_BP1PB_1DI = 3,
        Type4_RS485_BP1P2_1DI_VI = 4,
        Type5_RS485_BP1PB_1DI_VI = 5,
        Type6_RS485x5_BP1P2_12DI_VI = 6,
        Type7_SDI12_B_12DI_VI = 7,
        Type8_RS485_5P1TOB1_B_12DI = 8,
        Type9_RS485_CTD_BP1P2_1DI_VI = 9,
        Type10_RS485_CTD_BP1PB_1DI_VI = 10
    }
}