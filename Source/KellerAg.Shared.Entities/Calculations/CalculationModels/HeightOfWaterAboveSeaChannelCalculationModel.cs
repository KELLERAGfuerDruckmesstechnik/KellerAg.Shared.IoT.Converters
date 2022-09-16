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
    public class HeightOfWaterAboveSeaChannelCalculationModel : WaterChannelCalculationModelBase
    {
        public HeightOfWaterAboveSeaChannelCalculationModel()
        {
            SetDefaultValues(ChannelType.mH20_G);
        }

        /// <summary>
        /// Needed for KOLIBRI Desktop to use custom channel names
        /// </summary>
        /// <param name="customChannelInfos"></param>
        public HeightOfWaterAboveSeaChannelCalculationModel(ChannelInfo[] customChannelInfos) : base(customChannelInfos)
        {
            SetDefaultValues(ChannelType.mH20_G);
        }

        public HeightOfWaterAboveSeaChannelCalculationModel(MeasurementFileFormatChannelCalculation fileFormatCalculation) : base(fileFormatCalculation)
        {

        }

        public HeightOfWaterAboveSeaChannelCalculationModel(MeasurementFileFormatChannelCalculation fileFormatCalculation, ChannelInfo[] customChannelInfos) : base(fileFormatCalculation, customChannelInfos)
        {
        }

        public double InstallationLength { get; set; }

        public double HeightOfWellheadAboveSea { get; set; }

        protected sealed override Dictionary<CalculationParameter, string> CombineParameters()
        {
            var dictionary = base.CombineParameters();
            dictionary.Add(CalculationParameter.HeightOfWellheadAboveSea, HeightOfWellheadAboveSea.ToString(CultureInfo.InvariantCulture));
            dictionary.Add(CalculationParameter.InstallationLength, InstallationLength.ToString(CultureInfo.InvariantCulture));
            return dictionary;
        }

        protected sealed override void SplitParameters(Dictionary<CalculationParameter, string> parameters)
        {
            base.SplitParameters(parameters);

            if (parameters.TryGetValue(CalculationParameter.InstallationLength, out string installationLength))
            {
                InstallationLength = double.TryParse(installationLength, NumberStyles.Number, CultureInfo.InvariantCulture, out double doubleInstallationLength) ? doubleInstallationLength : 0;
            }

            if (parameters.TryGetValue(CalculationParameter.HeightOfWellheadAboveSea, out string heightOfWellheadAboveSea))
            {
                HeightOfWellheadAboveSea = double.TryParse(heightOfWellheadAboveSea, NumberStyles.Number, CultureInfo.InvariantCulture, out double doubleHeight) ? doubleHeight : 0;
            }
        }

        public sealed override int CalculationTypeId => CalculationTypeInfo.GetCalculationTypeId(CalculationType.HeightOfWaterAboveSea);
    }
}