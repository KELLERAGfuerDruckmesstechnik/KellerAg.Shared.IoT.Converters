namespace JsonToBusinessObjects.Tests.CommandRules.Configuration
{
    using System;
    using System.Text;
    using DataContainers;
    using DataContainers.Configuration;
    using FakeItEasy;
    using FluentAssertions;
    using Infrastructure.Logging;
    using JsonToBusinessObjects.CommandRules;
    using JsonToBusinessObjects.CommandRules.Configuration;
    using Newtonsoft.Json.Linq;
    using Xunit;

    public class CommandRuleLowerATest
    {
        private readonly CommandRuleLowerA testee = new CommandRuleLowerA(A.Dummy<ILogger>());

        [Fact]
        public void TheHandledCommandCharacterShouldBeCapitalG()
        {
            this.testee.HandledCommandCharacter.Should().Be('a');
        }

        [Fact]
        public void WhenInputIsCorrect_ThenApplyingCommandModificationGivesExpectedResult()
        {
            string inputJson = CreateInputJson();
            var businessObjectRoot = new BusinessObjectRoot();

            ICommandModification commandModification = this.testee.CreateModificationObject(JToken.Parse(inputJson));
            commandModification.ApplyToBusinessObjectRoot(businessObjectRoot);

            businessObjectRoot.Configuration.GprsSettings.GprsAPN.Should().Be("a_Value");
            businessObjectRoot.Configuration.GprsSettings.GprsID.Should().Be("b_Value");
            businessObjectRoot.Configuration.GprsSettings.GprsPassword.Should().Be("c_Value");
            businessObjectRoot.Configuration.GprsSettings.GprsDNS.Should().Be("d_Value");
            businessObjectRoot.Configuration.GprsSettings.SmtpShowedName.Should().Be("e_Value");
            businessObjectRoot.Configuration.GprsSettings.PopUsername.Should().Be("f_Value");
            businessObjectRoot.Configuration.GprsSettings.PopPassword.Should().Be("g_Value");
            businessObjectRoot.Configuration.GprsSettings.OptSmtpUsername.Should().Be("h_Value");
            businessObjectRoot.Configuration.GprsSettings.OptSmtpPassword.Should().Be("i_Value");
            businessObjectRoot.Configuration.GprsSettings.Pop3Server.Should().Be("j_Value");
            businessObjectRoot.Configuration.GprsSettings.Pop3Port.Should().Be("k_Value");
            businessObjectRoot.Configuration.GprsSettings.SmtpServer.Should().Be("l_Value");
            businessObjectRoot.Configuration.GprsSettings.SmtpPort.Should().Be("m_Value");
            businessObjectRoot.Configuration.GprsSettings.ReturnAddress.Should().Be("n_Value");
        }

        [Fact]
        public void WhenPropertyIsAlreadySet_ThenCanApplyToBusinessObjectRootReturnsFalse()
        {
            var businessObjectRoot = new BusinessObjectRoot();

            businessObjectRoot.Configuration = new ConfigurationContainer { GprsSettings = new GprsSettings() };

            ICommandModification commandModification = this.testee.CreateModificationObject(JToken.Parse(@"{}"));
            bool canApplyToBusinessObjectRoot = commandModification.CanApplyToBusinessObjectRoot(businessObjectRoot);

            canApplyToBusinessObjectRoot.Should().BeFalse("because there is already a value present");
        }

        [Fact]
        public void WhenPropertyIsAlreadySet_ThenApplyToBusinessObjectRootThrowsException()
        {
            var businessObjectRoot = new BusinessObjectRoot();

            businessObjectRoot.Configuration = new ConfigurationContainer {GprsSettings = new GprsSettings()};

            ICommandModification commandModification = this.testee.CreateModificationObject(JToken.Parse(@"{}"));

            commandModification.Invoking(_ => _.ApplyToBusinessObjectRoot(businessObjectRoot)).Should().Throw<InvalidOperationException>("because there is already a value present");
        }

        private static string CreateInputJson()
        {
            char[] commandCharacters = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            foreach (char commandCharacter in commandCharacters)
            {
                sb.AppendLine($"\"{commandCharacter}\": \"{commandCharacter + "_Value"}\",");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}