using System;
using KellerAg.Shared.Entities.Channel;
using KellerAg.Shared.Entities.FileFormat;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using KellerAg.Shared.Entities.Units;

namespace KellerAg.Shared.Entities.Calculations.CalculationModels
{
    /// <summary>
    /// Please see KellerAg.Shared.Entities.Tests.CalculationModelTest for an example
    /// </summary>
    public abstract class WaterChannelCalculationModelBase : ChannelCalculationModelBase
    {
        protected WaterChannelCalculationModelBase()
        {
        }

        /// <summary>
        /// Needed for KOLIBRI Desktop to use custom channel names
        /// </summary>
        /// <param name="customChannelInfos"></param>
        protected WaterChannelCalculationModelBase(ChannelInfo[] customChannelInfos) : base(customChannelInfos)
        {
        }

        protected WaterChannelCalculationModelBase(MeasurementFileFormatChannelCalculation fileFormatCalculation) : base(fileFormatCalculation)
        {

        }

        protected WaterChannelCalculationModelBase(MeasurementFileFormatChannelCalculation fileFormatCalculation, ChannelInfo[] customChannelInfos) : base(fileFormatCalculation, customChannelInfos)
        {
        }

        protected void SetDefaultValues(ChannelType? correspondingChannelType = null)
        {
            Gravity = 9.80665 /* m/s^2 */;
            Density = 998.2/* kg/m^3 */;
            if (correspondingChannelType.HasValue)
            {
                CorrespondingChannel = ChannelInfo.GetChannelInfo(correspondingChannelType.Value);  //Must be overwritten when calc channel like 55-59
            }
            if (CorrespondingChannel != null)
            {
                ChannelInfo.UnitType = CorrespondingChannel.UnitType;
                ChannelInfo.Description = CorrespondingChannel.Description;
                ChannelInfo.Name = CorrespondingChannel.Name;
                ChannelInfo.ColorCode = CorrespondingChannel.ColorCode;
            }
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

        [CanBeNull]
        public DateTime? FromDate { get; set; }

        [CanBeNull]
        public DateTime? ToDate { get; set; }

        public ChannelInfo CorrespondingChannel { get; set; }

        public double Offset { get; set; }

        public double Density { get; set; }

        public double Gravity { get; set; }

        public bool UseBarometricPressureToCompensate { get; set; }

        protected override Dictionary<CalculationParameter, string> CombineParameters()
        {
            return new Dictionary<CalculationParameter, string>
            {
                {CalculationParameter.HydrostaticPressureMeasurementDefinitionId, HydrostaticPressureChannel.MeasurementDefinitionId.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.BarometricPressureMeasurementDefinitionId, BarometricPressureChannel?.MeasurementDefinitionId.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.CorrespondingMeasurementDefinitionId, CorrespondingChannel?.MeasurementDefinitionId.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.Gravity, Gravity.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.Offset, Offset.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.Density, Density.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.UseBarometricPressureToCompensate, UseBarometricPressureToCompensate.ToString()},
                {CalculationParameter.From, FromDate?.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.To, ToDate?.ToString(CultureInfo.InvariantCulture)},
            };
        }

        protected override void SplitParameters(Dictionary<CalculationParameter, string> parameters)
        {
            if (parameters.TryGetValue(CalculationParameter.HydrostaticPressureMeasurementDefinitionId, out var hydro))
            {
                HydrostaticPressureChannel = CustomChannelInfos.FirstOrDefault(x => x.MeasurementDefinitionId == Convert.ToInt32(hydro));
            }

            if (parameters.TryGetValue(CalculationParameter.BarometricPressureMeasurementDefinitionId, out var baro))
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

            if (parameters.TryGetValue(CalculationParameter.Offset, out var offset))
            {
                Offset = double.TryParse(offset, NumberStyles.Number, CultureInfo.InvariantCulture, out var doubleOffset) ? doubleOffset : 0;
            }

            if (parameters.TryGetValue(CalculationParameter.Density, out var density))
            {
                Density = double.TryParse(density, NumberStyles.Number, CultureInfo.InvariantCulture, out var doubleDensity) ? doubleDensity : 998.2/* kg/m^3 */;
            }

            if (parameters.TryGetValue(CalculationParameter.UseBarometricPressureToCompensate, out var useBarometric))
            {
                UseBarometricPressureToCompensate = bool.TryParse(useBarometric, out var useBaroBool) && useBaroBool;
            }

            if (parameters.TryGetValue(CalculationParameter.From, out var fromDateString))
            {
                if (DateTime.TryParse(fromDateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fromDate))
                {
                    FromDate = fromDate;
                }

            }

            if (parameters.TryGetValue(CalculationParameter.To, out var toDateString))
            {
                if (DateTime.TryParse(toDateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var toDate))
                {
                    ToDate = toDate;
                }

            }
        }
    }
}