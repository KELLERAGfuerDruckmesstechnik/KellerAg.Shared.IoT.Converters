using System;
using System.Data;
using System.Linq;

namespace KellerAg.Shared.Entities.Units
{
    public class UnitInfo : IEquatable<UnitInfo>
    {
        private string _fullName;
        private string _shortName;
        private double _factor;
        private double _offset;
        private UnitType _unitType;

        /// <summary>
        /// Gets back a list of all valid units. The first unit is the default unit.
        /// </summary>
        /// <param name="unitType"></param>
        /// <returns></returns>
        public static UnitInfo[] GetUnits(UnitType unitType)
        {
            UnitInfo[] unitInfos;
            switch (unitType)
            {
                case UnitType.Pressure:
                    unitInfos = new[]
                    {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("bar", "bar", UnitType.Pressure, 1, 0),
                        new UnitInfo("millibar", "mbar", UnitType.Pressure, 1000, 0),
                        new UnitInfo("pound-force per square inch", "PSI", UnitType.Pressure, 14.5038, 0),
                        new UnitInfo("Megapascal", "MPa", UnitType.Pressure, 0.1, 0),
                        new UnitInfo("kilopascal", "kPa", UnitType.Pressure, 100, 0),
                        new UnitInfo("pascal", "Pa", UnitType.Pressure, 100000, 0)
                    };
                    break;
                case UnitType.Length:
                    unitInfos = new[]
                    {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("meter", "m", UnitType.Length, 1, 0),
                        new UnitInfo("centimeter", "cm", UnitType.Length, 100, 0),
                        new UnitInfo("inch", "inch", UnitType.Length, 39.3701, 0),
                        new UnitInfo("feet", "ft", UnitType.Length, 3.28084, 0)
                    };
                    break;
                case UnitType.Temperature:
                    unitInfos = new[]
                    {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("celsius", "°C", UnitType.Temperature, 1, 0),
                        new UnitInfo("fahrenheit", "°F", UnitType.Temperature, 1.8, 32)
                    };
                    break;
                case UnitType.Conductivity:
                    unitInfos = new[]
                    {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("millisiemens/centimeter", "mS/cm", UnitType.Conductivity, 1, 0),
                        new UnitInfo("microsiemens/centimeter", "uS/cm", UnitType.Conductivity, 1000, 0)
                    };
                    break;
                case UnitType.Voltage:
                    unitInfos = new[]
                    {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("volt", "V", UnitType.Voltage, 1, 0),
                    };
                    break;
                case UnitType.Density:
                    unitInfos = new[]
                    {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("kilogram/meter^3", "kg/m^3", UnitType.Density, 1, 0),
                    };
                    break;
                case UnitType.Acceleration:
                    unitInfos = new[]
                    {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("meter/second^2", "m/s^2", UnitType.Acceleration, 1, 0),
                    };
                    break;
                case UnitType.Angle:
                    unitInfos = new[]
                    {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("degrees", "°", UnitType.Angle, 1, 0),
                    };
                    break;
                case UnitType.Flow:
                    unitInfos = new[]
                    {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("meter^3/second", "m^3/s", UnitType.Flow, 1, 0),
                        new UnitInfo("liters/second", "l/s", UnitType.Flow, 1000, 0),
                    };
                    break;
                case UnitType.Volume:
                    unitInfos = new[]
                    {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("meter^3", "m^3", UnitType.Volume, 1, 0),
                        new UnitInfo("liter", "l", UnitType.Volume, 1000, 0),
                    };
                    break;
                case UnitType.Force:
                    unitInfos = new[]
                    {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("newton", "N", UnitType.Force, 1, 0),
                        new UnitInfo("kilonewton", "kN", UnitType.Force, 1000, 0),
                    };
                    break;
                case UnitType.Area:
                    unitInfos = new[]
                    {                        
                        //Be aware that the first unit is the default unit
                        new UnitInfo("meter^2", "m^2", UnitType.Area, 1, 0),
                        new UnitInfo("centimeter^2", "cm^2", UnitType.Area, 10000, 0),
                        new UnitInfo("inch^2", "inch^2", UnitType.Area, 1550, 0),
                        new UnitInfo("feet^2", "ft^2", UnitType.Area, 10.7639, 0)
                    };
                    break;
                case UnitType.Unknown:
                    unitInfos = new UnitInfo[0];
                    break;
                default:
                    unitInfos = new UnitInfo[0];
                    break;
            }
            return unitInfos;
        }

