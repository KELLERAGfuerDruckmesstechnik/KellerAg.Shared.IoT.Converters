using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JsonToBusinessObjects.Conversion;
using JsonToBusinessObjects.Conversion.GsmArc;
using Xunit;

namespace JsonToBusinessObjects.Tests.GSM_Conversion
{
    public class MeasurementDataDecoderTest
    {
        private readonly MeasurementDataDecoder _testee = new MeasurementDataDecoder();

        private const float Precision = 4e-4f;

        [Fact]
        public void WhenGivenAValidBase64String_ThenValuesAreCorrect()
        {
            // Arrange
            // #F/e=0#C/a=2239/b=5#T/s=552566160/p=17.07.05,12:34:52+08#M/a=+0.0053356+0.9665605+0.0000000+27.224365+0.0000000+0.9612249+27.750000+17.132229+17.973981/c=+1+1#I/n=1815/s=19/b=99/f=16.51/v=+3.866#B/a=Jr0g729LAABwQdZmoEGJR7BBjRTwAAJYADutSBA/d4AgAAAAQEHT3VAAAABgP3YlcEHYAKBBiTiwQY2O/wAAACa9IO9z+gAAADus7RA/d3ogAAAAQEHVElAAAABgP3YgcEHZM6BBiTKwQY4A8AACWAA7rlYQP3d3IAAAAEBB1mUmvSDvdlIAAFAAAABgP3YacEHbM6BBiR2wQY6K8AACWAA7reMQP3d0IAAAAEBB13xQAAAAYD92GXBB3ACgQYkgJr0g73iqAACwQY7w8AACWAA7rZwQP3d1IAAAAEBB2JVQAAAAYD92GnBB3MygQYkUsEGPXPAAAlsAO67XED93cCa9IO99XQAAIAAAAEBB2ctQAAAAYD92EnBB3gCgQYkOsEGPyv9BwhVQAAAA8AAAPAA771cQP3eGQEHCcVAAAAA=#E/e#X/a=57523
            byte[] dataFromBase64String = Convert.FromBase64String("Jr0g729LAABwQdZmoEGJR7BBjRTwAAJYADutSBA/d4AgAAAAQEHT3VAAAABgP3YlcEHYAKBBiTiwQY2O/wAAACa9IO9z+gAAADus7RA/d3ogAAAAQEHVElAAAABgP3YgcEHZM6BBiTKwQY4A8AACWAA7rlYQP3d3IAAAAEBB1mUmvSDvdlIAAFAAAABgP3YacEHbM6BBiR2wQY6K8AACWAA7reMQP3d0IAAAAEBB13xQAAAAYD92GXBB3ACgQYkgJr0g73iqAACwQY7w8AACWAA7rZwQP3d1IAAAAEBB2JVQAAAAYD92GnBB3MygQYkUsEGPXPAAAlsAO67XED93cCa9IO99XQAAIAAAAEBB2ctQAAAAYD92EnBB3gCgQYkOsEGPyv9BwhVQAAAA8AAAPAA771cQP3eGQEHCcVAAAAA=");
            float[] expectedValues = { +0.0053356f, +0.9665605f, +0.0000000f, +27.224365f, +0.0000000f, +0.9612249f, +27.750000f, +17.132229f, +17.973981f };


            // Act
            ChannelDataStorage result = this._testee.Decode(dataFromBase64String);

            // Assert
            List<DataPoint> dataPoints = result.DataPointsByChannel.OrderBy(pair => pair.Key).Select(pair => pair.Value.OrderBy(point => point.Time).Last()).ToList();

            dataPoints.Should().HaveCount(9);
            foreach (var z in dataPoints.Select(point => point.Value).Zip(expectedValues, (a, e) => new { actual = a, expected = e }))
            {
                z.actual.Should().BeApproximately(z.expected, Precision);
            }
        }


        [Fact]
        public void WhenGivenABase64StringWithOFL_ThenNaNValuesMustBeIgnored()
        {
            // Arrange
            byte[] dataFromBase64String = Convert.FromBase64String("IsMiGXm7AACwU2jU8AACWQBTaNQQU2jUIFNo1EBTaNRQU2jUYD93tXBBvzOgU2jUsFNo1PAAAlgAU2jUEFNo1CLDIhl+bAAAIFNo1EBTaNRQU2jUYD93tXBBvGagU2jUsFNo1PAAAlgAU2jUEFNo1CBTaNRAU2jUUFNo1GA/d7EiwyIZgMQAAHBBu5mgU2jUsFNo1PAAAlgAU2jUEFNo1CBTaNRAU2jUUFNo1GA/d7JwQbrMoFNo1LBTaNT/QTcvIsMiGYV0AAAAU2jUEFNo1CBTaNRAU2jUUFNo1GA/d7RwQbuZoFNo1LBTaNTwAAJYAFNo1BBTaNQgU2jUQFNo1CLDIhmHzAAAUFNo1GA/d6twQb0zoFNo1LBTaNT/QTcvsEEmt/AAAlgAO6HYED920CAAAABAQafzUAAAAGA/dYw=");
            //There are 9 values. Yet 7 are OFL which will be scrambled back to a float number like 9E+11

            float[] expectedValues = { float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, 0.967453f, +23.6499023f, float.NaN, float.NaN};


            // Act
            ChannelDataStorage result = this._testee.Decode(dataFromBase64String);

            // Assert
            List<DataPoint> dataPoints = result.DataPointsByChannel.OrderBy(pair => pair.Key).Select(pair => pair.Value.OrderBy(point => point.Time).Last()).ToList();

            dataPoints.Should().HaveCount(9);
            foreach (var z in dataPoints.Select(point => point.Value).Zip(expectedValues, (a, e) => new { actual = a, expected = e }))
            {
                if (float.IsNaN(z.actual))
                {
                    z.expected.Should().Be(float.NaN);
                }
                else
                {
                    z.actual.Should().BeApproximately(z.expected, Precision);
                }   
            }
        }
    }
}