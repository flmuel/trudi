namespace TRuDI.Models.BasicData
{
    using System;

    [Flags]
    public enum OmsStatusWord : uint
    {
        /// <summary>
        /// SF1 b7 - Wird vom SMGW gesetzt, wenn EF1 ausgewertet wurde (ist in EF1 gesetzt, wenn ein normiertes EF2 folgt) 
        /// </summary>
        SF1_Evaluated = 0x80000000,

        /// <summary>
        /// SF1 b6 - magnetische Manipulation
        /// </summary>
        Magnetic_Manipulation = 0x40000000,

        /// <summary>
        /// SF1 b5 - Verbindung unterbrochen 
        /// </summary>
        Connection_Interrupted = 0x20000000,

        /// <summary>
        /// SF1 b4 - Allgemeiner Hardware- oder Softwarefehler 
        /// </summary>
        Hardware_Or_Software_Error = 0x10000000,

        /// <summary>
        /// SF1 b3 - Batterie getrennt oder Abschaltung 
        /// </summary>
        Battery_Disconnected_Or_Shutdown = 0x08000000,

        /// <summary>
        /// SF1 b2 - Externer Alarm
        /// </summary>
        External_Alarm = 0x04000000,

        /// <summary>
        /// SF1 b1 - Batterie schwach 
        /// </summary>
        Low_Battery = 0x02000000,

        /// <summary>
        /// SF1 b0 - Manipulation
        /// </summary>
        Manipulation = 0x01000000,

        /// <summary>
        /// SF2 b7 - Wird vom SMGW gesetzt, wenn EF2 ausgewertet wurde (ist in EF2 gesetzt, wenn ein normiertes EF3 folgt)
        /// </summary>
        SF2_Evaluated = 0x00800000,

        /// <summary>
        /// SF2 b6 - Uhrensynchronisierungsfehler
        /// </summary>
        Clock_Synchronization_Error = 0x00400000,

        /// <summary>
        /// SF2 b5 - Sensor außerhalb des zulässigen Bereichs
        /// </summary>
        Sensor_Out_Of_Range = 0x00200000,

        /// <summary>
        /// SF2 b4 - Verbrauch außerhalb des zulässigen Bereichs 
        /// </summary>
        Consumption_Out_Of_Range = 0x00100000,

        /// <summary>
        /// SF2 b3 - Fehlender Durchfluss 
        /// </summary>
        Lack_Of_Flow = 0x00080000,

        /// <summary>
        /// SF2 b2 - Leckage
        /// </summary>
        Leakage = 0x00040000,

        /// <summary>
        /// SF2 b1 - Guthabengrenze überschritten 
        /// </summary>
        Credit_Limit_Exceeded = 0x00020000,

        /// <summary>
        /// SF2 b0 - nicht autorisierter Zugriffsversuch 
        /// </summary>
        Unauthorized_Access_Attempt = 0x00010000,

        /// <summary>
        /// SF3 b7 - Wird vom SMGW gesetzt, wenn EF3 ausgewertet wurde
        /// </summary>
        SF3_Evaluated = 0x00008000,

        /// <summary>
        /// SF3 b6 - Historisch: Sensor außerhalb des zulässigen Bereichs 
        /// </summary>
        Historical_Sensor_Out_Of_Range = 0x00004000,

        /// <summary>
        /// SF3 b5 - Historisch: fehlender Durchfluss 
        /// </summary>
        Historical_Lack_Of_Flow = 0x00002000,

        /// <summary>
        /// SF3 b4 - Historisch: Leckage 
        /// </summary>
        Historical_Leakage = 0x00001000,

        /// <summary>
        /// SF3 b3 - Historisch: externer Alarm
        /// </summary>
        Historical_External_Alarm = 0x00000800,

        /// <summary>
        /// SF3 b2 - Historisch: Unterbrechung der Verbindung 
        /// </summary>
        Historical_Connection_Interrupted = 0x00000400,

        /// <summary>
        /// SF3 b1 - Historisch: magnetische Manipulation 
        /// </summary>
        Historical_Magnetic_Manipulation = 0x00000200,

        /// <summary>
        /// SF3 b0 - Historisch: mechanische Manipulation 
        /// </summary>
        Historical_Mechanical_Manipulation = 0x00000100,

        /// <summary>
        /// STS b4 - Vorübergehender Fehler
        /// </summary>
        Temporary_Error = 0x10,

        /// <summary>
        /// STS b3 - Dauerhafter Fehler 
        /// </summary>
        Permanent_Error = 0x08,

        /// <summary>
        /// STS b2 - Energieversorgung niedrig 
        /// </summary>
        Power_Low = 0x04,

        Application_Busy = 0x01,

        Application_Error = 0x02,

        Application_Alarm = 0x03,

    }
}