namespace TRuDI.Models.BasicData
{
    /// <summary>
    /// Das Datenelement uom codiert die Maßeinheit, welche für alle Messwerte der Messwertliste gilt. 
    /// Gültige Werte nach ESPI REQ.21 sind:
    /// 
    ///     0 = Not Applicable
    ///     5 = A(Current)
    ///     29 = Voltage
    ///     31 = J(Energy joule)
    ///     33 = Hz(Frequency)
    ///     38 = Real power(Watts)
    ///     42 = m^3(Cubic Meter)
    ///     61 = VA(Apparent power)
    ///     63 = VAr(Reactive power)
    ///     65 = CosPhi(Power factor)
    ///     67 = V^2(Volts squared)
    ///     69 = A^2(Amp squared)
    ///     71 = VAh(Apparent energy)
    ///     72 = Real energy(Watt-hours)
    ///     73 = VArh(Reactive energy)
    ///     106 = Ah(Ampere-hours / Available Charge)
    ///     119 = ft^3(Cubic Feet)
    ///     122 = ft^3/h(Cubic Feet per Hour)
    ///     125 = m^3/h(Cubic Meter per Hour)
    ///     128 = US gl(US Gallons)
    ///     129 = US gl/h(US Gallons per Hour)
    ///     
    ///Das Datenelement uom muss mit einem entsprechenden Wert gefüllt werden.
    /// </summary>
    public enum Uom : ushort
    {
        /// <summary>
        /// Not Applicable, none
        /// </summary>
        Not_Applicable = 0,

        /// <summary>
        /// Current, ampere, A
        /// </summary>
        Ampere = 5,

        /// <summary>
        /// Plane angle, degrees, deg
        /// </summary>
        AngleDegrees = 9,

        /// <summary>
        /// Relative temperature in degrees Celsius
        /// </summary>
        Degrees_Celsius = 23,

        /// <summary>
        /// Electric potential, Volt (W/A), V
        /// </summary>
        Volltage = 29,

        /// <summary>
        /// Energy joule, (N·m = C·V = W·s), J
        /// </summary>
        Joule = 31,

        /// <summary>
        /// Frequency hertz, (1/s), Hz
        /// </summary>
        Frequency = 33,

        /// <summary>
        /// Real power (Watts)
        /// </summary>
        Real_power = 38,

        /// <summary>
        /// Volume, cubic meter, m³"
        /// </summary>
        Cubic_meter = 42,

        /// <summary>
        /// Apparent power, Volt Ampere, VA
        /// </summary>
        Apparent_power = 61,

        /// <summary>
        /// Reactive power, Volt Ampere reactive, VAr
        /// </summary>
        Reactive_power = 63,

        /// <summary>
        /// Power factor, Dimensionless, cos?
        /// </summary>
        Power_factor = 65,

        /// <summary>
        /// Volts squared, Volt squared (W2/A2), V²
        /// </summary>
        Volts_squared = 67,

        /// <summary>
        /// Amps squared, amp squared, A2
        /// </summary>
        Ampere_squared = 69,

        /// <summary>
        /// Apparent energy, Volt Ampere hours, VAh
        /// </summary>
        Apparent_energy = 71,

        /// <summary>
        /// Real energy, Watt hours, Wh
        /// </summary>
        Real_energy = 72,

        /// <summary>
        /// Reactive energy, Volt Ampere reactive hours, VArh
        /// </summary>
        Reactive_energie = 73,

        /// <summary>
        /// Ampere-hours, Ampere-hours, Ah
        /// </summary>
        Ampere_hours = 106,

        /// <summary>
        /// Signal Strength, Bel-mW, normalized to 1mW, Bm
        /// </summary>
        Bel_mW = 113,

        /// <summary>
        /// Volume, cubic feet, ft³
        /// </summary>
        Cubic_feet = 119,

        /// <summary>
        /// Volumetric flow rate, cubic feet per hour, ft³/h
        /// </summary>
        Cubic_feet_per_hour = 122,

        /// <summary>
        /// Volumetric flow rate, cubic meters per hour, m³/h
        /// </summary>
        Cubic_meter_per_hour = 125,

        /// <summary>
        /// Volume, US gallons, Gal
        /// </summary>
        US_Gallons = 128,

        /// <summary>
        /// Volumetric flow rate, US gallons per hour, USGal/h
        /// </summary>
        US_Gallons_per_hour = 129,
    }
}