        public static UnitInfo[] CurrentUnits()
        {
            var unitTypes = GetAllUnitTypes();
            var unitInfos = new UnitInfo[unitTypes.Length];
            var i = 0;
            foreach (var unit in unitTypes)
            {
                unitInfos[i] = CurrentUnit(unit);
                i++;
            }

            return unitInfos;
        }

        public static UnitInfo[] BaseUnits()
        {
            var unitTypes = GetAllUnitTypes();
            var unitInfos = new UnitInfo[unitTypes.Length];
            var i = 0;
            foreach (var unit in unitTypes)
            {
                unitInfos[i] = GetUnits(unit).FirstOrDefault();
                i++;
            }

            return unitInfos;
        }

        public static UnitInfo CurrentUnit(UnitType unitType)
        {
            UnitInfo unit;
            switch (unitType)
            {
                case UnitType.Pressure:
                    unit = CurrentPressureUnit ?? GetUnits(UnitType.Pressure).FirstOrDefault();
                    break;
                case UnitType.Length:
                    unit = CurrentLengthUnit ?? GetUnits(UnitType.Length).FirstOrDefault();
                    break;
                case UnitType.Temperature:
                    unit = CurrentTemperatureUnit ?? GetUnits(UnitType.Temperature).FirstOrDefault();
                    break;
                case UnitType.Conductivity:
                    unit = CurrentConductivityUnit ?? GetUnits(UnitType.Conductivity).FirstOrDefault();
                    break;
                case UnitType.Voltage:
                    unit = CurrentVoltageUnit ?? GetUnits(UnitType.Voltage).FirstOrDefault();
                    break;
                case UnitType.Volume:
                    unit = CurrentVolumeUnit ?? GetUnits(UnitType.Volume).FirstOrDefault();
                    break;
                case UnitType.Density:
                    unit = CurrentDensityUnit ?? GetUnits(UnitType.Density).FirstOrDefault();
                    break;
                case UnitType.Acceleration:
                    unit = CurrentAccelerationUnit ?? GetUnits(UnitType.Acceleration).FirstOrDefault();
                    break;
                case UnitType.Angle:
                    unit = CurrentAngleUnit ?? GetUnits(UnitType.Angle).FirstOrDefault();
                    break;
                case UnitType.Flow:
                    unit = CurrentFlowUnit ?? GetUnits(UnitType.Flow).FirstOrDefault();
                    break;
                case UnitType.Force:
                    unit = CurrentForceUnit ?? GetUnits(UnitType.Force).FirstOrDefault();
                    break;
                case UnitType.Area:
                    unit = CurrentAreaUnit ?? GetUnits(UnitType.Area).FirstOrDefault();
                    break;
                case UnitType.Unknown:
                    unit = new UnitInfo();
                    break;
                default:
                    unit = new UnitInfo();
                    break;
            }
            return unit;
        }

