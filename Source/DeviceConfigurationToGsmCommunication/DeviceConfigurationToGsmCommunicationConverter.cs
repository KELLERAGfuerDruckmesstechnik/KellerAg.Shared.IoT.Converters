using System;
using System.Text;
using System.Text.RegularExpressions;
using DeviceConfigurationToGsmCommunication.Exceptions;
using KellerAg.Shared.Entities.Database;

namespace DeviceConfigurationToGsmCommunication
{
    public class DeviceConfigurationToGsmCommunicationConverter
    {
        private readonly Regex _unsupportedCharacterRegex = new Regex(@"[^\x20-\x7A]");
        private const int MaxAckNumber = 65000;

        public string Convert(DeviceSettings config)
        {
            var builder = new StringBuilder();

            ExtractGprsSettings(config, ref builder);
            ExtractTextNumberAndAddresses(config, ref builder);
            ExtractMeasurementSettings(config, ref builder);
            ExtractFloatingPointValueSettings(config, ref builder);
            ExtractMeasurementSettings2(config, ref builder);
            //ExtractFtpSettings(config, ref builder); These fields will be read-only. They should never go down to the device.

            if (builder.Length == 0)
            {
                throw new EmptyGsmMessageException();
            }

            int fileAckNumber = config.Id % MaxAckNumber; //Let's use the Row Id in the table but do not go over 65000 as the GSM protocol doesn't allow more

            builder.Append($"#O/f={fileAckNumber}"); //Add File Ack Number that GSM/ARC must send back
            builder.Append("#E/e"); //Add End of File
            return builder.ToString();
        }

        private void ExtractGprsSettings(DeviceSettings config, ref StringBuilder sb)
        {
            const string accountSettingsIdFieldName = "#a";

            var newSbParts = new StringBuilder();
            /*
             These fields will be read-only. They should never go down to the device.
            ExtractField("/a=", config.GprsAPN, 50, ref newSbParts);
            ExtractField("/b=", config.GprsID, 50, ref newSbParts);
            ExtractField("/c=", config.GprsPassword, 50, ref newSbParts);
            ExtractField("/d=", config.GprsDNS, 50, ref newSbParts);
            */

            ExtractField("/e=", config.MailSmtpShowedName, 50, ref newSbParts); // I think this is a identifier text which essentially does not have a real purpose

            /*
             These fields will be read-only. They should never go down to the device.
            //ExtractField("/f=", config.MailPop3Username, 50, ref newSbParts);
            //ExtractField("/g=", config.MailPop3Password, 50, ref newSbParts);
            //ExtractField("/h=", config.MailOptSmtpUsername, 50, ref newSbParts);
            //ExtractField("/i=", config.MailOptSmtpPassword, 50, ref newSbParts);
            //ExtractField("/j=", config.MailPop3Server, 50, ref newSbParts);
            //ExtractField("/k=", config.MailPop3Port, 0, 65535, ref newSbParts);
            //ExtractField("/l=", config.MailSmtpServer, 50, ref newSbParts);
            //ExtractField("/m=", config.MailSmtpPort, 0, 65535, ref newSbParts);
            //ExtractField("/n=", config.MailReturnAddress, 50, ref newSbParts);
            */

            ExtractField("/o=", config.CellularModuleId               , 30, ref newSbParts);
            ExtractField("/p=", config.CellularModuleRevisionId       , 30, ref newSbParts);
            ExtractField("/q=", config.CellularModuleSerialNumberIMEI , 30, ref newSbParts);
            ExtractField("/r=", config.CellularSIMCardId              , 30, ref newSbParts);
            ExtractField("/s=", config.CellularSIMCardSubscriberId    , 30, ref newSbParts);

            if (newSbParts.Length > 0)
            {
                newSbParts.Insert(0, accountSettingsIdFieldName);
                sb.Append(newSbParts);
            }
        }

