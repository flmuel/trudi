namespace TRuDI.Models.BasicData
{
    using System;

    /// <summary>
    /// Kombiniertes Statuswort zu einem Messwert (nach FNN LH SMGw).
    /// </summary>
    /// <remarks>
    /// Soweit ein Messwert ein Statuswort mitführt, muss dieses als kombiniertes Statuswort aus dem Statuswort des Messwert-Gebers (i.d.R. ein Sensor bzw. Zähler)
    /// und dem Statuswort des SMGW gebildet werden.
    /// </remarks>
    public class StatusFNN
    {
        public const string SMGWMASK = "00000000000000000xxx00xx000001x1";
        public const string BZMASK = "00000000000xxxxxxxxxxxxx00000100";

        public StatusFNN(string status)
        {
            this.Status = status.PadLeft(16, '0');
            var smgwStat = Convert.ToInt64(this.Status.Substring(0, 8), 16);
            var bzStat = Convert.ToInt64(this.Status.Substring(8), 16);
            this.SmgwStatusWord = (SmgwStatusWord)smgwStat;
            this.BzStatusWord = (BzStatusWord)bzStat;
        }

        public string Status
        {
            get; set;
        }

        public SmgwStatusWord SmgwStatusWord
        {
            get; set;
        }

        public BzStatusWord BzStatusWord
        {
            get; set;
        }

        public StatusPTB MapToStatusPtb()
        {
            if (this.BzStatusWord.HasFlag(BzStatusWord.Fatal_Error) ||
                this.SmgwStatusWord.HasFlag(SmgwStatusWord.Fatal_Error))
            {
                return StatusPTB.FatalError;
            }

            if (this.SmgwStatusWord.HasFlag(SmgwStatusWord.Systemtime_Invalid) ||
                this.SmgwStatusWord.HasFlag(SmgwStatusWord.PTB_Temp_Error_is_invalid))
            {
                return StatusPTB.CriticalTemporaryError;
            }

            if (this.SmgwStatusWord.HasFlag(SmgwStatusWord.PTB_Temp_Error_signed_invalid) ||
                this.BzStatusWord.HasFlag(BzStatusWord.Manipulation_KD_PS) ||
                this.BzStatusWord.HasFlag(BzStatusWord.Magnetically_Influenced))
            {
                return StatusPTB.TemporaryError;
            }

            if (this.SmgwStatusWord.HasFlag(SmgwStatusWord.PTB_Warning))
            {
                return StatusPTB.Warning;
            }

            return StatusPTB.NoError;
        }
    }

    // 0x00002005
    [Flags]
    public enum SmgwStatusWord : uint
    {
        SmgwStatusWordIdentification = 0x05,
        Transparency_Bit = 0x2,
        Fatal_Error = 0x100,
        Systemtime_Invalid = 0x200,
        PTB_Warning = 0x1000,
        PTB_Temp_Error_signed_invalid = 0x2000,
        PTB_Temp_Error_is_invalid = 0x4000,
    }

    [Flags]
    public enum BzStatusWord : uint
    {
        BzStatusWordIdentification = 0x04,
        Start_Up = 0x100,
        Magnetically_Influenced = 0x200,
        Manipulation_KD_PS = 0x400,
        Sum_Energiedirection_neg = 0x800,
        Energiedirection_L1_neg = 0x1000,
        Energiedirection_L2_neg = 0x2000,
        Energiedirection_L3_neg = 0x4000,
        PhaseSequenz_RotatingField_Not_L1_L2_L3 = 0x8000,
        BackStop_Active = 0x10000,
        Fatal_Error = 0x20000,
        Lead_Voltage_L1_existent = 0x40000,
        Lead_Voltage_L2_existent = 0x80000,
        Lead_Voltage_L3_existent = 0x100000
    }
}
