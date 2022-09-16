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
    public class DepthToWaterChannelCalculationModel : WaterChannelCalculationModelBase
    {
        public DepthToWaterChannelCalculationModel()
        {
            SetDefaultValues(ChannelType.mH20_F);
        }

        /// <summary>
        /// Needed for KOLIBRI Desktop to use custom channel names
        /// </summary>
        /// <param name="customChannelInfos"></param>
        public DepthToWaterChannelCalculationModel(ChannelInfo[] customChannelInfos) : base(customChannelInfos)
        {
            SetDefaultValues(ChannelType.mH20_F);
        }

        public DepthToWaterChannelCalculationModel(MeasurementFileFormatChannelCalculation fileFormatCalculation) : base(fileFormatCalculation)
        {

        }

        public DepthToWaterChannelCalculationModel(MeasurementFileFormatChannelCalculation fileFormatCalculation, ChannelInfo[] customChannelInfos) : base(fileFormatCalculation, customChannelInfos)
        {
        }

        public double InstallationLength { get; set; }

        protected sealed override Dictionary<CalculationParameter, string> CombineParameters()
        {
            var dictionary = base.CombineParameters();
            dictionary.Add(CalculationParameter.InstallationLength, InstallationLength.ToString(CultureInfo.InvariantCulture));
            return dictionary;
        }

        protected sealed override void SplitParameters(Dictionary<CalculationParameter, string> parameters)
        {
            base.SplitParameters(parameters);

            if (parameters.TryGetValue(CalculationParameter.InstallationLength, out var installationLength))
            {
                InstallationLength = double.TryParse(installationLength, NumberStyles.Number, CultureInfo.InvariantCulture, out var doubleInstallationLength) ? doubleInstallationLength : 0;
            }
        }

        public sealed override int CalculationTypeId => CalculationTypeInfo.GetCalculationTypeId(CalculationType.DepthToWater);
    }
}