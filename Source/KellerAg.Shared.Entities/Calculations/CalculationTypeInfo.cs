using System.Linq;

namespace KellerAg.Shared.Entities.Calculations
{
    public class CalculationTypeInfo
    {
        private static CalculationTypeInfo[] CalculationTypes { get; set; } = null;

        public static CalculationTypeInfo[] GetCalculationTypes()
        {
            return CalculationTypes ?? (CalculationTypes = new[]
            {
                //IMPORTANT: NEVER change the Id of the types! Extending the list is allowed
                new CalculationTypeInfo(1, CalculationType.HeightOfWater),
                new CalculationTypeInfo(2, CalculationType.DepthToWater),
                new CalculationTypeInfo(3, CalculationType.HeightOfWaterAboveSea),
                new CalculationTypeInfo(4, CalculationType.Offset),
                new CalculationTypeInfo(5, CalculationType.OverflowPoleni),
                new CalculationTypeInfo(6, CalculationType.OverflowThomson),
                new CalculationTypeInfo(7, CalculationType.OverflowVenturi),
                new CalculationTypeInfo(8, CalculationType.Force),
                new CalculationTypeInfo(9, CalculationType.Tank),
            });
        }

        public static CalculationType GetCalculationType(int calculationTypeId)
        {
            var type = GetCalculationTypes().SingleOrDefault(x => x.CalculationTypeId == calculationTypeId);
            return type?.CalculationType ?? CalculationType.Unknown;
        }

        public static int GetCalculationTypeId(CalculationType type)
        {
            var info = GetCalculationTypes().FirstOrDefault(x => x.CalculationType == type);
            return info?.CalculationTypeId ?? 0;
        }

        public CalculationTypeInfo(int calculationTypeId, CalculationType calculationType)
        {
            CalculationType = calculationType;
            CalculationTypeId = calculationTypeId;
        }

        public CalculationType CalculationType { get; set; }

        public int CalculationTypeId { get; set; }
    }
}
