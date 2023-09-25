namespace TRuDI.TafAdapter.Taf8
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TRuDI.Models;
    using TRuDI.Models.BasicData;
    using TRuDI.TafAdapter.Interface;
    using TRuDI.TafAdapter.Interface.Taf2;
    using TRuDI.TafAdapter.Interface.Taf8;
    using TRuDI.TafAdapter.Taf8.Components;

    /// <inheritdoc />
    /// <summary>
    /// Default Taf-1 implementation.
    /// </summary>
    public class TafAdapterTaf8 : ITafAdapter
    {
        private List<OriginalValueList> originalValueLists;
        private DateTime billingPeriodStart;
        private DateTime billingPeriodEnd;

        /// <inheritdoc />
        /// <summary>
        /// Calculates the derived register for Taf1.
        /// </summary>
        /// <param name="device">Date from the SMGW. There should be just original value lists.</param>
        /// <param name="supplier">The calculation data from the supplier.</param>
        /// <returns>An ITaf2Data instance. The object contains the calculated data.</returns>
        public TafAdapterData Calculate(UsagePointAdapterTRuDI device, UsagePointLieferant supplier)
        {
            this.originalValueLists =
                device.MeterReadings.Where(mr => mr.IsOriginalValueList()).Select(mr => new OriginalValueList(mr, device.ServiceCategory.Kind ?? Kind.Electricity)).ToList();

            var ovlGroups = this.originalValueLists.GroupBy(i => i.MeterReading.ReadingType.QualifiedLogicalName).ToList();
            foreach (var ovlGroup in ovlGroups)
            {
                if (ovlGroup.Count() == 2)
                {
                    // Remove current data reading from OVL
                    var currentDataReadout = ovlGroup.FirstOrDefault(i => i.ValueCount == 1);
                    if (currentDataReadout != null)
                    {
                        this.originalValueLists.Remove(currentDataReadout);
                    }
                }
            }

            if (!this.originalValueLists.Any())
            {
                throw new InvalidOperationException("Es ist keine originäre Messwertliste verfügbar.");
            }

            var accountingPeriod = new Taf8Data();
            accountingPeriod.SetBillingPeriod(supplier.AnalysisProfile.BillingPeriod.Start, supplier.AnalysisProfile.BillingPeriod.GetEnd());

            var registers = new List<Taf8Register>();
            accountingPeriod.SetRegisters(registers);

            foreach (var ts in supplier.AnalysisProfile.TariffStages)
            {
                if (!ObisId.TryParse(ts.ObisCode, out var obisCode))
                {
                    throw new InvalidOperationException($"OBIS konnte nicht gelesen werden: \"{ts.ObisCode}\"");
                }

                registers.Add(new Taf8Register { ObisCode = obisCode, Amount = null });
            }

            foreach (var ovl in this.originalValueLists.Where(o => (o.Obis.C == 1 || o.Obis.C == 2) && o.Obis.D == 8))
            {
                var readings = ovl.GetReadings(accountingPeriod.Begin, accountingPeriod.End).Where(r => r.PowerValue != null).ToList();
                if (!readings.Any())
                {
                    throw new InvalidOperationException($"Keine gültigen Werte innerhalb des angegebenen Zeitbereichs.");
                }

                readings.Sort((a, b) =>
                {
                    var res = b.PowerValue.Value.CompareTo(a.PowerValue.Value);
                    if (res != 0)
                    {
                        return res;
                    }

                    return a.TargetTime.Value.CompareTo(b.TargetTime.Value);
                });

                var maxIds = new int[] { 6, 16, 26, 36, 46 };
                var minIds = new int[] { 3, 13, 23, 33, 43 };
                int idx = 0;

                foreach (var register in registers.Where(r => r.ObisCode.C == ovl.Obis.C && maxIds.Contains(r.ObisCode.D)))
                {
                    if (idx >= readings.Count)
                    {
                        break;
                    }

                    var reading = readings[idx];
                    register.SourceType = ovl.MeterReading.ReadingType;
                    register.Amount = reading.PowerValue;
                    register.Timestamp = reading.TargetTime.Value;
                    idx++;
                }

                idx = readings.Count - 1;
                foreach (var register in registers.Where(r => r.ObisCode.C == ovl.Obis.C && minIds.Contains(r.ObisCode.D)))
                {
                    if (idx < 0)
                    {
                        break;
                    }

                    var reading = readings[idx];
                    register.SourceType = ovl.MeterReading.ReadingType;
                    register.Amount = reading.PowerValue;
                    register.Timestamp = reading.TargetTime.Value;
                    idx--;
                }
            }

            registers.RemoveAll(r => r.Amount == null);

            return new TafAdapterData(typeof(Taf8SummaryView), null, accountingPeriod);
        }
    }
}
