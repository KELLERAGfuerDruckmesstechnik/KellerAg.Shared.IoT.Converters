using System;

namespace GsmCommunicationToJson.Tests
{
    using Exceptions;
    using FluentAssertions;
    using Newtonsoft.Json.Linq;
    using Xunit;

    public class GsmCommunicationToJsonConverterTest
    {
        private readonly GsmCommunicationToJsonConverter _testee = new GsmCommunicationToJsonConverter();

        [Fact]
        public void WhenTryingToConvertWithEmptyMessage_ThenExceptionIsThrown()
        {
            this._testee.Invoking(_ => _.Convert("")).Should().Throw<EmptyGsmMessageException>();
        }

        [Fact]
        public void WhenTryingToConvertWithInvalidMessage_ThenExceptionIsThrown()
        {
            string invalidMessage = "anInvalidMessage";
            this._testee.Invoking(_ => _.Convert(invalidMessage))
                .Should().Throw<InvalidGsmMessageFormatException>();
        }

        [Fact]
        public void WhenTryingToConvertWithInvalidMessage_ThenExceptionContainsCorrectInformation()
        {
            string invalidMessage = "anInvalidMessage";
            this._testee.Invoking(_ => _.Convert(invalidMessage))
                .Should().Throw<InvalidGsmMessageFormatException>()
                .WithMessage("The Message was in invalid format (see member MessageWithInvalidFormat).")
                .Where(exception => exception.MessageWithInvalidFormat.Equals(invalidMessage));
        }

        [Fact]
        public void WhenSimpleMessage_ThenConversionIsCorrect()
        {
            JObject convertedObject = this._testee.Convert("#F/b=0.3/r=rValue#E/e");

            JToken commandF = convertedObject["F"];
            commandF["b"].Value<string>().Should().Be("0.3");
            commandF["r"].Value<string>().Should().Be("rValue");

            JToken commandE = convertedObject["E"];
            commandE["e"].Value<string>().Should().Be("");
        }

        [Fact]
        public void WhenSimpleMessageBase64_ThenConversionIsCorrect()
        {
            JObject convertedObject = this._testee.Convert("#B/a=VGhpcyBpcyBhIHRlc3Q=#E/e");

            JToken commandB = convertedObject["B"];
            commandB["a"].Value<string>().Should().Be("VGhpcyBpcyBhIHRlc3Q=");

            JToken commandE = convertedObject["E"];
            commandE["e"].Value<string>().Should().Be("");

            convertedObject.Count.Should().Be(2);
        }

        [Fact]
        public void WhenMessageWithCRLF_ThenMessageIsHandledByRemovingCRLF()
        {
            JObject convertedObject = this._testee.Convert("#F/b=0#T/s=633340852/p=20.01.26,10:00:59+08#M/a=+OFL+OFL+OFL+0.9994725+21.900000/c=+1+1/d=+OFL+OFL+OFL+OFL+OFL+OFL+0.9994725+" + Environment.NewLine + 
                                                          "21.900000#I/n=3998/s=31/b=97/f=15.50/v=+3.897#a/a=iot.1nce.net/b=/c=/d=000.000.000.000/e=gsm_131@gsmdata.ch/f=gsm_131@gsmdata.ch/g=aB-b91As/h=gsm_131@" +Environment.NewLine +
                                                          "gsmdata.ch/i=aB-b91As/j=pop.gsmdata.ch/k=110/l=smtp.gsmdata.ch/m=587/n=gsm_131@gsmdata.ch#b/a=gsm_131@gsmdata.ch/b=gsm_131@gsmdata.ch/c=gsm_131@gsmdata.ch/g=/j=/k=+358449835326/m=+358449835326/n=+358449835326/o=+358449835326/q=+882285000016868/r=Testi/s=+882285100338757/t=Jorvas/u=/v=Alarm/w=/0=0/1=0/2=1#c/a=633376800/b=633427200/c=633427200/d=633344400/e=601387200/g=90000/h=86400/i=86400/j=3600/k=300/m=211/n=0/o=1/p=0/q=1/r=0/s=1/t=1/v=6/w=5/x=8/y=15/z=240/0=0/1=0/2=0/3=0/4=3/5=10/6=3/7=0/8=1/9=2#f/a=601387200/g=300/h=300/m=0/n=1/o=1/q=2/z=0/3=2#d/a=+2.0000000/b=+1.0000000/c=+0.0000000/f=+1.0000000/g=+1.0000000/i=+5.0000000/j=+1.0000000/k=+0.0030000/m=+1.0000000/n=+0.0000000/o=+0.0000000/p=+0.0000000/q=+998.20000/r=+1.0000000/s=+0.0000000/t=+0.0000000/u=+0.0000000/v=+10.000000/w=+11.000000/0=+0.0000000/1=+0.0000000/2=+1.0000000#k/a=ftp.gsmdata.ch/b=gsm_131@gsmdata.ch/c=aB-b91As/d=gsm_131@gsmdata.ch/e=21/f=21/g=2000/h=1nceTesti#E/e");
            convertedObject.Count.Should().Be(11);
        }

