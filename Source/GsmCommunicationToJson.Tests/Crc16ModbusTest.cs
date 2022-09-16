namespace GsmCommunicationToJson.Tests
{
    using System.Text;
    using FluentAssertions;
    using Xunit;

    /// <summary>
    /// Verified with https://www.lammertbies.nl/comm/info/crc-calculation.html and http://www.scadacore.com/field-tools/programming-calculators/online-checksum-calculator/
    /// </summary>
    public class Crc16ModbusTest
    {
        private readonly Crc16Modbus _testee = new Crc16Modbus();

        [Fact]
        public void ComputeChecksum_WhenValueIsZero_ThenChecksumIsCorrect()
        {
            ushort checksum = this._testee.ComputeChecksum(new byte[] {0});

            checksum.Should().Be(0x40BF);
        }

        [Fact]
        public void ComputeChecksum_WhenValueIsOneByte_ThenChecksumIsCorrect()
        {
            ushort checksum = this._testee.ComputeChecksum(new byte[] {0x1A});

            checksum.Should().Be(0x8B3E);
        }

        [Fact]
        public void ComputeChecksum_WhenValueIsTwoBytes_ThenChecksumIsCorrect()
        {
            ushort checksum = this._testee.ComputeChecksum(new byte[] {0x1A, 0x2B});

            checksum.Should().Be(0xCF4A);
        }

        [Fact]
        public void ComputeChecksum_WhenValueIsOneAsciiCharacters_ThenChecksumIsCorrect()
        {
            ushort checksum = this._testee.ComputeChecksum(Encoding.ASCII.GetBytes("K"));

            checksum.Should().Be(0x77FF);
        }

        [Fact]
        public void ComputeChecksum_WhenValueIsMultipleAsciiCharacters_ThenChecksumIsCorrect()
        {
            ushort checksum = this._testee.ComputeChecksum(Encoding.ASCII.GetBytes("Lorem ipsum dolor sit amet"));

            checksum.Should().Be(0x272D);
        }

        [Fact]
        public void ComputeChecksum_WhenValueIsMultipleAsciiCharactersWithSpecialChars_ThenChecksumIsCorrect()
        {
            ushort checksum = this._testee.ComputeChecksum(Encoding.ASCII.GetBytes("#F/f=0#C/a=3003/b=30#T/s=548285170/p=00000000000000000000#I/n=1152/s=20/b=99/f=17.20/v=+3.802#O/g=39035#B/a=y7sAAVGGAA"));

            checksum.Should().Be(0x7494);
        }
    }
}