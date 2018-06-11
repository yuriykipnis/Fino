using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv
{
    public class PirutPirteiHafkadaAchrona
    {
        [XmlElement("TAARICH-HAFKADA-ACHARON")]
        public String TaarichHafkadaAcharon { get; set; }

        [XmlElement("TOTAL-HAFKADA")]
        public Double? TotalHafkada { get; set; }

        [XmlElement("HAFKADA-LEHISCHON-A")]
        public String HafkadaLehisachonA { get; set; }

        [XmlElement("HAFKADA-LEHISCHON-B")]
        public String HafkadaLehisachonB { get; set; }

        [XmlElement("TAARICH-ERECH-HAFKADA")]
        public String TaarichErechHafkada { get; set; }

        [XmlElement("SUG-HAFKADA")]
        public int? SugHafkada { get; set; }

        [XmlElement("TOTAL-HAFKADA-ACHRONA")]
        public Double? TotalHafkadaAchrona { get; set; }

    }
}