        [Fact]
        public void WhenRealisticMessageArc1DataFilePlain_ThenConversionIsCorrect()
        {
            // Message 'ARC-1 Data file (plain Text / 5 measurements)' from 'Different Data formats of ARC-1 GSM-2.txt'
            JObject convertedObject = this._testee.Convert("#F/a=0" + 
                                                          "#T/s=549559125/p=17.05.31,17:18:31+08/m=549558984" +
                                                          "#M/b=-0.9661281-0.0000822+OFL+29.481324+OFL+0.9659800+30.570002+0.0010681+0.0008392-0.9660997-0.0002241+OFL+29.568237+OFL+0.9658999+30.610000+0.0014495+0.0012969-0.9660616-0.0002765+OFL+29.548704+OFL+0.9658599+30.610000+0.0015258+0.0014495-0.9662744-0.0004277+OFL+29.558347+OFL+0.9658700+30.570002+0.0016021+0.0014495-0.9660347-0.0001642+OFL+29.617066+OFL+0.9659100+30.520000+0.0013732+0.0014495/c=+1+1" +
                                                          "#I/n=112233/s=16/b=99/e=9.20/f=17.21/h=37/v=+3.572" +
                                                          "#G/a=This is a Measurement TEXT" +
                                                          "#E/e" +
                                                          "#X/a=25903");

            JToken commandF = convertedObject["F"];
            commandF["a"].Value<string>().Should().Be("0");

            JToken commandT = convertedObject["T"];
            commandT["s"].Value<string>().Should().Be("549559125");
            commandT["p"].Value<string>().Should().Be("17.05.31,17:18:31+08");
            commandT["m"].Value<string>().Should().Be("549558984");

            JToken commandM = convertedObject["M"];
            commandM["b"].Value<string>().Should().Be("-0.9661281-0.0000822+OFL+29.481324+OFL+0.9659800+30.570002+0.0010681+0.0008392-0.9660997-0.0002241+OFL+29.568237+OFL+0.9658999+30.610000+0.0014495+0.0012969-0.9660616-0.0002765+OFL+29.548704+OFL+0.9658599+30.610000+0.0015258+0.0014495-0.9662744-0.0004277+OFL+29.558347+OFL+0.9658700+30.570002+0.0016021+0.0014495-0.9660347-0.0001642+OFL+29.617066+OFL+0.9659100+30.520000+0.0013732+0.0014495");
            commandM["c"].Value<string>().Should().Be("+1+1");

            JToken commandI = convertedObject["I"];
            commandI["n"].Value<string>().Should().Be("112233");
            commandI["s"].Value<string>().Should().Be("16");
            commandI["b"].Value<string>().Should().Be("99");
            commandI["e"].Value<string>().Should().Be("9.20");
            commandI["f"].Value<string>().Should().Be("17.21");
            commandI["h"].Value<string>().Should().Be("37");
            commandI["v"].Value<string>().Should().Be("+3.572");

            JToken commandG = convertedObject["G"];
            commandG["a"].Value<string>().Should().Be("This is a Measurement TEXT");

            JToken commandE = convertedObject["E"];
            commandE["e"].Value<string>().Should().Be("");

            JToken commandX = convertedObject["X"];
            commandX["a"].Value<string>().Should().Be("25903");

            convertedObject.Count.Should().Be(7);
        }

