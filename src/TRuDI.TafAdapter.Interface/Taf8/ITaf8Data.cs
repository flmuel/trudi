namespace TRuDI.TafAdapter.Interface.Taf8
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface used for the TAF-8 data (Erfassung von Extremwerten).
    /// </summary>
    public interface ITaf8Data : ITafData
    {
        IReadOnlyList<Taf8Register> Registers { get; }
    }
}