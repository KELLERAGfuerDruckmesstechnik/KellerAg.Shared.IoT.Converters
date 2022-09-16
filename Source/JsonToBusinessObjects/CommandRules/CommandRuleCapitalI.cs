namespace JsonToBusinessObjects.CommandRules
{
    using System;
    using DataContainers;
    using Infrastructure;
    using Infrastructure.Logging;
    using Newtonsoft.Json.Linq;

    internal class CommandRuleCapitalI : CommandRuleBase, ICommandRule
    {
        private class CommandModificationCapitalI : ICommandModification
        {
            private readonly DeviceInformation _deviceInformation;

            public CommandModificationCapitalI(DeviceInformation deviceInformation)
            {
                this._deviceInformation = deviceInformation;
            }

            public void ApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                if (this.CanApplyToBusinessObjectRoot(businessObjectRoot) == false)
                {
                    throw new InvalidOperationException($"Cannot apply command rule {nameof(CommandModificationCapitalI)} to business object.");
                }

                businessObjectRoot.DeviceInformation = this._deviceInformation;
            }

            public bool CanApplyToBusinessObjectRoot(BusinessObjectRoot businessObjectRoot)
            {
                return businessObjectRoot.DeviceInformation == null;
            }
        }

        public CommandRuleCapitalI(ILogger logger) : base(logger)
        {
        }

        public char HandledCommandCharacter => 'I';

        public ICommandModification CreateModificationObject(JToken variables)
        {
            DeviceInformation deviceInformation = new DeviceInformation();

            variables.ExecuteIfAvailable("n", (int value) => deviceInformation.DeviceSerialNumber = value, this.Logger);
            variables.ExecuteIfAvailable("s", (int value) => deviceInformation.SignalQuality = value, this.Logger);
            variables.ExecuteIfAvailable("b", (byte value) => deviceInformation.BatteryCapacity = value, this.Logger);
            variables.ExecuteIfAvailable("e", (string value) => deviceInformation.DeviceIdAndClass = value, this.Logger);
            variables.ExecuteIfAvailable("f", (string value) => deviceInformation.GsmModuleSoftwareVersion = value, this.Logger);
            variables.ExecuteIfAvailable("h", (byte value) => deviceInformation.MeasuredHumidity = value, this.Logger);
            variables.ExecuteIfAvailable("v", (float value) => deviceInformation.MeasuredBatteryVoltage = value, this.Logger);
            variables.ExecuteIfAvailable("t", (int value) => deviceInformation.CellularTechnologyInUse = value, this.Logger);


            switch (deviceInformation.DeviceIdAndClass)
            {
                case null:
                    deviceInformation.ProductLine = ProductLineName.GSM;
                    break;
                case "": //GSM (2G)
                    deviceInformation.ProductLine = ProductLineName.GSM;
                    break;
                case "9.20": //ARC(2G,3G,4G)
                    deviceInformation.ProductLine = ProductLineName.ARC1;
                    break;
                case "9.50": //ARC1 (LoRa)
                    deviceInformation.ProductLine = ProductLineName.ARC1_LoRa;
                    break;
                case "19.00": //ADT1 (LoRa)
                    deviceInformation.ProductLine = ProductLineName.ADT1_LoRa;
                    break;
                case "19.20": //ADT1 (NB-IoT / LTE-M)
                    deviceInformation.ProductLine = ProductLineName.ADT1_NBIoT_LTEM;
                    break;
                default:
                    if (deviceInformation.CellularTechnologyInUse != null)
                    {
                        //it is NOT a GSM-2
                        deviceInformation.ProductLine = ProductLineName.ARC1;
                    }

                    //default: ARC
                    deviceInformation.ProductLine = ProductLineName.ARC1;
                    break;
            }

            return new CommandModificationCapitalI(deviceInformation);
        }
    }
}