        [Fact]
        public void WhenRealisticMessageArc1DataFileBase64_ThenConversionIsCorrect()
        {
            // Message 'ARC-1 Data file (base64 / 5 measurements)' from 'Different Data formats of ARC-1 GSM-2.txt'
            JObject convertedObject = this._testee.Convert("#F/e=0" +
                                                          "#C/a=2148/b=2" +
                                                          "#T/s=549559786/p=17.05.31,17:30:25+08" +
                                                          "#M/a=-0.9658963-0.0001001+OFL+29.549683+OFL+0.9657700+31.139997+0.0013732+0.0012207/c=+1+1" +
                                                          "#I/n=112233/s=17/b=99/e=9.20/f=17.21/h=35/v=+3.555" +
                                                          "#B/a=CGIgwZ2jAAAg////QEHsZVD///9gP3c8cEH5HoA6tACQOqAA//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8=" +
                                                          "#E/e" +
                                                          "#X/a=30033");

            JToken commandF = convertedObject["F"];
            commandF["e"].Value<string>().Should().Be("0");

            JToken commandC = convertedObject["C"];
            commandC["a"].Value<string>().Should().Be("2148");
            commandC["b"].Value<string>().Should().Be("2");

            JToken commandT = convertedObject["T"];
            commandT["s"].Value<string>().Should().Be("549559786");
            commandT["p"].Value<string>().Should().Be("17.05.31,17:30:25+08");

            JToken commandM = convertedObject["M"];
            commandM["a"].Value<string>().Should().Be("-0.9658963-0.0001001+OFL+29.549683+OFL+0.9657700+31.139997+0.0013732+0.0012207");
            commandM["c"].Value<string>().Should().Be("+1+1");

            JToken commandI = convertedObject["I"];
            commandI["n"].Value<string>().Should().Be("112233");
            commandI["s"].Value<string>().Should().Be("17");
            commandI["b"].Value<string>().Should().Be("99");
            commandI["e"].Value<string>().Should().Be("9.20");
            commandI["f"].Value<string>().Should().Be("17.21");
            commandI["h"].Value<string>().Should().Be("35");
            commandI["v"].Value<string>().Should().Be("+3.555");

            JToken commandB = convertedObject["B"];
            commandB["a"].Value<string>().Should().Be("CGIgwZ2jAAAg////QEHsZVD///9gP3c8cEH5HoA6tACQOqAA//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8=");

            JToken commandE = convertedObject["E"];
            commandE["e"].Value<string>().Should().Be("");

            JToken commandX = convertedObject["X"];
            commandX["a"].Value<string>().Should().Be("30033");

            convertedObject.Count.Should().Be(8);
        }