        private void ExtractTextNumberAndAddresses(DeviceSettings config, ref StringBuilder sb)
        {
            const string accountSettingsIdFieldName = "#b";

            var newSbParts = new StringBuilder();

            ExtractField("/a=", config.MeasurementMailAddress, 50, ref newSbParts);
            ExtractField("/b=", config.AlarmMailAddress, 50, ref newSbParts);
            ExtractField("/c=", config.InfoMailAddress, 50, ref newSbParts);
            //ExtractField("/g=", config.GprsPasswordForQuerySms, 10, ref newSbParts);  These fields will be read-only. They should never go down to the device.
            //ExtractField("/j=", config.GprsSimPin, 10, ref newSbParts); These fields will be read-only. They should never go down to the device.
            ExtractField("/k=", config.HardwareDataConnectionCallNumber, 30, ref newSbParts);
            ExtractField("/m=", config.MeasurementSmsNumber, 30, ref newSbParts);
            ExtractField("/n=", config.AlarmSmsNumber, 30, ref newSbParts);
            ExtractField("/o=", config.InfoSmsNumber, 30, ref newSbParts);
            //ExtractField("/q=", config.GprsSmsServiceCenterNr, 30, ref newSbParts); These fields will be read - only.They should never go down to the device.
            ExtractField("/r=", config.GeneralNetworkName, 20, ref newSbParts);
            ExtractField("/s=", config.GeneralOwnTelNumber, 30, ref newSbParts);
            ExtractField("/t=", config.GeneralLocationName, 20, ref newSbParts);
            ExtractField("/u=", config.MeasurementSmsText, 160, ref newSbParts);
            ExtractField("/v=", config.AlarmSmsText, 160, ref newSbParts);
            ExtractField("/w=", config.CheckAnswerSmsText, 160, ref newSbParts);
            ExtractField("/0=", config.GeneralLongitudeText, 20, ref newSbParts);
            ExtractField("/1=", config.GeneralLatitudeText, 20, ref newSbParts);
            ExtractField("/2=", config.GeneralAltitudeText, 20, ref newSbParts);

            // /3= - /5= are read-only

            if (newSbParts.Length > 0)
            {
                newSbParts.Insert(0, accountSettingsIdFieldName);
                sb.Append(newSbParts);
            }
        }

