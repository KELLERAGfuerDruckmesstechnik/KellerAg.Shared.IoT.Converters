using KellerAg.Shared.Entities.Channel;
using KellerAg.Shared.Entities.FileFormat;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace KellerAg.Shared.Entities.Calculations.CalculationModels
{
    /// <summary>
    /// Please see KellerAg.Shared.Entities.Tests.CalculationModelTest for an example
    /// </summary>
    public class HeightOfWaterChannelCalculationModel : WaterChannelCalculationModelBase
    {
        public HeightOfWaterChannelCalculationModel()
        {
            SetDefaultValues(ChannelType.mH20_E);
        }

        /// <summary>
        /// Needed for KOLIBRI Desktop to use custom channel names
        /// </summary>
        /// <param name="customChannelInfos"></param>
        public HeightOfWaterChannelCalculationModel(ChannelInfo[] customChannelInfos) : base(customChannelInfos)
        {
            SetDefaultValues(ChannelType.mH20_E);
        }

        public HeightOfWaterChannelCalculationModel(MeasurementFileFormatChannelCalculation fileFormatCalculation) : base(fileFormatCalculation)
        {
        }

        public HeightOfWaterChannelCalculationModel(MeasurementFileFormatChannelCalculation fileFormatCalculation, ChannelInfo[] customChannelInfos) : base(fileFormatCalculation, customChannelInfos)
        {
        }

        protected sealed override Dictionary<CalculationParameter, string> CombineParameters()
        {
            return base.CombineParameters();
        }

        protected sealed override void SplitParameters(Dictionary<CalculationParameter, string> parameters)
        {
            base.SplitParameters(parameters);
        }

        public sealed override int CalculationTypeId => CalculationTypeInfo.GetCalculationTypeId(CalculationType.HeightOfWater);
    }
}