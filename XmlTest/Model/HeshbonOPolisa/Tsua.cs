using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa
{
    public class Tsua
    {
        [XmlElement("SHEUR-TSUA-NETO")]
        public Double? SheurTsuaNeto { get; set; }

        [XmlElement("SHEUR-TSUA-BRUTO-CHS-1")]
        public String SheurTsuaBRutoChs1 { get; set; }

        [XmlElement("SHEUR-TSUA-MOVTACHAT-MEYOADOT")]
        public Double? SheurTsuaMovtachatMeyoadot { get; set; }

        [XmlElement("REVACH-HEFSED-BENIKOI-HOZAHOT")]
        public Double? RevachHefsedBenikoiHozahot { get; set; }

        [XmlElement("SIMAN-REVACH-HEFSED")]
        public String SimanRevachNefsed { get; set; }

        [XmlElement("ACHUZ-TSUA-BRUTO-CHS-2")]
        public String SheurTsuaBRutoChs2 { get; set; }
    }
}
