using System;
using JsonToBusinessObjects.DataContainers;
using JsonToBusinessObjects.DataContainers.Configuration;
using KellerAg.Shared.Entities.Database;

namespace KellerAg.Shared.IoT.Converters
{
    public static class BusinessObjectToDeviceConfiguration
    {
        public static DeviceSettings CreateConfiguration(
            string deviceSerialNumber,
            GprsSettings gprsSettings,
            TextEmailSmsLocSettings textEmailSmsLocSettings,
            MeasurementSettings measurementSettings,
            FloatingPointMeasurementSettings floatingPointMeasurementSettings,
            FtpSettings ftpSettings,
            MeasurementSettings2 measurementSettings2)
        {
            var configurationEntity = new DeviceSettings
            {
                CreationDateTime   = DateTime.UtcNow,
                State              = SendState.ConfirmedActiveness,
                IsSentFromAPI      = false,
                UniqueSerialNumber = string.IsNullOrEmpty(deviceSerialNumber) ? "unknown": deviceSerialNumber,
                //DESCRIPTION -> See Device.Note

                //LockTimerOnlyCheck / Bit 3 -> This is for write. WHICH MUST BE ALWAYS TRUE IN KOLIBRI
                //Val110Fu3031Index20?  (Reserve)
                //Val109Fu3031Index19?  (Reserve)

                LockTimerMeasurement = (measurementSettings?.LockTimersWithoutCheck & (1 << 0)) != 0, //bit position 0 / SHOULD BE TRUE
                LockTimerAlarm       = (measurementSettings?.LockTimersWithoutCheck & (1 << 1)) != 0, //bit position 1
                LockTimerInfo        = (measurementSettings?.LockTimersWithoutCheck & (1 << 2)) != 0, //bit position 2
                LockTimerCheck       = (measurementSettings?.LockTimersWithoutCheck & (1 << 3)) != 0, //bit position 3 / SHOULD BE TRUE. Always!
                LockTimerDial        = (measurementSettings?.LockTimersWithoutCheck & (1 << 4)) != 0, //bit position 4 / SHOULD BE FALSE

                GeneralNetworkName               = textEmailSmsLocSettings?.NetworkName,
                GeneralOwnTelNumber              = textEmailSmsLocSettings?.OwnTelNumber,
                GeneralLocationName              = textEmailSmsLocSettings?.LocationName,
                //GeneralLocationAddressText     = //DB only
                GeneralGsmSerialNumber           = null,
                GeneralGsmSoftwareVersion        = null,
                GeneralLongitudeText             = textEmailSmsLocSettings?.LongitudeText,
                GeneralLatitudeText              = textEmailSmsLocSettings?.LatitudeText,
                GeneralAltitudeText              = textEmailSmsLocSettings?.AltitudeText,
                GeneralGpsLongitude              = (decimal?)(floatingPointMeasurementSettings?.LongitudeFu3031Index24),
                GeneralGpsLatitude               = (decimal?)(floatingPointMeasurementSettings?.LatitudeFu3031Index45),
                GeneralGpsAltitude               = (decimal?)(floatingPointMeasurementSettings?.AltitudeFu3031Index26),
                GeneralCellLocateLongitudeText   = textEmailSmsLocSettings?.CellLocateLongitude,
                GeneralCellLocateLatitudeText    = textEmailSmsLocSettings?.CellLocateLatitude,
                GeneralCellLocateAltitudeText    = textEmailSmsLocSettings?.CellLocateAltitude,

                HardwareConnectionType       = measurementSettings?.ConnectionType,
                HardwarePowerExternalDevice  = measurementSettings?.PowerExternalDevice,
                HardwareMeasureSaveChannel0  = (measurementSettings?.MeasureAndSaveCH0_7 & (1 << 0)) != 0,
                HardwareMeasureSaveChannel1  = (measurementSettings?.MeasureAndSaveCH0_7 & (1 << 1)) != 0,
                HardwareMeasureSaveChannel2  = (measurementSettings?.MeasureAndSaveCH0_7 & (1 << 2)) != 0,
                HardwareMeasureSaveChannel3  = (measurementSettings?.MeasureAndSaveCH0_7 & (1 << 3)) != 0,
                HardwareMeasureSaveChannel4  = (measurementSettings?.MeasureAndSaveCH0_7 & (1 << 4)) != 0,
                HardwareMeasureSaveChannel5  = (measurementSettings?.MeasureAndSaveCH0_7 & (1 << 5)) != 0,
                HardwareMeasureSaveChannel6  = (measurementSettings?.MeasureAndSaveCH0_7 & (1 << 6)) != 0,
                HardwareMeasureSaveChannel7  = (measurementSettings?.MeasureAndSaveCH0_7 & (1 << 7)) != 0,
                HardwareMeasureSaveChannel8  = (measurementSettings?.MeasureAndSaveCH8_15 & (1 << 0)) != 0,
                HardwareMeasureSaveChannel9  = (measurementSettings?.MeasureAndSaveCH8_15 & (1 << 1)) != 0,
                HardwareMeasureSaveChannel10 = (measurementSettings?.MeasureAndSaveCH8_15 & (1 << 2)) != 0,
                HardwareMeasureSaveChannel11 = (measurementSettings?.MeasureAndSaveCH8_15 & (1 << 3)) != 0,
                HardwareMeasureSaveChannel12 = (measurementSettings?.MeasureAndSaveCH8_15 & (1 << 4)) != 0,
                HardwareMeasureSaveChannel13 = (measurementSettings?.MeasureAndSaveCH8_15 & (1 << 5)) != 0,
                HardwareMeasureSaveChannel14 = (measurementSettings?.MeasureAndSaveCH8_15 & (1 << 6)) != 0,
                HardwareMeasureSaveChannel15 = (measurementSettings?.MeasureAndSaveCH8_15 & (1 << 7)) != 0,
                HardwareDataConnectionTimer  = (int?)measurementSettings?.Timer4DataConnection,
                HardwareDataConnectionInterval = (int?)measurementSettings?.Interval4DataConnection,
                HardwareSupportedSensorTypes = measurementSettings?.SupportedConnectionTypes,
                HardwarePreOnTime = (int?)measurementSettings2?.HardwarePreOnTimeInSeconds,
                HardwareMultiplierPressureChannels = (decimal?)floatingPointMeasurementSettings?.MultiplierPressureChannels,
                HardwareMultiplierTemperatureChannels = (decimal?)floatingPointMeasurementSettings?.MultiplierTemperatureChannels,
                HardwareResolutionPressureChannels = measurementSettings?.ResolutionPressureChannels,
                HardwareResolutionTemperatureChannels = measurementSettings?.ResolutionTemperatureChannels,
                HardwareDataConnectionCallNumber = textEmailSmsLocSettings?.RecallForDataConnection,

                MeasurementTimer = (int?)measurementSettings?.Timer0Measure,
                MeasurementInterval = (int?)measurementSettings?.Interval0Measure,
                MeasurementSendSms = (measurementSettings?.SendSmsEmail & (1 << 0)) != 0,
                MeasurementSendMail = (measurementSettings?.SendSmsEmail & (1 << 4)) != 0,
                MeasurementSendFTP = (measurementSettings2?.SendTypeFtp & (1 << 0)) != 0,//BitPos 0
                MeasurementMailAddress = textEmailSmsLocSettings?.EmailAddress1,
                MeasurementSmsNumber = textEmailSmsLocSettings?.SmsNumber1Measure,
                MeasurementSmsText = textEmailSmsLocSettings?.SmsText1Measure,
                MeasurementSendMailAfterX = measurementSettings?.SendMailAfterXMeasurements,
                MeasurementSendSmsAfterX = measurementSettings?.SendSmsAfterXMeasurements,
                MeasurementSendFtpAfterX = measurementSettings2?.SendToFtpAfterXCollectedMeasurements, //Let's not do this anymore in KOLIBRI

                GprsAPN                        = gprsSettings?.GprsAPN,
                GprsID                         = gprsSettings?.GprsID,
                GprsPassword                   = gprsSettings?.GprsPassword,
                GprsDNS                        = gprsSettings?.GprsDNS,
                GprsSimPin                     = textEmailSmsLocSettings?.SimPin,
                GprsSmsServiceCenterNr         = textEmailSmsLocSettings?.SmsServiceCenterNr,
                GprsPasswordForQuerySms        = textEmailSmsLocSettings?.PasswordForQuerySms,
                GprsModemProtocol              = measurementSettings?.ModemProtocol,
                CellularModuleId               = gprsSettings?.CellularModuleId,
                CellularModuleRevisionId       = gprsSettings?.CellularModuleRevisionId,
                CellularModuleSerialNumberIMEI = gprsSettings?.CellularModuleSerialNumberIMEI,
                CellularSIMCardId              = gprsSettings?.CellularSIMCardId,
                CellularSIMCardSubscriberId    = gprsSettings?.CellularSIMCardSubscriberId,


                FtpServerName                  = ftpSettings?.FtpServerName,
                FtpServerPath                  = ftpSettings?.FtpFilePath,
                FtpUsername                    = ftpSettings?.FtpLoginUserName,
                FtpAccount                     = ftpSettings?.FtpAccount,
                FtpPassword                    = ftpSettings?.FtpPassword,
                FtpPort                        = ConvertToInt(ftpSettings?.FtpDestinationControlPort),
                FtpUseTLS                      = measurementSettings?.SslTlsFtpEnable != 0,
                FtpSourceControlPort           = ConvertToInt(ftpSettings?.FtpSourceControlPort),
                FtpSourceDataPort              = ConvertToInt(ftpSettings?.FtpSourceDataPort),
                FtpUseActiveMode               = measurementSettings?.FtpMode != 0, //Read-only

                MailSmtpShowedName             = gprsSettings?.SmtpShowedName,
                MailPop3Username               = gprsSettings?.PopUsername,
                MailPop3Password               = gprsSettings?.PopPassword,
                MailSmtpUseSSL                 = (measurementSettings?.AccountSetting & (1 << 0)) != 0,
                MailPop3UseSSL                 = (measurementSettings?.AccountSetting & (1 << 1)) != 0,
                MailUseAlternativeSMTPLogin    = measurementSettings?.ServerConfig == 1,  //0 = same login; 1 = different login
                MailReturnAddress              = gprsSettings?.ReturnAddress,
                MailOptSmtpUsername            = gprsSettings?.OptSmtpUsername,
                MailOptSmtpPassword            = gprsSettings?.OptSmtpPassword,
                MailSmtpServer                 = gprsSettings?.SmtpServer,
                MailSmtpPort                   = ConvertToInt(gprsSettings?.SmtpPort),
                MailPop3Server                 = gprsSettings?.Pop3Server,
                MailPop3Port                   = ConvertToInt(gprsSettings?.Pop3Port),


                InfoTimer                      = (int?)measurementSettings?.Timer2Info,
                InfoInterval                   = (int?)measurementSettings?.Interval2Info,
                InfoSendSms                    = (measurementSettings?.SendSmsEmail & (1 << 2)) != 0,
                InfoSendMail                   = (measurementSettings?.SendSmsEmail & (1 << 6)) != 0,
                InfoSendFTP                    = (measurementSettings2?.SendTypeFtp & (1 << 2)) != 0,//BitPos 2
                InfoMailAddress                = textEmailSmsLocSettings?.EmailAddress3,
                InfoSmsNumber                  = textEmailSmsLocSettings?.SmsNumber3Info,


                CheckTimer                     = (int?)measurementSettings?.Timer3Check,
                CheckInterval                  = (int?)measurementSettings?.Interval3Check,
                CheckSendSms                   = (measurementSettings?.SendSmsEmail & (1 << 3)) != 0,
                CheckSendMail                  = (measurementSettings?.SendSmsEmail & (1 << 7)) != 0,
                CheckSendFTP                   = (measurementSettings2?.SendTypeFtp & (1 << 3)) != 0, //BitPos 3
                CheckAnswerSmsText             = textEmailSmsLocSettings?.SmsText3AnswerCheck,


                AlarmTimer                     = (int?)measurementSettings?.Timer1Alarm,
                AlarmInterval                  = (int?)measurementSettings?.Interval1Alarm,
                AlarmSendSms                   = (measurementSettings?.SendSmsEmail & (1 << 1)) != 0,
                AlarmSendMail                  = (measurementSettings?.SendSmsEmail & (1 << 5)) != 0,
                AlarmSendFTP                   = (measurementSettings2?.SendTypeFtp & (1 << 1)) != 0, //BitPos 1
                AlarmMailAddress               = textEmailSmsLocSettings?.EmailAddress2,
                AlarmSmsNumber                 = textEmailSmsLocSettings?.SmsNumber2Alarm,
                AlarmSmsText                   = textEmailSmsLocSettings?.SmsText2Alarm,
                AlarmSendXTimes                = measurementSettings?.SendAlarmXTimes,
                AlarmType                      = measurementSettings?.AlarmType,
                AlarmChannelNumber             = measurementSettings?.AlarmChannelNumber,
                AlarmOnThreshold               = (decimal?)floatingPointMeasurementSettings?.AlarmOn,
                AlarmOffThreshold              = (decimal?)floatingPointMeasurementSettings?.AlarmOff,
                AlarmDeltaThreshold            = (decimal?)floatingPointMeasurementSettings?.AlarmDelta,


                EventType                      = measurementSettings2?.EventType,
                EventMeasureTimer              = (int?)measurementSettings2?.TimerEvent,
                EventCheckInterval             = (int?)measurementSettings2?.IntervalEventCheck,
                EventMeasureInterval           = (int?)measurementSettings2?.IntervalEventMeasure,
                EventChannel                   = measurementSettings2?.EventChannel,
                EventOnValueThreshold          = (decimal?)floatingPointMeasurementSettings?.Val1OnEventLogging,
                EventOffValueThreshold         = (decimal?)floatingPointMeasurementSettings?.Val2OffEventLogging,
                EventDeltaValueThreshold       = (decimal?)floatingPointMeasurementSettings?.Val3DeltaEventLogging,
                EventSendMailXTimes            = measurementSettings2?.SendAfterYFilesWithRecordData,


                WaterLevelCalculationEnable           = (decimal?)floatingPointMeasurementSettings?.Val100WlcEnabled,
                WaterLevelCalculationLength           = (decimal?)floatingPointMeasurementSettings?.Val101WlcLength,
                WaterLevelCalculationHeight           = (decimal?)floatingPointMeasurementSettings?.Val102WlcHeight,
                WaterLevelCalculationOffset           = (decimal?)floatingPointMeasurementSettings?.Val103CalcOffset,
                WaterLevelCalculationDensity          = (decimal?)floatingPointMeasurementSettings?.Val104WlcDensity,
                WaterLevelCalculationWidth            = (decimal?)floatingPointMeasurementSettings?.Val105OflWidth,
                WaterLevelCalculationAngle            = (decimal?)floatingPointMeasurementSettings?.Val106OflAngle,
                WaterLevelCalculationFormFactor       = (decimal?)floatingPointMeasurementSettings?.Val107OflFormFactor,
                WaterLevelCalculationMinCalcHeight    = (decimal?)floatingPointMeasurementSettings?.Val108OflMinCalc,
                WaterLevelCalculationFormType         = measurementSettings?.OflFormType,
                WaterLevelCalculationIsAbsoluteSensor = (measurementSettings?.ConfigBytes0 == 1),
                WaterLevelCalculationCalculateFrom    = measurementSettings?.CalcChannels,
                WaterLevelCalculationConversionTo     = measurementSettings?.CalcConversionTo,
            };

            return configurationEntity;
        }

