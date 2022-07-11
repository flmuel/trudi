using System.Diagnostics;
using System.Reflection;

namespace TRuDI.Backend.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using TRuDI.HanAdapter.Interface;
    using TRuDI.Models;
    using TRuDI.Models.BasicData;

    /// <summary>
    /// Holds the XML data from the SMGW/HAN adapter.
    /// </summary>
    public class XmlDataResult
    {
        /// <summary>
        /// Gets or sets the raw XML.
        /// </summary>
        public XDocument Raw { get; set; }

        /// <summary>
        /// Gets or sets the parsed model of the XML file.
        /// </summary>
        public UsagePointAdapterTRuDI Model { get; set; }

        /// <summary>
        /// Gets or sets the a shortcut to the original value lists.
        /// </summary>
        public IReadOnlyList<OriginalValueList> OriginalValueLists { get; set; }

        /// <summary>
        /// Gets or sets the meter readings.
        /// </summary>
        public IReadOnlyList<MeterReading> MeterReadings { get; set; }

        /// <summary>
        /// Gets or sets the begin of the billing period.
        /// </summary>
        public DateTime? Begin { get; set; }

        /// <summary>
        /// Gets or sets the end of the billing period.
        /// </summary>
        public DateTime? End { get; set; }

        /// <summary>
        /// Gets the TAF identifier from the analysis profile if it exists within the data.
        /// </summary>
        public TafId? TafId => this.Model?.AnalysisProfile?.TariffUseCase;

        /// <summary>
        /// Gets the Raw XML with an XML comment element on top, containing the TRuDI Version information
        /// </summary>
        public XDocument VersionedExportXml
        {
            get
            {
                if (this.Raw != null)
                {
                    var ar = XNamespace.Get("http://vde.de/AR_2418-6.xsd");

                    var generatorVersion = this.Raw.Root.Descendants().FirstOrDefault(n => n.Name.LocalName == "GeneratorVersion");
                    if(generatorVersion == null)
                    {
                        var fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
                        var versionText = $"TRuDI {fileVersionInfo.FileVersion} on {System.Runtime.InteropServices.RuntimeInformation.OSDescription}".Trim();
                        this.Raw.Root.AddFirst(new XElement(ar + "GeneratorVersion", versionText));
                    }
                }

                return this.Raw;
            }
        }
    }
}
