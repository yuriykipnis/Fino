using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv.Yitrot
{
    public class PerutYitrot
    {
        [XmlElement("KOD-SUG-ITRA")]
        public int? KodSugItra { get; set; }

        [XmlElement("KOD-SUG-HAFRASHA")]
        public int? KodSugHafrasha { get; set; }

        [XmlElement("TOTAL-CHISACHON-MTZBR")]
        public Double? TotalChisachonMtzbr { get; set; }

        [XmlElement("TOTAL-ERKEI-PIDION")]
        public Double? TotalErkeiPidion { get; set; }
    }
}