        private void ExtractMeasurementSettings(DeviceSettings config, ref StringBuilder sb)
        {
            const string accountSettingsIdFieldName = "#c";

            var newSbParts = new StringBuilder();
            
            ExtractField("/a=", config.MeasurementTimer, 0, 1100000000, ref newSbParts);

            // These fields will be read-only. They should never go down to the device.
            //ExtractField("/b=", config.AlarmTimer, 0, 1100000000, ref newSbParts);
            //ExtractField("/c=", config.InfoTimer, 0, 1100000000, ref newSbParts);
            //ExtractField("/d=", config.CheckTimer, 0, 1100000000, ref newSbParts);
            //ExtractField("/e=", config.HardwareDataConnectionTimer, 0, 1100000000, ref newSbParts);

            ExtractField("/g=", config.MeasurementInterval, 1, 2592000, ref newSbParts);
            ExtractField("/h=", config.AlarmInterval, 1, 2592000, ref newSbParts);
            ExtractField("/i=", config.InfoInterval, 1, 2592000, ref newSbParts);
            ExtractField("/j=", config.CheckInterval, 1, 2592000, ref newSbParts);
            ExtractField("/k=", config.HardwareDataConnectionInterval, 1, 2592000, ref newSbParts);
            ExtractField("/m=", new bool?[] {
                config.HardwareMeasureSaveChannel0,
                config.HardwareMeasureSaveChannel1,
                config.HardwareMeasureSaveChannel2,
                config.HardwareMeasureSaveChannel3,
                config.HardwareMeasureSaveChannel4,
                config.HardwareMeasureSaveChannel5,
                config.HardwareMeasureSaveChannel6,
                config.HardwareMeasureSaveChannel7
            }, ref newSbParts);
            ExtractField("/o=", config.MeasurementSendSmsAfterX, ref newSbParts);
            ExtractField("/p=", new bool?[] {
                config.HardwareMeasureSaveChannel8,
                config.HardwareMeasureSaveChannel9,
                config.HardwareMeasureSaveChannel10,
                config.HardwareMeasureSaveChannel11,
                config.HardwareMeasureSaveChannel12,
                config.HardwareMeasureSaveChannel13,
                config.HardwareMeasureSaveChannel14,
                config.HardwareMeasureSaveChannel15
            }, ref newSbParts);
            ExtractField("/q=", config.MeasurementSendMailAfterX, ref newSbParts);
            ExtractField("/r=", config.AlarmChannelNumber, ref newSbParts);
            ExtractField("/s=", config.AlarmType, ref newSbParts);
            ExtractField("/t=", config.AlarmSendXTimes, ref newSbParts);
            ExtractField("/v=", config.HardwareResolutionPressureChannels, ref newSbParts);
            ExtractField("/w=", config.HardwareResolutionTemperatureChannels, ref newSbParts);
            ExtractField("/x=", new bool?[] {
                config.LockTimerMeasurement,
                config.LockTimerAlarm,
                config.LockTimerInfo,
                config.LockTimerCheck,
                config.LockTimerDial
            }, ref newSbParts);
            ExtractField("/y=", new bool?[] {
                config.LockTimerMeasurement,
                config.LockTimerAlarm,
                config.LockTimerInfo,
                config.LockTimerCheck,
                config.LockTimerDial
            }, ref newSbParts);
            ExtractField("/z=", new bool?[] {
                config.MeasurementSendSms,
                config.AlarmSendSms,
                config.InfoSendSms,
                config.CheckSendSms,
                config.MeasurementSendMail,
                config.AlarmSendMail,
                config.InfoSendMail,
                config.CheckSendMail
            }, ref newSbParts);
            ExtractField("/0=", config.GprsModemProtocol, ref newSbParts);
            //ExtractField("/1=", new bool?[] { config.MailSmtpUseSSL, config.MailPop3UseSSL }, ref newSbParts); //These fields will be read-only. They should never go down to the device.
            ExtractField("/2=", config.MailUseAlternativeSMTPLogin, ref newSbParts);
            ExtractField("/3=", config.WaterLevelCalculationFormType, ref newSbParts);
            ExtractField("/4=", config.HardwarePowerExternalDevice, ref newSbParts);
            ExtractField("/5=", config.HardwareSupportedSensorTypes, ref newSbParts);
            ExtractField("/6=", config.HardwareConnectionType, ref newSbParts);
            ExtractField("/7=", config.WaterLevelCalculationIsAbsoluteSensor, ref newSbParts);
            ExtractField("/8=", config.WaterLevelCalculationCalculateFrom, ref newSbParts);
            ExtractField("/9=", config.WaterLevelCalculationConversionTo, ref newSbParts);
            /*
             These fields will be read-only. They should never go down to the device.
            ExtractField("/A=", config.FtpUseTLS, ref newSbParts);
            ExtractField("/B=", config.FtpUseActiveMode, ref newSbParts);
            */

            if (newSbParts.Length > 0)
            {
                newSbParts.Insert(0, accountSettingsIdFieldName);
                sb.Append(newSbParts);
            }
        }

