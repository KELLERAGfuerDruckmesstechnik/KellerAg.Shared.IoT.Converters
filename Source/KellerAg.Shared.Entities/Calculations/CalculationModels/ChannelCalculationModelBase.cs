using System.Collections.Generic;
using KellerAg.Shared.Entities.Channel;
using KellerAg.Shared.Entities.FileFormat;

namespace KellerAg.Shared.Entities.Calculations.CalculationModels
{
    public abstract class ChannelCalculationModelBase : MeasurementFileFormatChannelCalculation
    {
        public ChannelInfo[] CustomChannelInfos { get; }

        protected ChannelCalculationModelBase()
        {
            CustomChannelInfos    = ChannelInfo.GetChannels();
            ChannelInfo           = ChannelInfo.GetCalculationChannelInfoInstance();
        }

        protected ChannelCalculationModelBase(ChannelInfo[] customChannelInfos)
        {
            CustomChannelInfos    = customChannelInfos;
            ChannelInfo           = ChannelInfo.GetCalculationChannelInfoInstance();
        }

        protected ChannelCalculationModelBase(MeasurementFileFormatChannelCalculation fileFormat, ChannelInfo[] customChannelInfos)
        {
            CustomChannelInfos    = customChannelInfos;
            ChannelInfo           = fileFormat.ChannelInfo;
            CalculationParameters = fileFormat.CalculationParameters;
        }

        protected ChannelCalculationModelBase(MeasurementFileFormatChannelCalculation fileFormat)
        {
            CustomChannelInfos    = ChannelInfo.GetChannels();
            ChannelInfo           = fileFormat.ChannelInfo;
            CalculationParameters = fileFormat.CalculationParameters;
        }

        public sealed override Dictionary<CalculationParameter, string> CalculationParameters
        {
            get => CombineParameters();
            set => SplitParameters(value);
        }

        public MeasurementFileFormatChannelCalculation GetBase()
        {
            return new MeasurementFileFormatChannelCalculation
            {
                CalculationParameters = CalculationParameters,
                CalculationTypeId     = CalculationTypeId,
                ChannelInfo           = ChannelInfo
            };
        }

        protected abstract Dictionary<CalculationParameter, string> CombineParameters();

        protected abstract void SplitParameters(Dictionary<CalculationParameter, string> parameters);

        public abstract override int CalculationTypeId { get; }
    }
}