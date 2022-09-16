using KellerAg.Shared.Entities.Calculations.CalculationModels;
using KellerAg.Shared.Entities.Channel;
using KellerAg.Shared.Entities.FileFormat;
using KellerAg.Shared.Entities.Units;
using System;

namespace KellerAg.Shared.Entities.Calculations
{
    public static class CalculationsHelper
    {
        public static MeasurementFileFormatChannelCalculation GenerateCalculation(MeasurementFileFormatWaterCalculationStoredInDevice deviceCalculation)
        {
            MeasurementFileFormatChannelCalculation calculation = null;

            if (deviceCalculation?.WaterLevelCalculation != null)
            {
                MeasurementFileFormatWaterLevel waterLevelConf = deviceCalculation.WaterLevelCalculation;
                switch (waterLevelConf.WaterLevelType)
                {
                    case WaterLevelType.HeightOfWater:
                        calculation = new HeightOfWaterChannelCalculationModel
                        {
                            HydrostaticPressureChannel = ChannelInfo.GetChannelInfo(waterLevelConf.HydrostaticPressureChannelId),
                            BarometricPressureChannel = waterLevelConf.BarometricPressureChannelId.HasValue ? ChannelInfo.GetChannelInfo((int)waterLevelConf.BarometricPressureChannelId) : null,
                            ChannelInfo = ChannelInfo.GetCalculationChannelInfoInstance(ChannelType.mH20_E),
                            Density = waterLevelConf.Density,
                            Offset = waterLevelConf.Offset,
                            Gravity = waterLevelConf.Gravity,
                            UseBarometricPressureToCompensate = waterLevelConf.UseBarometricPressureToCompensate

                        }.GetBase();
                        break;
                    case WaterLevelType.DepthToWater:
                        calculation = new DepthToWaterChannelCalculationModel
                        {
                            HydrostaticPressureChannel = ChannelInfo.GetChannelInfo(waterLevelConf.HydrostaticPressureChannelId),
                            BarometricPressureChannel = waterLevelConf.BarometricPressureChannelId.HasValue ? ChannelInfo.GetChannelInfo((int)waterLevelConf.BarometricPressureChannelId) : null,
                            ChannelInfo = ChannelInfo.GetCalculationChannelInfoInstance(ChannelType.mH20_F),
                            Density = waterLevelConf.Density,
                            Offset = waterLevelConf.Offset,
                            Gravity = waterLevelConf.Gravity,
                            InstallationLength = waterLevelConf.InstallationLength,
                            UseBarometricPressureToCompensate = waterLevelConf.UseBarometricPressureToCompensate

                        }.GetBase();
                        break;
                    case WaterLevelType.HeightOfWaterAboveSeaLevel:
                        calculation = new HeightOfWaterAboveSeaChannelCalculationModel
                        {
                            HydrostaticPressureChannel = ChannelInfo.GetChannelInfo(waterLevelConf.HydrostaticPressureChannelId),
                            BarometricPressureChannel = waterLevelConf.BarometricPressureChannelId.HasValue ? ChannelInfo.GetChannelInfo((int)waterLevelConf.BarometricPressureChannelId) : null,
                            ChannelInfo = ChannelInfo.GetCalculationChannelInfoInstance(ChannelType.mH20_G),
                            Density = waterLevelConf.Density,
                            Offset = waterLevelConf.Offset,
                            Gravity = waterLevelConf.Gravity,
                            InstallationLength = waterLevelConf.InstallationLength,
                            HeightOfWellheadAboveSea = waterLevelConf.HeightOfWellhead,
                            UseBarometricPressureToCompensate = waterLevelConf.UseBarometricPressureToCompensate

                        }.GetBase();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (deviceCalculation?.OverflowCalculation != null)
            {
                MeasurementFileFormatOverflow overflowCalc = deviceCalculation.OverflowCalculation;
                switch (overflowCalc.OverflowType)
                {
                    case OverflowType.Poleni:
                        calculation = new OverflowPoleniChannelCalculationModel()
                        {
                            HydrostaticPressureChannel = ChannelInfo.GetChannelInfo(overflowCalc.HydrostaticPressureChannelId),
                            BarometricPressureChannel = overflowCalc.BarometricPressureChannelId.HasValue ? ChannelInfo.GetChannelInfo((int)overflowCalc.BarometricPressureChannelId) : null,
                            ChannelInfo = ChannelInfo.GetCalculationChannelInfoInstance("Overflow (Poleni)", "", "#000000", UnitType.Flow),
                            Density = overflowCalc.Density,
                            Offset = overflowCalc.Offset,
                            Gravity = overflowCalc.Gravity,
                            UseBarometricPressureToCompensate = overflowCalc.UseBarometricPressureToCompensate,
                            WallHeight = overflowCalc.WallHeight,
                            FormWidth = overflowCalc.FormWidth,
                            FormFactor = overflowCalc.FormFactor


                        }.GetBase();
                        break;
                    case OverflowType.Thomson:
                        calculation = new OverflowThomsonChannelCalculationModel()
                        {
                            HydrostaticPressureChannel = ChannelInfo.GetChannelInfo(overflowCalc.HydrostaticPressureChannelId),
                            BarometricPressureChannel = overflowCalc.BarometricPressureChannelId.HasValue ? ChannelInfo.GetChannelInfo((int)overflowCalc.BarometricPressureChannelId) : null,
                            ChannelInfo = ChannelInfo.GetCalculationChannelInfoInstance("Overflow (Thomson)", "", "#000000", UnitType.Flow),
                            Density = overflowCalc.Density,
                            Offset = overflowCalc.Offset,
                            Gravity = overflowCalc.Gravity,
                            UseBarometricPressureToCompensate = overflowCalc.UseBarometricPressureToCompensate,
                            WallHeight = overflowCalc.WallHeight,
                            FormAngle = overflowCalc.FormAngle,
                            FormFactor = overflowCalc.FormFactor


                        }.GetBase();
                        break;
                    case OverflowType.Venturi:
                        calculation = new OverflowVenturiChannelCalculationModel()
                        {
                            HydrostaticPressureChannel = ChannelInfo.GetChannelInfo(overflowCalc.HydrostaticPressureChannelId),
                            BarometricPressureChannel = overflowCalc.BarometricPressureChannelId.HasValue ? ChannelInfo.GetChannelInfo((int)overflowCalc.BarometricPressureChannelId) : null,
                            ChannelInfo = ChannelInfo.GetCalculationChannelInfoInstance("Flow (Venturi)", "", "#000000", UnitType.Flow),
                            Density = overflowCalc.Density,
                            Offset = overflowCalc.Offset,
                            Gravity = overflowCalc.Gravity,
                            UseBarometricPressureToCompensate = overflowCalc.UseBarometricPressureToCompensate,
                            FormWidth = overflowCalc.FormWidth,
                            FormFactor = overflowCalc.FormFactor


                        }.GetBase();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (deviceCalculation?.TankCalculation != null)
            {
            }

            return calculation;
        }


        private static bool IsOverflowCalculation(int calculationId)
        {
            return
                calculationId == (int) CalculationType.OverflowPoleni ||
                calculationId == (int) CalculationType.OverflowThomson ||
                calculationId == (int) CalculationType.OverflowVenturi;
        }
    }
}