        private void ExtractFloatingPointValueSettings(DeviceSettings config, ref StringBuilder sb)
        {
            const string accountSettingsIdFieldName = "#d";

            var newSbParts = new StringBuilder();

            ExtractField("/a=", config.AlarmOnThreshold, ref newSbParts);
            ExtractField("/b=", config.AlarmOffThreshold, ref newSbParts);
            ExtractField("/c=", config.AlarmDeltaThreshold, ref newSbParts);
            ExtractField("/f=", config.HardwareMultiplierTemperatureChannels, ref newSbParts);
            ExtractField("/g=", config.HardwareMultiplierPressureChannels, ref newSbParts);
            ExtractField("/i=", config.EventOnValueThreshold, ref newSbParts);
            ExtractField("/j=", config.EventOffValueThreshold, ref newSbParts);
            ExtractField("/k=", config.EventDeltaValueThreshold, ref newSbParts);
            ExtractField("/m=", config.WaterLevelCalculationEnable, ref newSbParts);
            ExtractField("/n=", config.WaterLevelCalculationLength, ref newSbParts);
            ExtractField("/o=", config.WaterLevelCalculationHeight, ref newSbParts);
            ExtractField("/p=", config.WaterLevelCalculationOffset, ref newSbParts);
            ExtractField("/q=", config.WaterLevelCalculationDensity, ref newSbParts);
            ExtractField("/r=", config.WaterLevelCalculationWidth, ref newSbParts);
            ExtractField("/s=", config.WaterLevelCalculationAngle, ref newSbParts);
            ExtractField("/t=", config.WaterLevelCalculationFormFactor, ref newSbParts);
            ExtractField("/u=", config.WaterLevelCalculationMinCalcHeight, ref newSbParts);

            // Let's ignore /v /w for now

            ExtractField("/0=", config.GeneralGpsLongitude, ref newSbParts);
            ExtractField("/1=", config.GeneralGpsLatitude, ref newSbParts);
            ExtractField("/2=", config.GeneralGpsAltitude, ref newSbParts);

            if (newSbParts.Length > 0)
            {
                newSbParts.Insert(0, accountSettingsIdFieldName);
                sb.Append(newSbParts);
            }
        }

        private void ExtractMeasurementSettings2(DeviceSettings config, ref StringBuilder sb)
        {
            const string accountSettingsIdFieldName = "#f";

            var newSbParts = new StringBuilder();

            // ExtractField("/a=", config.EventMeasureTimer, 0, 5184000, ref newSbParts);
            ExtractField("/g=", config.EventCheckInterval, 1, 2592000, ref newSbParts);
            ExtractField("/h=", config.EventMeasureInterval, 1, 2592000, ref newSbParts);
            ExtractField("/m=", config.EventChannel, ref newSbParts);
            ExtractField("/n=", config.EventType, ref newSbParts);

            ExtractField("/o=", config.EventSendMailXTimes, 1, 30, ref newSbParts);
            ExtractField("/q=", config.MeasurementSendFtpAfterX, ref newSbParts);
            ExtractField("/z=", new bool?[] {
                config.MeasurementSendFTP,
                config.AlarmSendFTP,
                config.InfoSendFTP,
                config.CheckSendFTP
            }, ref newSbParts);
            ExtractField("/3=", config.HardwarePreOnTime, 0, 254, ref newSbParts);

            if (newSbParts.Length > 0)
            {
                newSbParts.Insert(0, accountSettingsIdFieldName);
                sb.Append(newSbParts);
            }
        }

        /// <summary>
        /// These fields will be read-only. They should never go down to the device.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sb"></param>
        private void ExtractFtpSettings(DeviceSettings config, ref StringBuilder sb)
        {
            const string ftpSettingsIdFieldName = "#k";

            var newSbParts = new StringBuilder();

            ExtractField("/a=", config.FtpServerName, 50, ref newSbParts);
            ExtractField("/b=", config.FtpUsername, 50, ref newSbParts);
            ExtractField("/c=", config.FtpPassword, 50, ref newSbParts);
            ExtractField("/d=", config.FtpAccount, 50, ref newSbParts);
            ExtractField("/e=", config.FtpSourceControlPort, 0, 65535, ref newSbParts);
            ExtractField("/f=", config.FtpPort, 0, 65535, ref newSbParts);
            ExtractField("/g=", config.FtpSourceDataPort, 0, 65535, ref newSbParts);
            ExtractField("/h=", config.FtpServerPath, 50, ref newSbParts);

            if (newSbParts.Length > 0)
            {
                newSbParts.Insert(0, ftpSettingsIdFieldName);
                sb.Append(newSbParts);
            }
        }

