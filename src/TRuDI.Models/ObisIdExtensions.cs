namespace TRuDI.Models
{
    using TRuDI.HanAdapter.Interface;

    public static class ObisIdExtensions
    {
        public static string ToFormattedObis(this string hexId)
        {
            return new ObisId(hexId).ToString();
        }

        public static bool IsCounter(this ObisId id)
        {
            switch (id.Medium)
            {
                case ObisMedium.Electricity:
                    return id.D == 8;

                case ObisMedium.HeatCostAllocator:
                    return (id.C == 1 && id.D == 0 && id.E == 0) || (id.C == 1 && id.D == 2 && id.E == 0);

                case ObisMedium.Cooling:
                case ObisMedium.Heat:
                    return (id.C == 1 && id.D == 0 && id.E == 0) || (id.C == 1 && id.D == 2 && id.E == 0) || (id.C == 2 && id.D == 0 && id.E == 0) || (id.C == 2 && id.D == 2 && id.E == 0);

                case ObisMedium.Gas:
                    return (id.C == 3 && id.D == 0 && id.E == 0) || (id.C == 3 && id.D == 1 && id.E == 0);

                case ObisMedium.WaterCold:
                case ObisMedium.WaterHot:
                    return id.C == 1 && id.D == 0 && id.E == 0;

                case ObisMedium.Abstract:
                case ObisMedium.Communication:
                default:
                    return false;
            }
        }

        public static string GetMediumLabel(this ObisId id)
        {
            switch (id.Medium)
            {
                case ObisMedium.Abstract:
                    return string.Empty;

                case ObisMedium.Electricity:
                    return "Elektrizität";

                case ObisMedium.HeatCostAllocator:
                    return "Heizkostenverteiler";

                case ObisMedium.Cooling:
                    return "Kälte";

                case ObisMedium.Heat:
                    return "Wärme";

                case ObisMedium.Gas:
                    return "Gas";

                case ObisMedium.WaterCold:
                    return "Kaltwasser";

                case ObisMedium.WaterHot:
                    return "Warmwasser";

                case ObisMedium.Communication:
                    return "Kommunikationsgerät";

                default:
                    return "unbekannt";
            }
        }

        public static string GetLabel(this ObisId id)
        {
            switch (id.Medium)
            {
                case ObisMedium.Electricity:
                    return GetElectricityLabel(id);

                case ObisMedium.Gas:
                    return GetGasLabel(id);

                case ObisMedium.Cooling:
                case ObisMedium.Heat:
                    return GetHeatOrCoolingLabel(id);

                case ObisMedium.HeatCostAllocator:
                    return GetHeatCostAllocatorLabel(id);

                case ObisMedium.WaterHot:
                case ObisMedium.WaterCold:
                    return GetWaterLabel(id);

                default:
                    if (id.C == 96)
                    {
                        return "Herstellerspezifische Kennziffer";
                    }

                    return $"Für die Kennziffer {id} ist keine Beschreibung hinterlegt";
            }
        }

        private static string GetWaterLabel(ObisId id)
        {
            // 8-0:1.0.0*255 - Volume (V), accumulated, total, current value
            if (id.C == 1 && id.D == 0 && id.E == 0)
            {
                return "Volumen";
            }

            // 8-0:1.2.0*255 - Volume (V), accumulated, total, due date value
            if (id.C == 1 && id.D == 2 && id.E == 0)
            {
                return "Volumen";
            }

            // 8-0:2.0.0*255 - Flow rate, average (Va/t), current value
            if (id.C == 2 && id.D == 0 && id.E == 0)
            {
                return "Durchfluss";
            }

            if (id.C == 96)
            {
                return "Herstellerspezifische Kennziffer";
            }

            return $"Für die Kennziffer {id} ist keine Beschreibung hinterlegt";
        }

        private static string GetHeatCostAllocatorLabel(ObisId id)
        {
            // 4-0:1.0.0*255 - Unrated integral, current value
            if (id.C == 1 && id.D == 0 && id.E == 0)
            {
                return "Unbewerteter Zählerstand";
            }

            // 4-0:1.2.0*255 - Unrated integral, due date value
            if (id.C == 1 && id.D == 2 && id.E == 0)
            {
                return "Unbewerteter Zählerstand";
            }

            if (id.C == 96)
            {
                return "Herstellerspezifische Kennziffer";
            }

            return $"Für die Kennziffer {id} ist keine Beschreibung hinterlegt";
        }

        private static string GetHeatOrCoolingLabel(ObisId id)
        {
            // 6-0:1.0.0*255 - Energy (A), total, current value
            if (id.C == 1 && id.D == 0 && id.E == 0)
            {
                return "Energie";
            }

            // 6-0:1.2.0*255 - Energy (A), total, due date value
            if (id.C == 1 && id.D == 2 && id.E == 0)
            {
                return "Energie";
            }

            // 6-0:2.0.0*255 - Volume (V), accumulated, total, current value
            if (id.C == 2 && id.D == 0 && id.E == 0)
            {
                return "Volumen";
            }

            // 6-0:2.2.0*255 - Volume (V), accumulated, total, due date value
            if (id.C == 2 && id.D == 2 && id.E == 0)
            {
                return "Volumen";
            }

            // 6-0:8.0.0*255 - Power (energy flow) (P), average, current value
            if (id.C == 8 && id.D == 0 && id.E == 0)
            {
                return "Leistung";
            }

            // 6-0:9.0.0*255 - Flow rate, average (Va/t), current value
            if (id.C == 9 && id.D == 0 && id.E == 0)
            {
                return "Durchfluss";
            }

            // 6-0:10.0.0*255 - Flow temperature, current value
            if (id.C == 10 && id.D == 0 && id.E == 0)
            {
                return "Vorlauftemperatur";
            }

            // 6-0:11.0.0*255 - Return temperature, current value
            if (id.C == 11 && id.D == 0 && id.E == 0)
            {
                return "Rücklauftemperatur";
            }

            if (id.C == 96)
            {
                return "Herstellerspezifische Kennziffer";
            }

            return $"Für die Kennziffer {id} ist keine Beschreibung hinterlegt";
        }

        private static string GetGasLabel(ObisId id)
        {
            // 7-0:3.1.0*255 - Volume, temperature converted
            if (id.C == 3 && id.D == 1 && id.E == 0)
            {
                return "Volumen";
            }

            // 7-0:3.0.0*255 - Volume, measuring conditions
            if (id.C == 3 && id.D == 0 && id.E == 0)
            {
                return "Volumen";
            }

            if (id.C == 96)
            {
                return "Herstellerspezifische Kennziffer";
            }

            return $"Für die Kennziffer {id} ist keine Beschreibung hinterlegt";
        }

        private static string GetElectricityLabel(ObisId id)
        {
            if (id.C == 81 && id.D == 7)
            {
                switch (id.E)
                {
                    case 1:
                        return "Phasenwinkel U (L2) / U (L1)";

                    case 2:
                        return "Phasenwinkel U (L3) / U (L1)";

                    case 4:
                        return "Phasenwinkel I (L1) / U (L1)";

                    case 15:
                        return "Phasenwinkel I (L2) / U (L2)";

                    case 26:
                        return "Phasenwinkel I (L3) / U (L3)";
                }
            }

            if (id.C == 1)
            {
                switch (id.D)
                {
                    case 6:
                        return "Wirkleistung Bezug Maximum 1";

                    case 16:
                        return "Wirkleistung Bezug Maximum 2";

                    case 26:
                        return "Wirkleistung Bezug Maximum 3";

                    case 36:
                        return "Wirkleistung Bezug Maximum 4";

                    case 46:
                        return "Wirkleistung Bezug Maximum 5";

                    case 3:
                        return "Wirkleistung Bezug Minimum 1";

                    case 13:
                        return "Wirkleistung Bezug Minimum 2";

                    case 23:
                        return "Wirkleistung Bezug Minimum 3";

                    case 33:
                        return "Wirkleistung Bezug Minimum 4";

                    case 43:
                        return "Wirkleistung Bezug Minimum 5";
                }
            }

            if (id.C == 2)
            {
                switch (id.D)
                {
                    case 6:
                        return "Wirkleistung Lieferung Maximum 1";

                    case 16:
                        return "Wirkleistung Lieferung Maximum 2";

                    case 26:
                        return "Wirkleistung Lieferung Maximum 3";

                    case 36:
                        return "Wirkleistung Lieferung Maximum 4";

                    case 46:
                        return "Wirkleistung Lieferung Maximum 5";

                    case 3:
                        return "Wirkleistung Lieferung Minimum 1";

                    case 13:
                        return "Wirkleistung Lieferung Minimum 2";

                    case 23:
                        return "Wirkleistung Lieferung Minimum 3";

                    case 33:
                        return "Wirkleistung Lieferung Minimum 4";

                    case 43:
                        return "Wirkleistung Lieferung Minimum 5";
                }
            }

            if (id.D == 7 && id.E == 0)
            {
                switch (id.C)
                {
                    case 14:
                        return "Frequenz";

                    case 16:
                        return "Wirkleistung Verbrauch";

                    case 32:
                        return "Spannung L1";

                    case 36:
                        return "Wirkleistung L1";

                    case 52:
                        return "Spannung L2";

                    case 56:
                        return "Wirkleistung L2";

                    case 72:
                        return "Spannung L3";

                    case 76:
                        return "Wirkleistung L3";

                    case 31:
                        return "Strom L1";

                    case 51:
                        return "Strom L2";

                    case 71:
                        return "Strom L3";
                }
            }

            string tariff;
            switch (id.E)
            {
                case 0:
                    tariff = "Gesamt";
                    break;

                case 63:
                    tariff = "Fehlerregister";
                    break;

                default:
                    tariff = $"Tarifregister {id.E}";
                    break;
            }

            switch (id.C)
            {
                case 1:
                    return "Elektrische Wirkarbeit Bezug " + tariff;

                case 2:
                    return "Elektrische Wirkarbeit Lieferung " + tariff;
            }

            if (id.C == 96)
            {
                return "Herstellerspezifische Kennziffer";
            }

            return $"Für die Kennziffer {id} ist keine Beschreibung hinterlegt";
        }
    }
}