using KellerAg.Shared.Entities.Channel;
using KellerAg.Shared.Entities.FileFormat;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using KellerAg.Shared.Entities.Units;

namespace KellerAg.Shared.Entities.Calculations.CalculationModels
{
    /// <summary>
    /// Please see KellerAg.Shared.Entities.Tests.CalculationModelTest for an example
    /// </summary>
    public class ForceChannelCalculationModel : ChannelCalculationModelBase
    {
        public ForceChannelCalculationModel()
        {
            SetDefaultValues();
        }

        /// <summary>
        /// Needed for KOLIBRI Desktop to use custom channel names
        /// </summary>
        /// <param name="customChannelInfos"></param>
        public ForceChannelCalculationModel(ChannelInfo[] customChannelInfos) : base(customChannelInfos)
        {
            SetDefaultValues();
        }

        public ForceChannelCalculationModel(MeasurementFileFormatChannelCalculation fileFormatCalculation) : base(fileFormatCalculation)
        {
        }

        public ForceChannelCalculationModel(MeasurementFileFormatChannelCalculation fileFormatCalculation, ChannelInfo[] customChannelInfos) : base(fileFormatCalculation, customChannelInfos)
        {
        }

        private void SetDefaultValues()
        {
            ChannelInfo.UnitType = UnitType.Force;
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

        public double Offset { get; set; }

        public double Area { get; set; }

        public bool UseBarometricPressureToCompensate { get; set; }

        protected override Dictionary<CalculationParameter, string> CombineParameters()
        {
            return new Dictionary<CalculationParameter, string>
            {
                {CalculationParameter.HydrostaticPressureMeasurementDefinitionId, HydrostaticPressureChannel.MeasurementDefinitionId.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.BarometricPressureMeasurementDefinitionId,  BarometricPressureChannel?.MeasurementDefinitionId.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.Offset,  Offset.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.Area,  Area.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.UseBarometricPressureToCompensate, UseBarometricPressureToCompensate.ToString()}
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

            if (parameters.TryGetValue(CalculationParameter.Offset, out string offset))
            {
                Offset = double.TryParse(offset, NumberStyles.Number, CultureInfo.InvariantCulture, out double doubleOffset) ? doubleOffset : 0;
            }

            if (parameters.TryGetValue(CalculationParameter.Area, out string area))
            {
                Area = double.TryParse(area, NumberStyles.Number, CultureInfo.InvariantCulture, out double doubleArea) ? doubleArea : 0;
            }

            if (parameters.TryGetValue(CalculationParameter.UseBarometricPressureToCompensate, out string useBarometric))
            {
                UseBarometricPressureToCompensate = bool.TryParse(useBarometric, out bool useBaroBool) && useBaroBool;
            }
        }

        public sealed override int CalculationTypeId => CalculationTypeInfo.GetCalculationTypeId(CalculationType.Force);
    }
}