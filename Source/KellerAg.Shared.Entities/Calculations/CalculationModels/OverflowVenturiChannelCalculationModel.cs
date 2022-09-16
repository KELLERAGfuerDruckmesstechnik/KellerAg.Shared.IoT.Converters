using KellerAg.Shared.Entities.Channel;
using KellerAg.Shared.Entities.FileFormat;
using KellerAg.Shared.Entities.Units;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace KellerAg.Shared.Entities.Calculations.CalculationModels
{
    public class OverflowVenturiChannelCalculationModel : ChannelCalculationModelBase
    {
        public OverflowVenturiChannelCalculationModel()
        {
            SetDefaultValues();
        }

        public OverflowVenturiChannelCalculationModel(ChannelInfo[] customChannelInfos) : base(customChannelInfos)
        {
            SetDefaultValues();
        }

        public OverflowVenturiChannelCalculationModel(MeasurementFileFormatChannelCalculation fileFormatCalculation) : base(fileFormatCalculation)
        {
        }

        public OverflowVenturiChannelCalculationModel(MeasurementFileFormatChannelCalculation fileFormatCalculation, ChannelInfo[] customChannelInfos) : base(fileFormatCalculation, customChannelInfos)
        {
        }
        private void SetDefaultValues()
        {
            Gravity = 9.80665 /* m/s^2 */;
            Density = 998.2/* kg/m^3 */;
            ChannelInfo.UnitType = UnitType.Flow;
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

        public bool UseBarometricPressureToCompensate { get; set; }

        public double Offset { get; set; }

        public double Density { get; set; }

        public double Gravity { get; set; }

        public double FormFactor { get; set; }

        public double FormWidth { get; set; }

        protected override Dictionary<CalculationParameter, string> CombineParameters()
        {
            return new Dictionary<CalculationParameter, string>
            {
                {CalculationParameter.HydrostaticPressureMeasurementDefinitionId, HydrostaticPressureChannel.MeasurementDefinitionId.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.BarometricPressureMeasurementDefinitionId,  BarometricPressureChannel?.MeasurementDefinitionId.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.CorrespondingMeasurementDefinitionId, CorrespondingChannel?.MeasurementDefinitionId.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.Gravity, Gravity.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.Offset,  Offset.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.Density, Density.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.UseBarometricPressureToCompensate, UseBarometricPressureToCompensate.ToString()},
                {CalculationParameter.FormFactor, FormFactor.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.FormWidth, FormWidth.ToString(CultureInfo.InvariantCulture)}
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

            if (parameters.TryGetValue(CalculationParameter.Offset, out string offset))
            {
                Offset = double.TryParse(offset, NumberStyles.Number, CultureInfo.InvariantCulture, out double doubleOffset) ? doubleOffset : 0;
            }

            if (parameters.TryGetValue(CalculationParameter.Density, out string density))
            {
                Density = double.TryParse(density, NumberStyles.Number, CultureInfo.InvariantCulture, out double doubleDensity) ? doubleDensity : 998.2/* kg/m^3 */;
            }

            if (parameters.TryGetValue(CalculationParameter.UseBarometricPressureToCompensate, out string useBarometric))
            {
                UseBarometricPressureToCompensate = bool.TryParse(useBarometric, out bool useBaroBool) && useBaroBool;
            }

            if (parameters.TryGetValue(CalculationParameter.FormFactor, out string formFactor))
            {
                FormFactor = double.TryParse(formFactor, NumberStyles.Number, CultureInfo.InvariantCulture, out double doubleFormFactor) ? doubleFormFactor : 0;
            }

            if (parameters.TryGetValue(CalculationParameter.FormWidth, out string formWidth))
            {
                FormWidth = double.TryParse(formWidth, NumberStyles.Number, CultureInfo.InvariantCulture, out double doubleFormWidth) ? doubleFormWidth : 0;
            }
        }

        public sealed override int CalculationTypeId => CalculationTypeInfo.GetCalculationTypeId(CalculationType.OverflowVenturi);
    }
}