        public static string SetExtractedUniqueSerialNumber(BusinessObjectRoot businessObject)
        {
            string uniqueSerialNumber = "unknown";

            if (businessObject.DeviceInformation == null)
            {
                return uniqueSerialNumber;
            }

            //todo could be in Convert...Message...(); e.g. ConvertLoraMessageActility()
            switch (businessObject.DeviceInformation.ProductLine)
            {
                case ProductLineName.GSM:
                    uniqueSerialNumber = $"GSM-{businessObject.DeviceInformation.DeviceSerialNumber}"; //An old GSM-device only has a serial number but no HW device id
                    break;
                case ProductLineName.ARC1:
                    uniqueSerialNumber = $"ARC-{businessObject.DeviceInformation.DeviceIdAndClass}-{businessObject.DeviceInformation.DeviceSerialNumber}"; //An ARC-device has a serial number and a HW device id
                    break;
                case ProductLineName.ARC1_LoRa:
                    uniqueSerialNumber = $"EUI-{businessObject.LoRaData.EUI}";
                    break;
                case ProductLineName.ADT1_LoRa:
                    uniqueSerialNumber = $"EUI-{businessObject.LoRaData.EUI}";
                    break;
                case ProductLineName.ADT1_NBIoT_LTEM:
                    uniqueSerialNumber = $"ADT-{businessObject.DeviceInformation.DeviceIdAndClass}-{businessObject.DeviceInformation.DeviceSerialNumber}";
                    break;
                default:
                    uniqueSerialNumber = "unknown";
                    break;
            }

            return uniqueSerialNumber;
        }

        private static int? ConvertToInt(string s)
        {
            int? i = null;
            if (int.TryParse(s, out int j))
                i = j;
            return i;
        }
    }
}