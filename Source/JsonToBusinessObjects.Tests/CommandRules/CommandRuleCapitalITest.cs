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

    public class CommandRuleCapitalITest
    {
        private readonly CommandRuleCapitalI testee = new CommandRuleCapitalI(A.Dummy<ILogger>());

        [Fact]
        public void TheHandledCommandCharacterShouldBeCapitalI()
        {
            this.testee.HandledCommandCharacter.Should().Be('I');
        }

        [Fact]
        public void Test()
        {
            // #I/n=112233/s=16/b=99/e=9.20/f=17.21/h=37/v=+3.572
            const string inputJson =
                @"{ 
                    ""n"": ""112233"", 
                    ""s"": ""16"", 
                    ""b"": ""99"", 
                    ""e"": ""9.20"", 
                    ""f"": ""17.21"", 
                    ""h"": ""37"", 
                    ""v"": ""+3.572"", 
                }";

            var businessObjectRoot = new BusinessObjectRoot();

            ICommandModification commandModification = this.testee.CreateModificationObject(JToken.Parse(inputJson));
            commandModification.ApplyToBusinessObjectRoot(businessObjectRoot);

            businessObjectRoot.DeviceInformation.DeviceSerialNumber.Should().Be(112233);
            businessObjectRoot.DeviceInformation.SignalQuality.Should().Be(16);
            businessObjectRoot.DeviceInformation.BatteryCapacity.Should().Be(99);
            businessObjectRoot.DeviceInformation.DeviceIdAndClass.Should().Be("9.20");
            businessObjectRoot.DeviceInformation.GsmModuleSoftwareVersion.Should().Be("17.21");
            businessObjectRoot.DeviceInformation.MeasuredHumidity.Should().Be(37);
            businessObjectRoot.DeviceInformation.MeasuredBatteryVoltage.Should().Be(3.572f);
            businessObjectRoot.DeviceInformation.ProductLine.Should().Be(ProductLineName.ARC1); //ARC because the is a DeviceIdAndClass
        }

        [Fact]
        public void WhenADeviceDoesntHaveDeviceId_ThenItIsAnGSMDeviceNotAnArc()
        {
            // #I/n=112233/s=16/b=99/f=17.21/h=37/v=+3.572
            const string inputJson =
                @"{ 
                    ""n"": ""112233"", 
                    ""s"": ""16"", 
                    ""b"": ""99"", 

                    ""f"": ""17.21"", 
                    ""h"": ""37"", 
                    ""v"": ""+3.572"", 
                }";

            var businessObjectRoot = new BusinessObjectRoot();

            ICommandModification commandModification = this.testee.CreateModificationObject(JToken.Parse(inputJson));
            commandModification.ApplyToBusinessObjectRoot(businessObjectRoot);

            businessObjectRoot.DeviceInformation.DeviceSerialNumber.Should().Be(112233);
            businessObjectRoot.DeviceInformation.SignalQuality.Should().Be(16);
            businessObjectRoot.DeviceInformation.BatteryCapacity.Should().Be(99);
            businessObjectRoot.DeviceInformation.DeviceIdAndClass.Should().NotBe("9.20");
            businessObjectRoot.DeviceInformation.GsmModuleSoftwareVersion.Should().Be("17.21");
            businessObjectRoot.DeviceInformation.MeasuredHumidity.Should().Be(37);
            businessObjectRoot.DeviceInformation.MeasuredBatteryVoltage.Should().Be(3.572f);
            businessObjectRoot.DeviceInformation.ProductLine.Should().Be(ProductLineName.GSM); //because the is NO a DeviceIdAndClass
        }

        [Fact]
        public void WhenPropertyIsAlreadySet_ThenCanApplyToBusinessObjectRootReturnsFalse()
        {
            var businessObjectRoot = new BusinessObjectRoot();

            businessObjectRoot.DeviceInformation = new DeviceInformation();

            ICommandModification commandModification = this.testee.CreateModificationObject(JToken.Parse(@"{}"));
            bool canApplyToBusinessObjectRoot = commandModification.CanApplyToBusinessObjectRoot(businessObjectRoot);

            canApplyToBusinessObjectRoot.Should().BeFalse("because there is already a value for DeviceInformation present");
        }

        [Fact]
        public void WhenPropertyIsAlreadySet_ThenApplyToBusinessObjectRootThrowsException()
        {
            var businessObjectRoot = new BusinessObjectRoot();

            businessObjectRoot.DeviceInformation = new DeviceInformation();

            ICommandModification commandModification = this.testee.CreateModificationObject(JToken.Parse(@"{}"));

            commandModification.Invoking(_ => _.ApplyToBusinessObjectRoot(businessObjectRoot)).Should().Throw<InvalidOperationException >("because there is already a value for DeviceInformation present");
        }
    }
}