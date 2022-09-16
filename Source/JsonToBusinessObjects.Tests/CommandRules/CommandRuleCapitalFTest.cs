namespace JsonToBusinessObjects.Tests.CommandRules
{
    using System;
    using DataContainers;
    using FakeItEasy;
    using FluentAssertions;
    using Infrastructure.Logging;
    using JsonToBusinessObjects.CommandRules;
    using JsonToBusinessObjects.Conversion;
    using Newtonsoft.Json.Linq;
    using Xunit;

    public class CommandRuleCapitalFTest
    {
        private readonly CommandRuleCapitalF testee = new CommandRuleCapitalF(A.Dummy<ILogger>());

        [Fact]
        public void TheHandledCommandCharacterShouldBeCapitalF()
        {
            this.testee.HandledCommandCharacter.Should().Be('F');
        }

        [Theory]
        [InlineData("a", MessageType.MeasurementMessage)]
        [InlineData("b", MessageType.ConfigurationMessageWithoutAck)]
        [InlineData("c", MessageType.AlarmMessage)]
        [InlineData("d", MessageType.ConfigurationMessageWithAck)]
        [InlineData("e", MessageType.RecordDataMessage)]
        [InlineData("f", MessageType.RequestedRecordDataMessage)]
        public void WhenInputIsCorrect_ThenApplyingCommandModificationGivesExpectedResult(string subcommand, MessageType expectedMessageType)
        {
            string inputJson =
                @"{ 
                    """ + subcommand + @""": 0, 
                }";

            var businessObjectRoot = new BusinessObjectRoot();

            ICommandModification commandModification = this.testee.CreateModificationObject(JToken.Parse(inputJson));
            commandModification.ApplyToBusinessObjectRoot(businessObjectRoot);

            businessObjectRoot.MessageType.Should().Be(expectedMessageType);
        }

        [Fact]
        public void WhenPropertyIsAlreadySet_ThenCanApplyToBusinessObjectRootReturnsFalse()
        {
            var businessObjectRoot = new BusinessObjectRoot {MessageType = MessageType.AlarmMessage};


            ICommandModification commandModification = this.testee.CreateModificationObject(JToken.Parse(@"{}"));
            bool canApplyToBusinessObjectRoot = commandModification.CanApplyToBusinessObjectRoot(businessObjectRoot);

            canApplyToBusinessObjectRoot.Should().BeFalse("because there is already a value for MessageType present");
        }

        [Fact]
        public void WhenPropertyIsAlreadySet_ThenApplyToBusinessObjectRootThrowsException()
        {
            var businessObjectRoot = new BusinessObjectRoot {MessageType = MessageType.AlarmMessage};


            ICommandModification commandModification = this.testee.CreateModificationObject(JToken.Parse(@"{}"));

            commandModification.Invoking(_ => _.ApplyToBusinessObjectRoot(businessObjectRoot)).Should().Throw<InvalidOperationException>("because there is already a value for MessageType present");
        }
    }
}