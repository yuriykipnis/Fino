using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv
{
    public class PerutHafrashotLePolisa
    {
        [XmlElement("SUG-HAMAFKID")]
        public int? SugHamafkid { get; set; }

        [XmlElement("SUG-HAFRASHA")]
        public int? SugHafrasha { get; set; }

        [XmlElement("ACHUZ-HAFRASHA")]
        public Double? AchuzHafrasha { get; set; }

        [XmlElement("SCHUM-HAFRASHA")]
        public Double? SchumHafrasha { get; set; }

        [XmlElement("TAARICH-MADAD")]
        public String TaarichMadad { get; set; }
    }
}
