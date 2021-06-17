namespace TRuDI.Models.BasicData
{
    using System.Diagnostics;

    /// <summary>
    /// Die Klasse IntervalReadingExt repräsentiert die Daten zu einem konkreten Messwert mit einem ggf. berechneten Leistungsmittelwert.
    /// </summary>
    [DebuggerDisplay("TargetTime: {TargetTime}, CaptureTime: {CaptureTime}, Value: {Value}, PowerValue: {PowerValue}, PTB-Status: {StatusPTB}")]
    public class IntervalReadingExt : IntervalReading
    {
        public IntervalReadingExt()
        {
        }

        public IntervalReadingExt(IntervalReading src)
        {
            this.IntervalBlock = src.IntervalBlock;
            this.TimePeriod = src.TimePeriod;
            this.TargetTime = src.TargetTime;
            this.MeasurementTimeMeter = src.MeasurementTimeMeter;
            this.Value = src.Value;
            this.Signature = src.Signature;
            this.MeterSignature = src.MeterSignature;
            this.StatusFNN = src.StatusFNN;
            this.StatusPTB = src.StatusPTB;
            this.StatusVendor = src.StatusVendor;
        }

        public long? PowerValue { get; set; }
        public string PowerValueInfo { get; set; }
    }
}