        // TODO: Find out, why the checksum is invalid 
        // [Fact(Skip = "Message '1. Email or FTP File' has invalid checksum (WHY?) and is skipped")]
        [Fact]
        public void WhenRealisticMessage_ThenConversionIsCorrect()
        {
            // Message '1. Email or FTP File' from 'Different Data formats of ARC-1 GSM-2.txt'
            JObject convertedObject = this._testee.Convert("#F/f=0" +
                                                          "#C/a=3123/b=17" +
                                                          "#T/s=548285406/p=00000000000000000000#I/n=1152/s=20/b=99/f=17.20/v=+3.779" +
                                                          "#O/g=39035" +
                                                          "#B/a=TDEgrhmAAACQPIwA/z93cXBBxszwAAEsAL9r5xA9OJMgAAAAQEGis1AAAABgP3dwcEHHM/AAASwAv2voED04bcw0IK4Z0gAAEFNo1GA/d0pwQehmgAAAAJA8jAD/AAEtAL9r5BA9OJQgAAAAQEGimVAAAABgP3ducEHGzP9BaADMNSCuHFMAABBTaNRgP3cncEH6AIA9FgCQPGwA8AAAeRBTaNRgP3ckcEH4ZoA9DACQPEwA8AAAdxBTaNRgP3cmTDUgrh1DAABwQfUzgD0CAJA8TADwAAB3EFNo1GA/dyRwQfMzgDzeAJA8SAD/AAEsAL9r1xA9OJcgAAAAQEGjPMw3IK4eMwAAEFNo1GA/dx5wQfAAgDzIAJA8SADwAAB4EFNo1GA/dx1wQe7MgDy0AJA8SADwAAB4EFNo1GA/dx1MNyCuHyMAAHBB7ZmAPKAAkDxIAPAAAHgQU2jUYD93HHBB7GaAPIwAkDxMAPAAAHgQU2jUYD93GnBB7ACAPIwATDcgriATAACQPEgA8AAAeBBTaNRgP3cYcEHrmYA8cACQPEgA8AAAThBTaNRgP3cXcEHqzIA8cACQPEgA/z93X0w3IK4hewAAEFNo1GA/dxVwQeuZgDxIAJA8SAD/QaO7UAAAAGA/d15wQcZm8AABLQC/a9UQPTieIAAAAEBBo6zMOyCuIowAABBTaNRgP3cIcEH0zIA88ACQPHAA8AAAeBBTaNRgP3cHcEH0zIA83ACQPHAA8AAAdxBTaNRgP3cFTDsgriN7AABwQfIAgDzIAJA8cAD/P3dccEHGzPAAASwAv2vXED04oSAAAABAQaP8UAAAAGA/d2JwQcWZ/0FwAMw9IK4k8AAAEFNo1GA/dv5wQfrMgDzwAJA8VADwAAB3EFNo1GA/dvxwQfkzgDzcAJA8UADwAAB3EFNo1GA/dvtMPSCuJd4AAHBB9ZmAPMgAkDxIAPAAAHgQU2jUYD92/HBB8zOAPLQAkDxIAPAAAHoQU2jUYD92+XBB8ZmAPKAATD0gribQAACQPEgA8AAAdxBTaNRgP3b3cEHwAIA8jACQPEgA8AAAeRBTaNRgP3b1cEHvM4A8gACQPEgA/z04i0w9IK4oNwAAEFNo1GA/dvRwQfAAgDxwAJA8SADwAAB3EFNo1GA/dvNwQe8zgDxIAJA8SADwAAB4EFNo1GA/dvJMPSCuKSYAAHBB7maAPEgAkDxIAPAAAHkQU2jUYD928XBB75mAPEgAkDxIAPAAAHgQU2jUYD928XBB7zOAPCAATD0grioXAACQPEgA8AAAeBBTaNRgP3bucEHuAIA8IACQPEgA8AAAeBBTaNRgP3bucEHtmYA78ACQPEgA/0GkMShlHnsB7gAAUAAAAGA/d21wQcWZ8AABLAC/a+YQPTi6IAAAAEBBo/VQAAAAYD93cXBBxZnwAAEsAL9r5hA9OLo=" +
                                                          "#E/e" +
                                                          "#X/a=19738");

            JToken commandF = convertedObject["F"];
            commandF["f"].Value<string>().Should().Be("0");

            JToken commandC = convertedObject["C"];
            commandC["a"].Value<string>().Should().Be("3123");
            commandC["b"].Value<string>().Should().Be("17");

            JToken commandT = convertedObject["T"];
            commandT["s"].Value<string>().Should().Be("548285406");
            commandT["p"].Value<string>().Should().Be("00000000000000000000");

            JToken commandI = convertedObject["I"];
            commandI["n"].Value<string>().Should().Be("1152");
            commandI["s"].Value<string>().Should().Be("20");
            commandI["b"].Value<string>().Should().Be("99");
            commandI["f"].Value<string>().Should().Be("17.20");
            commandI["v"].Value<string>().Should().Be("+3.779");

            JToken commandO = convertedObject["O"];
            commandO["g"].Value<string>().Should().Be("39035");

            JToken commandB = convertedObject["B"];
            commandB["a"].Value<string>().Should().Be("TDEgrhmAAACQPIwA/z93cXBBxszwAAEsAL9r5xA9OJMgAAAAQEGis1AAAABgP3dwcEHHM/AAASwAv2voED04bcw0IK4Z0gAAEFNo1GA/d0pwQehmgAAAAJA8jAD/AAEtAL9r5BA9OJQgAAAAQEGimVAAAABgP3ducEHGzP9BaADMNSCuHFMAABBTaNRgP3cncEH6AIA9FgCQPGwA8AAAeRBTaNRgP3ckcEH4ZoA9DACQPEwA8AAAdxBTaNRgP3cmTDUgrh1DAABwQfUzgD0CAJA8TADwAAB3EFNo1GA/dyRwQfMzgDzeAJA8SAD/AAEsAL9r1xA9OJcgAAAAQEGjPMw3IK4eMwAAEFNo1GA/dx5wQfAAgDzIAJA8SADwAAB4EFNo1GA/dx1wQe7MgDy0AJA8SADwAAB4EFNo1GA/dx1MNyCuHyMAAHBB7ZmAPKAAkDxIAPAAAHgQU2jUYD93HHBB7GaAPIwAkDxMAPAAAHgQU2jUYD93GnBB7ACAPIwATDcgriATAACQPEgA8AAAeBBTaNRgP3cYcEHrmYA8cACQPEgA8AAAThBTaNRgP3cXcEHqzIA8cACQPEgA/z93X0w3IK4hewAAEFNo1GA/dxVwQeuZgDxIAJA8SAD/QaO7UAAAAGA/d15wQcZm8AABLQC/a9UQPTieIAAAAEBBo6zMOyCuIowAABBTaNRgP3cIcEH0zIA88ACQPHAA8AAAeBBTaNRgP3cHcEH0zIA83ACQPHAA8AAAdxBTaNRgP3cFTDsgriN7AABwQfIAgDzIAJA8cAD/P3dccEHGzPAAASwAv2vXED04oSAAAABAQaP8UAAAAGA/d2JwQcWZ/0FwAMw9IK4k8AAAEFNo1GA/dv5wQfrMgDzwAJA8VADwAAB3EFNo1GA/dvxwQfkzgDzcAJA8UADwAAB3EFNo1GA/dvtMPSCuJd4AAHBB9ZmAPMgAkDxIAPAAAHgQU2jUYD92/HBB8zOAPLQAkDxIAPAAAHoQU2jUYD92+XBB8ZmAPKAATD0gribQAACQPEgA8AAAdxBTaNRgP3b3cEHwAIA8jACQPEgA8AAAeRBTaNRgP3b1cEHvM4A8gACQPEgA/z04i0w9IK4oNwAAEFNo1GA/dvRwQfAAgDxwAJA8SADwAAB3EFNo1GA/dvNwQe8zgDxIAJA8SADwAAB4EFNo1GA/dvJMPSCuKSYAAHBB7maAPEgAkDxIAPAAAHkQU2jUYD928XBB75mAPEgAkDxIAPAAAHgQU2jUYD928XBB7zOAPCAATD0grioXAACQPEgA8AAAeBBTaNRgP3bucEHuAIA8IACQPEgA8AAAeBBTaNRgP3bucEHtmYA78ACQPEgA/0GkMShlHnsB7gAAUAAAAGA/d21wQcWZ8AABLAC/a+YQPTi6IAAAAEBBo/VQAAAAYD93cXBBxZnwAAEsAL9r5hA9OLo=");

            JToken commandE = convertedObject["E"];
            commandE["e"].Value<string>().Should().Be("");

            JToken commandX = convertedObject["X"];
            commandX["a"].Value<string>().Should().Be("19738");

            convertedObject.Count.Should().Be(8);
        }

