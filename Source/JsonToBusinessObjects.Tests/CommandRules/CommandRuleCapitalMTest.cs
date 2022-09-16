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

    public class CommandRuleCapitalMTest
    {
        private readonly CommandRuleCapitalM testee = new CommandRuleCapitalM(A.Dummy<ILogger>());

        [Fact]
        public void TheHandledCommandCharacterShouldBeCapitalM()
        {
            this.testee.HandledCommandCharacter.Should().Be('M');
        }

        [Fact]
        public void WhenInputIsCorrect_ThenApplyingCommandModificationGivesExpectedResult()
        {
            const string inputJson =
                @"{ 
                    ""a"":""-0.0015142+0.9579943+OFL+0.0000000+26.038575+0.0000000-0.9564799+27.350000""
                }";

            float[] expectedValues = { -0.0015142f, +0.9579943f, float.NaN, +0.0000000f, +26.038575f, +0.0000000f, -0.9564799f, +27.350000f };

            var businessObjectRoot = new BusinessObjectRoot();

            ICommandModification commandModification = this.testee.CreateModificationObject(JToken.Parse(inputJson));
            commandModification.ApplyToBusinessObjectRoot(businessObjectRoot);

            businessObjectRoot.CurrentValuesOfSelectedChannels.Should().BeEquivalentTo(expectedValues);
        }

        [Fact]
        public void WhenPropertyIsAlreadySet_ThenCanApplyToBusinessObjectRootReturnsFalse()
        {
            var businessObjectRoot = new BusinessObjectRoot();

            businessObjectRoot.CurrentValuesOfSelectedChannels = new float[]{};

            ICommandModification commandModification = this.testee.CreateModificationObject(JToken.Parse(@"{}"));
            bool canApplyToBusinessObjectRoot = commandModification.CanApplyToBusinessObjectRoot(businessObjectRoot);

            canApplyToBusinessObjectRoot.Should().BeFalse("because there is already a value for CurrentValuesOfSelectedChannels present");
        }

        [Fact]
        public void WhenPropertyIsAlreadySet_ThenApplyToBusinessObjectRootThrowsException()
        {
            var businessObjectRoot = new BusinessObjectRoot();

            businessObjectRoot.CurrentValuesOfSelectedChannels = new float[] { };

            ICommandModification commandModification = this.testee.CreateModificationObject(JToken.Parse(@"{}"));

            commandModification.Invoking(_ => _.ApplyToBusinessObjectRoot(businessObjectRoot)).Should().Throw<InvalidOperationException>("because there is already a value for CurrentValuesOfSelectedChannels present");
        }
    }
}