        public static void SetCurrentUnit(UnitInfo unit)
        {
            switch (unit.UnitType)
            {
                case UnitType.Pressure:
                    CurrentPressureUnit = unit;
                    break;
                case UnitType.Length:
                    CurrentLengthUnit = unit;
                    break;
                case UnitType.Temperature:
                    CurrentTemperatureUnit = unit;
                    break;
                case UnitType.Conductivity:
                    CurrentConductivityUnit = unit;
                    break;
                case UnitType.Voltage:
                    CurrentVoltageUnit = unit;
                    break;
                case UnitType.Volume:
                    CurrentVolumeUnit = unit;
                    break;
                case UnitType.Density:
                    CurrentDensityUnit = unit;
                    break;
                case UnitType.Acceleration:
                    CurrentAccelerationUnit = unit;
                    break;
                case UnitType.Angle:
                    CurrentAngleUnit = unit;
                    break;
                case UnitType.Flow:
                    CurrentFlowUnit = unit;
                    break;
                case UnitType.Force:
                    CurrentForceUnit = unit;
                    break;
                case UnitType.Area:
                    CurrentAreaUnit = unit;
                    break;
                case UnitType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public double ToBase(double number)
        {
            return (number - Offset) / Factor;
        }

        public double? ToBase(double? number)
        {
            return (number - Offset) / Factor;
        }

        public double FromBase(double number)
        {
            return (Factor * number) + Offset;
        }

        public double? FromBase(double? number)
        {
            return Factor * number + Offset;
        }

        public static double BaseToCurrent(double numberInBaseUnit, UnitType unitType)
        {
            return CurrentUnit(unitType).FromBase(numberInBaseUnit);
        }

        public static double CurrentToBase(double numberInCurrentUnit, UnitType unitType)
        {
            return CurrentUnit(unitType).ToBase(numberInCurrentUnit);
        }

        private static UnitType[] GetAllUnitTypes()
        {
            return Enum.GetValues(typeof(UnitType)).Cast<UnitType>().ToArray();
        }

        private static UnitInfo CurrentPressureUnit { get; set; }
        private static UnitInfo CurrentTemperatureUnit { get; set; }
        private static UnitInfo CurrentLengthUnit { get; set; }
        private static UnitInfo CurrentConductivityUnit { get; set; }
        private static UnitInfo CurrentVoltageUnit { get; set; }
        private static UnitInfo CurrentVolumeUnit { get; set; }
        private static UnitInfo CurrentDensityUnit { get; set; }
        private static UnitInfo CurrentAccelerationUnit { get; set; }
        private static UnitInfo CurrentAngleUnit { get; set; }
        private static UnitInfo CurrentFlowUnit { get; set; }
        private static UnitInfo CurrentForceUnit { get; set; }
        private static UnitInfo CurrentAreaUnit { get; set; }

        public UnitInfo()
        {
        }

        public UnitInfo(string fullName, string shortName, UnitType unitType, double factor, double offset, bool isReadonly = true)
        {
            _fullName = fullName;
            _shortName = shortName;
            _unitType = unitType;
            _factor = factor;
            _offset = offset;
            IsReadonly = isReadonly;
        }

        public string FullName
        {
            get => _fullName;
            set
            {
                ReadOnlyCheck();
                _fullName = value;
            }
        }

        public string ShortName
        {
            get => _shortName;
            set
            {

                ReadOnlyCheck();
                _shortName = value;
            }
        }

        public double Factor
        {
            get => _factor;
            set
            {

                ReadOnlyCheck();
                _factor = value;
            }
        }

        public double Offset
        {
            get => _offset;
            set
            {
                ReadOnlyCheck();
                _offset = value;
            }
        }

        public UnitType UnitType
        {
            get => _unitType;
            set
            {

                ReadOnlyCheck();
                _unitType = value;
            }
        }

        public bool IsReadonly { get; }

        private void ReadOnlyCheck()
        {
            if (IsReadonly)
            {
                throw new ReadOnlyException($"This {typeof(UnitInfo)} was initialized as readonly");
            }
        }

        public bool Equals(UnitInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _fullName == other._fullName && _shortName == other._shortName && _factor.Equals(other._factor) && _offset.Equals(other._offset) && _unitType == other._unitType && IsReadonly == other.IsReadonly;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((UnitInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + _fullName.GetHashCode();
                hash = hash * 23 + _shortName.GetHashCode();
                hash = hash * 23 + _factor.GetHashCode();
                hash = hash * 23 + _offset.GetHashCode();
                hash = hash * 23 + _unitType.GetHashCode();
                hash = hash * 23 + IsReadonly.GetHashCode();
                return hash;
            }
            //Only since netstandard2.1
            //return HashCode.Combine(_fullName, _shortName, _factor, _offset, (int) _unitType, IsReadonly);
        }
    }
}
