namespace JsonToBusinessObjects.Tests.CommandRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataContainers;
    using FakeItEasy;
    using FluentAssertions;
    using Infrastructure.Logging;
    using JsonToBusinessObjects.CommandRules;
    using JsonToBusinessObjects.Conversion;
    using Newtonsoft.Json.Linq;
    using Xunit;

    public class CommandRuleCapitalBTest
    {
        private readonly CommandRuleCapitalB testee = new CommandRuleCapitalB(A.Dummy<ILogger>());
        private const float Precision = 4e-4f;

        [Fact]
        public void TheHandledCommandCharacterShouldBeCapitalB()
        {
            this.testee.HandledCommandCharacter.Should().Be('B');
        }

        [Fact]
        public void WhenNoDataInAParameter_ThenThrowException()
        {
            this.testee
                .Invoking(_ => _.CreateModificationObject(JToken.Parse(@"{a:""""}")))
                .Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void WhenNoAParameter_ThenThrowException()
        {
            this.testee
                .Invoking(_ => _.CreateModificationObject(JToken.Parse(@"{}")))
                .Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void WhenValidBase64_ThenTheFloatMeasurementValuesAreCorrectlyParsed()
        {
            // Arange
            JToken jTokenInput = JToken.Parse(@"{a:""CGIgwZ2jAAAg////QEHsZVD///9gP3c8cEH5HoA6tACQOqAA//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8=""}");
            float[] expectedValues = { float.NaN, +29.549683f, float.NaN, +0.9657700f, +31.139997f, +0.0013732f, +0.0012207f };
            BusinessObjectRoot businessObjectRoot = new BusinessObjectRoot();

            // Act
            ICommandModification commandModification = this.testee.CreateModificationObject(jTokenInput);
            commandModification.ApplyToBusinessObjectRoot(businessObjectRoot);
            List<DataPoint> dataPoints = businessObjectRoot.Measurements.DataPointsByChannel.OrderBy(pair => pair.Key).Select(pair => pair.Value.OrderBy(point => point.Time).Last()).ToList();

            // Assert
            dataPoints.Should().HaveCount(7);
            foreach (var z in dataPoints.Select(point => point.Value).Zip(expectedValues, (a, e) => new { actual = a, expected = e }))
            {
                if (float.IsNaN(z.expected))
                {
                    z.actual.Should().Be(float.NaN);
                    continue;
                }
                z.actual.Should().BeApproximately(z.expected, Precision);
            }
        }

        [Fact]
        public void WhenValidBase64_ThenTheTimeValuesAreCorrectlyParsed()
        {
            // Arange
            JToken jTokenInput = JToken.Parse(@"{a:""CGIgwZ2jAAAg////QEHsZVD///9gP3c8cEH5HoA6tACQOqAA//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8=""}");
            float[] expectedValues = { float.NaN, +29.549683f, float.NaN, +0.9657700f, +31.139997f, +0.0013732f, +0.0012207f };
            BusinessObjectRoot businessObjectRoot = new BusinessObjectRoot();

            // Act
            ICommandModification commandModification = this.testee.CreateModificationObject(jTokenInput);
            commandModification.ApplyToBusinessObjectRoot(businessObjectRoot);
            List<DataPoint> dataPoints = businessObjectRoot.Measurements.DataPointsByChannel.OrderBy(pair => pair.Key).Select(pair => pair.Value.OrderBy(point => point.Time).Last()).ToList();

            // Assert
            dataPoints.Should().HaveCount(7);
            foreach (var z in dataPoints.Select(point => point.Time).Zip(expectedValues, (a, e) => new { actual = a, expected = e }))
            {

                //z.actual.Should().Be(DateTime.Parse("31.05.2017, 15:28:35")); this format is system dependent and doesn't work on the VSTS
                //Console.WriteLine(DateTime.Parse("31.05.2017, 15:28:35").Ticks.ToString() + "is 636318413150000000");
                z.actual.Ticks.Should().Be(636318413150000000);
            }
        }
    }
}