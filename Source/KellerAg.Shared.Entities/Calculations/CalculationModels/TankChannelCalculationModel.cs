using KellerAg.Shared.Entities.Channel;
using KellerAg.Shared.Entities.FileFormat;
using KellerAg.Shared.Entities.Units;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace KellerAg.Shared.Entities.Calculations.CalculationModels
{
    /// <summary>
    /// Please see KellerAg.Shared.Entities.Tests.CalculationModelTest for an example
    /// </summary>
    public class TankChannelCalculationModel : ChannelCalculationModelBase
    {
        public TankChannelCalculationModel()
        {
            SetDefaultValues();
        }

        /// <summary>
        /// Needed for KOLIBRI Desktop to use custom channel names
        /// </summary>
        /// <param name="customChannelInfos"></param>
        public TankChannelCalculationModel(ChannelInfo[] customChannelInfos) : base(customChannelInfos)
        {
            SetDefaultValues();
        }

        public TankChannelCalculationModel(MeasurementFileFormatChannelCalculation fileFormatCalculation) : base(fileFormatCalculation)
        {
        }

        public TankChannelCalculationModel(MeasurementFileFormatChannelCalculation fileFormatCalculation, ChannelInfo[] customChannelInfos) : base(fileFormatCalculation, customChannelInfos)
        {
        }

        private void SetDefaultValues()
        {
            Gravity = 9.80665 /* m/s^2 */;
            Density = 998.2/* kg/m^3 */;
            ChannelInfo.UnitType = UnitType.Volume;
        }

        /// <summary>
        /// Former P1 in (Pd=P1-P2)
        /// </summary>
        public ChannelInfo HydrostaticPressureChannel { get; set; }

        /// <summary>
        /// Former P2 in (Pd=P1-P2)
        /// </summary>
        [CanBeNull]
        public ChannelInfo BarometricPressureChannel { get; set; }

        public ChannelInfo CorrespondingChannel { get; set; }

        public double Density { get; set; }

        public double Gravity { get; set; }

        public double Height { get; set; }

        public double Width { get; set; }

        public double Length { get; set; }

        public double InstallationLength { get; set; }

        public int TankTypeId { get; set; }

        public bool UseBarometricPressureToCompensate { get; set; }

        protected override Dictionary<CalculationParameter, string> CombineParameters()
        {
            return new Dictionary<CalculationParameter, string>
            {
                {CalculationParameter.HydrostaticPressureMeasurementDefinitionId, HydrostaticPressureChannel.MeasurementDefinitionId.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.BarometricPressureMeasurementDefinitionId,  BarometricPressureChannel?.MeasurementDefinitionId.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.CorrespondingMeasurementDefinitionId, CorrespondingChannel?.MeasurementDefinitionId.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.Gravity, Gravity.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.Density, Density.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.UseBarometricPressureToCompensate, UseBarometricPressureToCompensate.ToString()},
                {CalculationParameter.InstallationLength, InstallationLength.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.Width, Width.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.Height, Height.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.Length, Length.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.TankType, TankTypeId.ToString(CultureInfo.InvariantCulture)},
            };
        }

        protected override void SplitParameters(Dictionary<CalculationParameter, string> parameters)
        {
            if (parameters.TryGetValue(CalculationParameter.HydrostaticPressureMeasurementDefinitionId, out string hydro))
            {
                HydrostaticPressureChannel = CustomChannelInfos.FirstOrDefault(x => x.MeasurementDefinitionId == Convert.ToInt32(hydro));
            }

            if (parameters.TryGetValue(CalculationParameter.BarometricPressureMeasurementDefinitionId, out string baro))
            {
                BarometricPressureChannel = CustomChannelInfos.FirstOrDefault(x => x.MeasurementDefinitionId == Convert.ToInt32(baro));
            }

            if (parameters.TryGetValue(CalculationParameter.CorrespondingMeasurementDefinitionId, out string hintId))
            {
                CorrespondingChannel = int.TryParse(hintId, out int measurementDefinitionId) ? ChannelInfo.GetChannelInfo(measurementDefinitionId) : ChannelInfo.GetChannelInfo(ChannelType.Undefined);
            }

            if (parameters.TryGetValue(CalculationParameter.Gravity, out var gravity))
            {
                Gravity = double.TryParse(gravity, NumberStyles.Number, CultureInfo.InvariantCulture, out var doubleGravity) ? doubleGravity : 9.80665/* m/s^2 */;
            }

            if (parameters.TryGetValue(CalculationParameter.Density, out string density))
            {
                Density = double.TryParse(density, NumberStyles.Number, CultureInfo.InvariantCulture, out double doubleDensity) ? doubleDensity : 998.2/* kg/m^3 */;
            }

            if (parameters.TryGetValue(CalculationParameter.UseBarometricPressureToCompensate, out string useBarometric))
            {
                UseBarometricPressureToCompensate = bool.TryParse(useBarometric, out bool useBaroBool) && useBaroBool;
            }

            if (parameters.TryGetValue(CalculationParameter.InstallationLength, out string installationLength))
            {
                InstallationLength = double.TryParse(installationLength, NumberStyles.Number, CultureInfo.InvariantCulture, out double doubleInstallationLength) ? doubleInstallationLength : 0;
            }

            if (parameters.TryGetValue(CalculationParameter.Width, out string width))
            {
                Width = double.TryParse(width, NumberStyles.Number, CultureInfo.InvariantCulture, out double doubleWidth) ? doubleWidth : 0;
            }

            if (parameters.TryGetValue(CalculationParameter.Height, out string height))
            {
                Height = double.TryParse(height, NumberStyles.Number, CultureInfo.InvariantCulture, out double doubleHeight) ? doubleHeight : 0;
            }

            if (parameters.TryGetValue(CalculationParameter.Length, out string length))
            {
                Length = double.TryParse(length, NumberStyles.Number, CultureInfo.InvariantCulture, out double doubleLength) ? doubleLength : 0;
            }

            if (parameters.TryGetValue(CalculationParameter.TankType, out string tankType))
            {
                TankTypeId = int.TryParse(tankType, NumberStyles.Number, CultureInfo.InvariantCulture, out int intTankType) ? intTankType : 0;
            }
        }

        public sealed override int CalculationTypeId => CalculationTypeInfo.GetCalculationTypeId(CalculationType.Tank);
    }
}