        [Fact]
        // ReSharper disable InconsistentNaming
        public void WhenSomeRealisticMessagesAreConverted_ThenNoExceptionIsThrown()
        {
            // ARC-1 Data file (base64 / 5 measurements)
            // ReSharper disable UnusedVariable - Check that no exception is thrown does not need to process returned value
            string result_ARC1_Base64 = this._testee.Convert("#F/e=0#C/a=2148/b=2#T/s=549559786/p=17.05.31,17:30:25+08#M/a=-0.9658963-0.0001001+OFL+29.549683+OFL+0.9657700+31.139997+0.0013732+0.0012207/c=+1+1#I/n=112233/s=17/b=99/e=9.20/f=17.21/h=35/v=+3.555#B/a=CGIgwZ2jAAAg////QEHsZVD///9gP3c8cEH5HoA6tACQOqAA//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8=#E/e#X/a=30033").ToString();

            // ARC-1 Data file (plain Text / 5 measurements)
            string result_ARC1_Plain = this._testee.Convert("#F/a=0#T/s=549559125/p=17.05.31,17:18:31+08/m=549558984#M/b=-0.9661281-0.0000822+OFL+29.481324+OFL+0.9659800+30.570002+0.0010681+0.0008392-0.9660997-0.0002241+OFL+29.568237+OFL+0.9658999+30.610000+0.0014495+0.0012969-0.9660616-0.0002765+OFL+29.548704+OFL+0.9658599+30.610000+0.0015258+0.0014495-0.9662744-0.0004277+OFL+29.558347+OFL+0.9658700+30.570002+0.0016021+0.0014495-0.9660347-0.0001642+OFL+29.617066+OFL+0.9659100+30.520000+0.0013732+0.0014495/c=+1+1#I/n=112233/s=16/b=99/e=9.20/f=17.21/h=37/v=+3.572#G/a=This is a Measurement TEXT#E/e#X/a=25903").ToString();

            // ARC-1 Alarm file
            string result_ARC1_Alarm = this._testee.Convert("#F/c=0#T/s=549559152/p=17.05.31,17:18:31+08#M/a=-0.9660347-0.0001642+OFL+29.617066+OFL+0.9659100+30.520000+0.0013732+0.0014495/c=+1+1#I/n=112233/s=16/b=99/e=9.20/f=17.21/h=37/v=+3.572#G/b=This is a Alarm TEXT#E/e#X/a=43822").ToString();

            // ARC-1 Configuration file without Acknowledge (Email / FTP) (NO CRC, so it works)
            string result_ARC1_Config = this._testee.Convert("#F/d=0#T/s=549557930/p=17.05.31,16:59:30+08#M/a=-0.9665307-0.0004150+OFL+29.422731+OFL+0.9661099+30.579997+0.0009155+0.0016021/c=+1+1/d=-0.9665307-0.0004150+OFL+OFL+29.422731+OFL+0.9661099+30.579997+0.0009155+0.0016021#I/n=112233/s=17/b=99/e=9.20/f=17.21/h=36/v=+3.576#a/a=gprs.swisscom.ch/b=gprs/c=gprs/d=000.000.000.000/e=ARC-1/f=datamanager_103@gsmdata.ch/g=secretPassword/h=datamanager_103@gsmdata.ch/i=secretPassword/j=pop.gsmdata.ch/k=110/l=smtp.gsmdata.ch/m=25/n=p.schlegel@keller-pressure.com#b/a=p.schlegel@keller-pressure.com/b=p.schlegel@kel-pressureuck.ch/c=p.schlegel@-pressure-pressure.com/g=SMS Pass/j=1234/k=/m=+41792657769/n=+41792657769/o=+41792657769/q=+41794999000/r=Winterthur KellerAG/s=+41792657766/t=ARC-1 112233/u=This is a Measurement TEXT/v=This is a Alarm TEXT/w=This is a Check TEXT/0=8.7475439/1=47.4984244/2=450.000#c/a=549557902/b=549643500/c=549643500/d=549643500/e=549556500/g=60/h=86400/i=86400/j=86400/k=86400/m=247/n=0/o=5/p=3/q=5/r=7/s=1/t=2/v=6/w=4/x=8/y=15/z=240/0=0/1=0/2=0/3=0/4=4/5=10/6=5/7=0/8=0/9=0#f/a=549556500/g=86400/h=86400/m=0/n=0/o=0/q=5/z=8/3=3#d/a=+26.000000/b=+35.000000/c=+0.5000000/f=+1.0000000/g=+1.0000000/i=+0.0000000/j=+0.0000000/k=+0.0000000/m=+0.0000000/n=+50.000000/o=+400.00000/p=+0.0000000/q=+998.20000/r=+0.0000000/s=+90.000000/t=+0.0000000/u=+0.0000000/v=+0.0000000/w=+0.0000000/0=+8.7475440/1=+47.498425/2=+450.00000#k/a=ftp.gsmdata.ch/b=datamanager_103@gsmdata.ch/c=secretPassword/d=ARC-1/e=21/f=21/g=0/h=ARC-1#O/g=0#E/e").ToString();

            //ARC-1 Info file without Acknowledge (Email / FTP) (NO CRC, so it works)
            string result_ARC1_Info = this._testee.Convert("#F/b=0#T/s=549557171/p=17.05.31,16:46:50+08#M/a=-0.9617821+0.0043878+OFL+29.411865+OFL+0.9662000+30.320000+0.0012969+0.0012207/c=+1+1/d=-0.9617821+0.0043878+OFL+OFL+29.411865+OFL+0.9662000+30.320000+0.0012969+0.0012207#I/n=112233/s=17/b=99/e=9.20/f=17.21/h=36/v=+3.586#a/a=gprs.swisscom.ch/b=gprs/c=gprs/d=000.000.000.000/e=ARC-1/f=datamanager_103@gsmdata.ch/g=secretPassword/h=datamanager_103@gsmdata.ch/i=secretPassword/j=pop.gsmdata.ch/k=110/l=smtp.gsmdata.ch/m=25/n=p.schlegel@keller-pressure.com#b/a=p.schlegel@kel-pressureuck.ch/b=p.schlegel@-pressure-pressure.com/c=p.schleg-pressure-pressureuck.ch/g=SMS Pass/j=1234/k=/m=+41792657769/n=+41792657769/o=+41792657769/q=+41794999000/r=Winterthur KellerAG/s=+41792657766/t=ARC-1 112233/u=This is a Measurement TEXT/v=This is a Alarm TEXT/w=This is a Check TEXT/0=8.7475439/1=47.4984244/2=450.000#c/a=549557160/b=549643500/c=549643500/d=549643500/e=549556500/g=60/h=86400/i=86400/j=86400/k=86400/m=247/n=0/o=5/p=3/q=5/r=7/s=1/t=2/v=6/w=4/x=8/y=15/z=240/0=0/1=0/2=0/3=0/4=4/5=10/6=5/7=0/8=0/9=0#f/a=549556500/g=86400/h=86400/m=0/n=0/o=0/q=5/z=8/3=3#d/a=+26.000000/b=+35.000000/c=+0.5000000/f=+1.0000000/g=+1.0000000/i=+0.0000000/j=+0.0000000/k=+0.0000000/m=+0.0000000/n=+50.000000/o=+400.00000/p=+0.0000000/q=+998.20000/r=+0.0000000/s=+90.000000/t=+0.0000000/u=+0.0000000/v=+0.0000000/w=+0.0000000/0=+8.7475440/1=+47.498425/2=+450.00000#k/a=ftp.gsmdata.ch/b=datamanager_103@gsmdata.ch/c=secretPassword/d=ARC-1/e=21/f=21/g=0/h=ARC-1#E/e").ToString();

            // 3. Email or FTP File
            string result3 = this._testee.Convert("#F/f=0#C/a=3033/b=30#T/s=548285266/p=00000000000000000000#I/n=1152/s=20/b=99/f=17.20/v=+3.802#O/g=39035#B/a=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#E/e#X/a=33606").ToString();
            
            // 7. Email or FTP File
            string result7 = this._testee.Convert("#F/f=0#C/a=3123/b=17#T/s=548285406/p=00000000000000000000#I/n=1152/s=20/b=99/f=17.20/v=+3.779#O/g=39035#B/a=TDEgrhmAAACQPIwA/z93cXBBxszwAAEsAL9r5xA9OJMgAAAAQEGis1AAAABgP3dwcEHHM/AAASwAv2voED04bcw0IK4Z0gAAEFNo1GA/d0pwQehmgAAAAJA8jAD/AAEtAL9r5BA9OJQgAAAAQEGimVAAAABgP3ducEHGzP9BaADMNSCuHFMAABBTaNRgP3cncEH6AIA9FgCQPGwA8AAAeRBTaNRgP3ckcEH4ZoA9DACQPEwA8AAAdxBTaNRgP3cmTDUgrh1DAABwQfUzgD0CAJA8TADwAAB3EFNo1GA/dyRwQfMzgDzeAJA8SAD/AAEsAL9r1xA9OJcgAAAAQEGjPMw3IK4eMwAAEFNo1GA/dx5wQfAAgDzIAJA8SADwAAB4EFNo1GA/dx1wQe7MgDy0AJA8SADwAAB4EFNo1GA/dx1MNyCuHyMAAHBB7ZmAPKAAkDxIAPAAAHgQU2jUYD93HHBB7GaAPIwAkDxMAPAAAHgQU2jUYD93GnBB7ACAPIwATDcgriATAACQPEgA8AAAeBBTaNRgP3cYcEHrmYA8cACQPEgA8AAAThBTaNRgP3cXcEHqzIA8cACQPEgA/z93X0w3IK4hewAAEFNo1GA/dxVwQeuZgDxIAJA8SAD/QaO7UAAAAGA/d15wQcZm8AABLQC/a9UQPTieIAAAAEBBo6zMOyCuIowAABBTaNRgP3cIcEH0zIA88ACQPHAA8AAAeBBTaNRgP3cHcEH0zIA83ACQPHAA8AAAdxBTaNRgP3cFTDsgriN7AABwQfIAgDzIAJA8cAD/P3dccEHGzPAAASwAv2vXED04oSAAAABAQaP8UAAAAGA/d2JwQcWZ/0FwAMw9IK4k8AAAEFNo1GA/dv5wQfrMgDzwAJA8VADwAAB3EFNo1GA/dvxwQfkzgDzcAJA8UADwAAB3EFNo1GA/dvtMPSCuJd4AAHBB9ZmAPMgAkDxIAPAAAHgQU2jUYD92/HBB8zOAPLQAkDxIAPAAAHoQU2jUYD92+XBB8ZmAPKAATD0gribQAACQPEgA8AAAdxBTaNRgP3b3cEHwAIA8jACQPEgA8AAAeRBTaNRgP3b1cEHvM4A8gACQPEgA/z04i0w9IK4oNwAAEFNo1GA/dvRwQfAAgDxwAJA8SADwAAB3EFNo1GA/dvNwQe8zgDxIAJA8SADwAAB4EFNo1GA/dvJMPSCuKSYAAHBB7maAPEgAkDxIAPAAAHkQU2jUYD928XBB75mAPEgAkDxIAPAAAHgQU2jUYD928XBB7zOAPCAATD0grioXAACQPEgA8AAAeBBTaNRgP3bucEHuAIA8IACQPEgA8AAAeBBTaNRgP3bucEHtmYA78ACQPEgA/0GkMShlHnsB7gAAUAAAAGA/d21wQcWZ8AABLAC/a+YQPTi6IAAAAEBBo/VQAAAAYD93cXBBxZnwAAEsAL9r5hA9OLo=#E/e#X/a=19738").ToString();

            // 8. Email or FTP File / GSM-2 Configuration file with Acknowledge -> from Datamanager (Email / FTP) 
            string result8 = this._testee.Convert("#F/d=0#T/s=548285443/p=00000000000000000000#M/a=+0.0000000+0.9644575+29.550000+0.0000000+0.0000000/c=+0+0/d=+0.0000000+0.0000000+0.0000000+0.0000000+0.0000000+0.0000000+0.9644575+29.550000+0.0000000+0.0000000#I/n=1152/s=20/b=99/f=17.20/v=+3.779#a/a=gprs.swisscom.ch/b=gprs/c=gprs/d=000.000.000.000/e=GSM2/f=datamanager_103@gsmdata.ch/g=secretPassword/h=datamanager_103@gsmdata.ch/i=secretPassword/j=pop.gsmdata.ch/k=110/l=smtp.gsmdata.ch/m=25/n=datamanager_103@gsmdata.ch#b/a=datamanager_103@gsmdata.ch/b=datamanager_103@gsmdata.ch/c=datamanager_103@gsmdata.ch/g=keller/j=0000/k=+41792657769/m=+41792657769/n=+41792657769/o=+41792657769/q=+41794999000/r=Winterthur KellerAG/s=+467190000184941/t=GSM-2 SN 1152/u=Messung Text/v=Alarm Textycvymxvcyx.mvyx.cvmyx.vmyx.cvmcyx.vmyx.v/w=FTP CHECK/0=8.5/1=42/2=10.000#c/a=548285306/b=544202351/c=544202934/d=548286180/e=544284534/g=120/h=60/i=600/j=1800/k=86400/m=194/n=0/o=1/p=3/q=10/r=1/s=2/t=1/v=6/w=6/x=8/y=9/z=246/0=0/1=0/2=0/3=0/4=1/5=10/6=5/7=0/8=0/9=0#f/a=544193100/g=120/h=60/m=0/n=1/o=10/q=10/z=0/3=0#d/a=+0.5000000/b=+0.4000000/c=+0.1000000/f=+1.0000000/g=+1.0000000/i=+0.1000000/j=+0.0900000/k=+0.1000000/m=+0.0000000/n=+4.8639999/o=+2.5000000/p=+0.0000000/q=+998.20000/r=+1.0000000/s=+0.0000000/t=+1.0000000/u=+0.0000000/v=+10.000000/w=+11.000000/0=+8.5000000/1=+42.000000/2=+10.000000#k/a=ftp.gsmdata.ch/b=datamanager_103@gsmdata.ch/c=secretPassword/d=GSMProduktion/e=21/f=21/g=2000/h=GSM2#O/g=39035#E/e").ToString();

            // ReSharper restore UnusedVariable
        }
    }
}