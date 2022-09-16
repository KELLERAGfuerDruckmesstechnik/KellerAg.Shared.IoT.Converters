using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using KellerAg.Shared.Entities.Channel;
using KellerAg.Shared.Entities.FileFormat;

namespace KellerAg.Shared.Entities.Calculations.CalculationModels
{
    public class OffsetChannelCalculationModel : ChannelCalculationModelBase
    {
        private ChannelInfo _offsetChannel;

        public OffsetChannelCalculationModel()
        {
        }

        public OffsetChannelCalculationModel(ChannelInfo[] customChannelInfos) : base(customChannelInfos)
        {
        }

        public OffsetChannelCalculationModel(MeasurementFileFormatChannelCalculation fileFormatCalculation) : base(fileFormatCalculation)
        {
        }

        public OffsetChannelCalculationModel(MeasurementFileFormatChannelCalculation fileFormatCalculation, ChannelInfo[] customChannelInfos) : base(fileFormatCalculation, customChannelInfos)
        {
        }

        public double Offset { get; set; }

        //public int ChannelTypeId { get; set; }

        public ChannelInfo OffsetChannel
        {
            get => _offsetChannel;
            set
            {
                _offsetChannel = value;
                if (ChannelInfo != null)
                {
                    ChannelInfo.UnitType = _offsetChannel.UnitType;
                }
            }
        }

        protected override Dictionary<CalculationParameter, string> CombineParameters()
        {
            return new Dictionary<CalculationParameter, string>
            {
                {CalculationParameter.Offset, Offset.ToString(CultureInfo.InvariantCulture)},
                {CalculationParameter.ChannelId, OffsetChannel?.MeasurementDefinitionId.ToString(CultureInfo.InvariantCulture)}

            };
        }

        protected override void SplitParameters(Dictionary<CalculationParameter, string> parameters)
        {
            if (parameters.TryGetValue(CalculationParameter.ChannelId, out string id))
            {
                var idNumber = Convert.ToInt32(id);
                OffsetChannel = CustomChannelInfos.FirstOrDefault(x => x.MeasurementDefinitionId == idNumber);
            }
            if (parameters.TryGetValue(CalculationParameter.Offset, out string offset))
            {
                Offset = double.TryParse(offset, NumberStyles.Number, CultureInfo.InvariantCulture, out var doubleOffset) ? doubleOffset : 0;
            }
        }

        public sealed override int CalculationTypeId => CalculationTypeInfo.GetCalculationTypeId(CalculationType.Offset);
    }
}