namespace GsmCommunicationToJson
{
    using System.Text;
    using System.Text.RegularExpressions;
    using Exceptions;
    using Newtonsoft.Json.Linq;

    public class GsmCommunicationToJsonConverter
    {
        private static readonly string CommandPatternExpression = @"((#(?<command>B+)(?<parameters>/[a-z0-9]+\=?[/\+\=A-Za-z0-9@&]*))|#(?<command>[AC-Za-z]+)(?<parameters>\/[A-Za-z0-9]+\=?[_\s\!\?\s\:\,\-\+\.A-Za-z0-9@\\%<>\*$\[\]^_{}()~]*)*)";

        /// <summary>
        /// This regex is used for validation of the incoming GSM message. This string was copied from GSM2Parser.pas of 
        /// the delphi implementation and modified Around ')~]*)*))+(#E/e)' to contain a plus sign instead of an asterisk.
        /// </summary>
        private static readonly string ValidationExpression = @"((#B+/[a-z0-9]+\=?[/\+\=A-Za-z0-9@]*)|(#[A-Za-z]+(/[a-z0-9]+\=?[_\s\!\?\s\:\,\-\+\.A-Za-z0-9@\\%<>\*$\[\]^_{}()~]*)*))+(#E/e)?(#X/a\=\d{5})?";

        private const string CrcPattern = @"(?<restOfString>.*?)#X/a=(?<crcValue>[0-9]+)";

        private const string ParameterMatchingPattern = @"(?<parameterName>[A-Za-z0-9]+)\=?(?<parameterValue>[_\s\!\?\s\:\,\-\+\.A-Za-z0-9@&\\%<>\*$\[\]^_{}()~]*)";
        private const string ParameterMatchingPatternForBase64 = @"(?<parameterName>[A-Za-z0-9]+)\=?(?<parameterValue>[/\+\=A-Za-z0-9@]*)";
        private const string Base64EncodingCommand = "B";

        private readonly Crc16Modbus _crc16Modbus = new Crc16Modbus();

        public JObject Convert(string message)
        {
            // Due to SMTP restrictions a line can only have a certain max size (~1000bytes or 2000bytes depending on SMTP service extensions or whatever green.ch does) so sometimes the message is truncated with <CRLF>
            message = message.Replace("\n", "").Replace("\r", "");

            if (string.IsNullOrEmpty(message))
            {
                throw new EmptyGsmMessageException("Message was null or empty.");
            }

            if (Regex.IsMatch(message, ValidationExpression) == false)
            {
                throw new InvalidGsmMessageFormatException($"The Message was in invalid format (see member {nameof(InvalidGsmMessageFormatException.MessageWithInvalidFormat)}).", message);
            }

            Match crcMatch = Regex.Match(message, CrcPattern);
            if (crcMatch.Success)
            {
                int crcValue = int.Parse(crcMatch.Groups["crcValue"].Value);
                string restOfString = crcMatch.Groups["restOfString"].Value;

                ushort computedChecksum = this._crc16Modbus.ComputeChecksum(Encoding.ASCII.GetBytes(restOfString));

                if (crcValue != computedChecksum)
                {
                    throw new GsmMessageChecksumMismatchException($"CRC16 check failed (expected: {crcValue}, computed: {computedChecksum})", message);
                }
            }

            MatchCollection matchCollection = Regex.Matches(message, CommandPatternExpression);

            JObject conversionResult = new JObject();

            foreach (Match match in matchCollection)
            {
                string commandCharacters = match.Groups["command"].Value;

                JObject commandJObject = new JObject();
                conversionResult.Add(commandCharacters, commandJObject);

                foreach (Capture capture in match.Groups["parameters"].Captures)
                {
                    // Retrieve parameterString (may contain '=' plus additional value or not)
                    string parameterString = capture.Value;

                    Match parameterMatch = Regex.Match(parameterString, commandCharacters.Equals(Base64EncodingCommand) ? ParameterMatchingPatternForBase64 : ParameterMatchingPattern);

                    commandJObject.Add(parameterMatch.Groups["parameterName"].Value, parameterMatch.Groups["parameterValue"].Value);
                }
            }

            return conversionResult;
        }
    }
}