        private bool IsFieldNull(object field)
        {
            return field == null;
        }

        /// <summary>
        /// Use it like this:  ExtractField("/2=", config.MailUseAlternativeSMTPLogin, ref newSbParts);
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="sb"></param>
        private void ExtractField(string id, bool? field, ref StringBuilder sb)
        {
            if (IsFieldNull(field))
            {
                return;
            }
            sb.Append(id);
            sb.Append((bool)field ? 1 : 0);
        }

        /// <summary>
        /// Use it like this:  ExtractField("/o=", config.MeasurementSendSmsAfterX, ref newSbParts);
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="sb"></param>
        private void ExtractField(string id, byte? field, ref StringBuilder sb)
        {
            if (IsFieldNull(field))
            {
                return;
            }
            sb.Append(id);
            sb.Append(field);
        }

        /// <summary>
        /// Use it like this:  ExtractField("/d=", config.CheckTimer, ref newSbParts);
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="sb"></param>
        private void ExtractField(string id, int? field, int min, int max, ref StringBuilder sb)
        {
            if (IsFieldNull(field))
            {
                return;
            }

            if (field < min || field > max)
            {
                throw new OverflowException();
            }

            sb.Append(id);
            sb.Append(field);
        }

        /// <summary>
        /// Use it like this:  ExtractField("/c=", config.GprsPassword, 50, ref newSbParts);
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="maxLength"></param>
        /// <param name="sb"></param>
        private void ExtractField(string id, string field, int maxLength, ref StringBuilder sb)
        {
            if (string.IsNullOrEmpty(field))
            {
                return;
            }

            if (field.Length > maxLength)
            {
                throw new CharValueTooLongException();
            }

            if (_unsupportedCharacterRegex.IsMatch(field))
            {
                throw new UnsupportedCharException();
            }

            sb.Append(id);
            sb.Append(field);
        }
        
        /// <summary>
        /// Use it like this:  ExtractField("/r=", config.WaterLevelCalculationWidth, ref newSbParts);
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="sb"></param>
        private void ExtractField(string id, decimal? field, ref StringBuilder sb)
        {
            if (IsFieldNull(field))
            {
                return;
            }

            if (field >= 999999999.5m || field <= -999999999.5m)
            {
                throw new DecimalTooManyDigitsException();
            }

            string fieldStr = ((decimal)field).ToString("0.00000000");
            int beforeDecimalLength = fieldStr.IndexOf(".");

            if (field < 0)
            {
                beforeDecimalLength--;
            }

            decimal roundedValue = (decimal)field;

            if (beforeDecimalLength <= 7 && beforeDecimalLength > 0) {
                roundedValue = Math.Round((decimal)field, 8 - beforeDecimalLength, MidpointRounding.AwayFromZero);
            }

            sb.Append(id);
            sb.Append(roundedValue >= 0 ? "+" : "-");

            var fieldFloatText = Math.Abs(roundedValue).ToString("0.00000000").Substring(0, 9);
            sb.Append(fieldFloatText);
        }

        /// <summary>
        /// Use it like this:  ExtractField("/z=", new bool?[] {
        ///    config.MeasurementSendFTP,
        ///    config.AlarmSendFTP,
        ///    config.InfoSendFTP,
        ///    config.CheckSendFTP
        /// }, ref sb);
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fields"></param>
        /// <param name="sb"></param>
        private void ExtractField(string id, bool?[] fields, ref StringBuilder sb)
        {
            bool isEveryFieldNull = true;
            int value = 0;

            for (int i = 0; i < fields.Length; i++)
            {
                isEveryFieldNull &= fields[i] == null;
                bool fieldBool = fields[i] != null && fields[i] == true;
                value = (value | (fieldBool ? 1 : 0) << i);
            }

            if (!isEveryFieldNull)
            {
                sb.Append(id);
                sb.Append(value);
            }
        }
    }
}
