namespace TRuDI.Models
{
    using System;
    using System.Globalization;

    using TRuDI.Models.BasicData;

    public static class UnitExtensions
    {
        private static readonly CultureInfo CultureInfoDe = CultureInfo.GetCultureInfo("DE");
        private static readonly string[] DecimalFormatLookup = { "N6", "N1", "N2", "N3", "N4", "N5", "N6", "N7", "N8", "N9" };

        public static string GetSiPrefix(this PowerOfTenMultiplier multiplier)
        {
            switch (multiplier)
            {
                case PowerOfTenMultiplier.micro:
                    return "μ";

                case PowerOfTenMultiplier.mili:
                    return "m";

                case PowerOfTenMultiplier.None:
                    return string.Empty;
                
                case PowerOfTenMultiplier.deci:
                    return "d";

                case PowerOfTenMultiplier.deca:
                    return "da";

                case PowerOfTenMultiplier.hecto:
                    return "h";

                case PowerOfTenMultiplier.kilo:
                    return "k";

                case PowerOfTenMultiplier.Mega:
                    return "M";

                case PowerOfTenMultiplier.Giga:
                    return "G";

                default:
                    return string.Empty;
            }
        }

        public static string GetUnitSymbol(this Uom uom)
        {
            switch (uom)
            {
                case Uom.Not_Applicable:
                    return string.Empty;

                case Uom.Ampere:
                    return "A";

                case Uom.Volltage:
                    return "V";

                case Uom.Joule:
                    return "J";

                case Uom.Frequency:
                    return "Hz";

                case Uom.AngleDegrees:
                    return "°";

                case Uom.Degrees_Celsius:
                    return "°C";

                case Uom.Real_power:
                    return "W";

                case Uom.Cubic_meter:
                    return "m³";

                case Uom.Apparent_power:
                    return "VA";

                case Uom.Reactive_power:
                    return "var";

                case Uom.Power_factor:
                    return "CosPhi";

                case Uom.Volts_squared:
                    return "V²";

                case Uom.Ampere_squared:
                    return "A²";

                case Uom.Apparent_energy:
                    return "VAh";

                case Uom.Real_energy:
                    return "Wh";

                case Uom.Reactive_energie:
                    return "varh";

                case Uom.Ampere_hours:
                    return "Ah";

                case Uom.Cubic_feet:
                    return "ft³";

                case Uom.Cubic_feet_per_hour:
                    return "ft³/h";

                case Uom.Cubic_meter_per_hour:
                    return "m³/h";

                case Uom.US_Gallons:
                    return "US gl";

                case Uom.US_Gallons_per_hour:
                    return "US gl/h";

                case Uom.Bel_mW:
                    return "Bm";

                default:
                    return string.Empty;
            }
        }

        public static string GetUnitSymbolPower(this Uom uom)
        {
            switch (uom)
            {
                case Uom.Not_Applicable:
                    return string.Empty;

                case Uom.Real_power:
                    return "W";

                case Uom.Apparent_power:
                    return "VA";

                case Uom.Reactive_power:
                    return "var";

                case Uom.Apparent_energy:
                    return "VA";

                case Uom.Real_energy:
                    return "W";

                case Uom.Reactive_energie:
                    return "var";

                case Uom.Ampere_hours:
                    return "A";

                default:
                    return string.Empty;
            }
        }

        public static string GetDisplayUnit(this Uom? uom, PowerOfTenMultiplier multiplier)
        {
            if (uom == null)
            {
                return string.Empty;
            }

            return uom.Value.GetDisplayUnit(multiplier);
        }

        public static string GetDisplayUnitPower(this Uom? uom, PowerOfTenMultiplier multiplier)
        {
            if (uom == null)
            {
                return string.Empty;
            }

            return uom.Value.GetDisplayUnitPower(multiplier);
        }

        public static string GetDisplayUnit(this Uom uom, PowerOfTenMultiplier multiplier)
        {
            if (uom == Uom.Not_Applicable)
            {
                return string.Empty;
            }

            // Special case for Wh --> return kWh
            if (multiplier == PowerOfTenMultiplier.None && uom == Uom.Real_energy)
            {
                return "kWh";
            }

            if (multiplier == PowerOfTenMultiplier.None && uom == Uom.Real_power)
            {
                return "kW";
            }

            return multiplier.GetSiPrefix() + uom.GetUnitSymbol();
        }

        public static string GetDisplayUnitPower(this Uom uom, PowerOfTenMultiplier multiplier)
        {
            if (uom != Uom.Real_energy)
            {
                return string.Empty;
            }

            // Special case for Wh/W --> return kWh/kW
            if (multiplier == PowerOfTenMultiplier.None)
            {
                return "kW";
            }

            var unit = uom.GetUnitSymbolPower();
            if (string.IsNullOrWhiteSpace(unit))
            {
                return string.Empty;
            }

            return multiplier.GetSiPrefix() + unit;
        }

        public static string GetDisplayValue(this long? value, ReadingType readingType, ObisId obisId)
        {
            return value.GetDisplayValue(readingType.Uom.Value, readingType.PowerOfTenMultiplier.Value, readingType.Scaler, obisId);
        }

        public static string GetDisplayValue(this long? value, Uom uom, PowerOfTenMultiplier multiplier, int scaler, ObisId obisId)
        {
            if (value == null)
            {
                return "---";
            }

            return value.Value.GetDisplayValue(uom, multiplier, scaler, obisId);
        }

        public static string GetDisplayValue(this long value, ReadingType readingType, ObisId obisId)
        {
            return value.GetDisplayValue(
                readingType.Uom.Value,
                readingType.PowerOfTenMultiplier.Value,
                readingType.Scaler,
                obisId);
        }

        public static string GetDisplayValue(this long value, Uom uom, PowerOfTenMultiplier multiplier, int scaler, ObisId obisId)
        {
            if (multiplier == PowerOfTenMultiplier.None && (uom == Uom.Real_energy || uom == Uom.Real_power))
            {
                scaler = scaler - 3;
                decimal scaledValue = scaler != 0 ? value * (decimal)Math.Pow(10, scaler) : value;
                return scaledValue.ToString(scaler < 0 && scaler >= -9 ? DecimalFormatLookup[scaler * -1] : "N", CultureInfoDe);
            }
            else
            {
                decimal scaledValue = scaler != 0 ? value * (decimal)Math.Pow(10, scaler) : value;
                return scaledValue.ToString(scaler < 0 && scaler >= -9 ? DecimalFormatLookup[scaler * -1] : "N", CultureInfoDe);
            }
        }
    }
}