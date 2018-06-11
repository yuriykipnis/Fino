using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.Kisuim
{
    public class PirteiTosafot
    {
        [XmlElement("TOSEFET-TAARIF")]
        public int? TosefetTaarif { get; set; }

        [XmlElement("KOD-SUG-TOSEFET")]
        public String KodSugTosefet { get; set; }
    
        [XmlElement("SHEUR-TOSEFET")]
        public String SheurTosefet { get; set; }

        [XmlElement("PROMIL-TOSEFET")]
        public String PromilTosefet { get; set; }

        [XmlElement("TAARICH-TOM-TOSEFET")]
        public String TaarichTomTosefet { get; set; }
    }
}
