namespace JsonToBusinessObjects.Tests.CommandRules
{
    using System;
    using DataContainers;
    using FakeItEasy;
    using FluentAssertions;
    using Infrastructure.Logging;
    using JsonToBusinessObjects.CommandRules;
    using Newtonsoft.Json.Linq;
    using Xunit;

    public class CommandRuleCapitalGTest
    {
        private readonly CommandRuleCapitalG testee = new CommandRuleCapitalG(A.Dummy<ILogger>());

        [Fact]
        public void TheHandledCommandCharacterShouldBeCapitalG()
        {
            this.testee.HandledCommandCharacter.Should().Be('G');
        }

        [Fact]
        public void WhenInputIsCorrect_ThenApplyingCommandModificationGivesExpectedResult()
        {
            const string inputJson = 
                @"{ 
                    ""a"": ""Text to measuring values"", 
                    ""b"": ""Text to alarm"", 
                    ""c"": ""Text to answer"" 
                }";

            var businessObjectRoot = new BusinessObjectRoot();

            ICommandModification commandModification = this.testee.CreateModificationObject(JToken.Parse(inputJson));
            commandModification.ApplyToBusinessObjectRoot(businessObjectRoot);

            businessObjectRoot.Texts.MeasuringValues.Should().Be("Text to measuring values");
            businessObjectRoot.Texts.Alarm.Should().Be("Text to alarm");
            businessObjectRoot.Texts.Answer.Should().Be("Text to answer");
        }

        [Fact]
        public void WhenPropertyIsAlreadySet_ThenCanApplyToBusinessObjectRootReturnsFalse()
        {
            var businessObjectRoot = new BusinessObjectRoot();

            businessObjectRoot.Texts = new TextContainer();

            ICommandModification commandModification = this.testee.CreateModificationObject(JToken.Parse(@"{}"));
            bool canApplyToBusinessObjectRoot = commandModification.CanApplyToBusinessObjectRoot(businessObjectRoot);

            canApplyToBusinessObjectRoot.Should().BeFalse("because there is already a value for Texts present");
        }

        [Fact]
        public void WhenPropertyIsAlreadySet_ThenApplyToBusinessObjectRootThrowsException()
        {
            var businessObjectRoot = new BusinessObjectRoot();

            businessObjectRoot.Texts = new TextContainer();

            ICommandModification commandModification = this.testee.CreateModificationObject(JToken.Parse(@"{}"));

            commandModification.Invoking(_ => _.ApplyToBusinessObjectRoot(businessObjectRoot)).Should().Throw<InvalidOperationException>("because there is already a value for Texts present");
        }
    }
}