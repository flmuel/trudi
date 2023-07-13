namespace TRuDI.Models.BasicData
{
    using System;

    [Flags]
    public enum SmgwStatusWord : uint
    {
        /// <summary>
        /// Statuswort-Identifikation
        /// </summary>
        SmgwStatusWordIdentification = 0x05,

        /// <summary>
        /// Abrechnungsrelevanz, entspricht dem TAF-Attribut "transparency_bit"
        /// </summary>
        Transparency_Bit = 0x2,

        /// <summary>
        /// Sobald das SMGw erstmalig einen fatalen Fehler erkennt. Dieser Zustand ist gemäß PTB-A 50.8 Tab. 4-2 zu setzen.
        /// </summary>
        Fatal_Error = 0x100,

        /// <summary>
        /// Sobald das SMGw feststellt, dass die Systemzeit ungültig ist.
        /// </summary>
        Systemtime_Invalid = 0x200,

        /// <summary>
        /// Warnung (wegen aktuell fehlender Definition nie gesetzt)
        /// </summary>
        PTB_Warning = 0x1000,

        /// <summary>
        /// Temporärer Fehler-V, kein Messwert im Empfangszeitfenster vom Zähler erfasst
        /// </summary>
        PTB_Temp_Error_signed_invalid = 0x2000,

        /// <summary>
        /// Temporärer Fehler-M, Magnetische oder mechanische Manipulation erkannt
        /// </summary>
        PTB_Temp_Error_is_invalid = 0x4000,

        /// <summary>
        /// Fehlerzustand nach OMS Spec. Vol-2
        /// </summary>
        Error_Status_according_to_OMS = 0x8000,

        /// <summary>
        /// Falls der Messwert aufgrund einer Schwellwertüber- oder unterschreitung den Messwertversand ausgelöst hat, wird das Bit auf ‚1‘ gesetzt
        /// </summary>
        Threshold_Marker = 0x10000,

        /// <summary>
        /// Identifikation Statuswort Zähler (1 = OMS Spec. Vol-2, 0 = FNN Lh. Basiszähler)
        /// </summary>
        Identification_Meter_Status_Word_OMS = 0x20000,
    }
}