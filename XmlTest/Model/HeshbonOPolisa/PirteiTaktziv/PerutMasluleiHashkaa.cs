using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv
{
    public class PerutMasluleiHashkaa
    {
        [XmlElement("KOD-SUG-MASLUL")]
        public int? KodSugMaslul { get; set; }

        [XmlElement("KOD-SUG-HAFRASHA")]
        public int? KodSugHafrasha { get; set; }

        [XmlElement("ACHUZ-HAFKADA-LEHASHKAA")]
        public Double? AchuzHafkadaLehashkaa { get; set; }

        [XmlElement("SCHUM-TZVIRA-BAMASLUL")]
        public Double? SchumTzviraBamaslul { get; set; }

        [XmlElement("SHEM-MASLUL-HASHKAA")]
        public String ShemMaslulHashkaa { get; set; }

        [XmlElement("SHEUR-DMEI-NIHUL-HAFKADA")]
        public Double? SheurDmeiNihulHafkada { get; set; }

        [XmlElement("SHEUR-DMEI-NIHUL-HISACHON")]
        public Double? SheurDmeiNihulHisachon { get; set; }

        [XmlElement("DMEI-NIHUL-ACHERIM")]
        public Double? DmeiNihulAcherim { get; set; }

        [XmlElement("TSUA-NETO")]
        public Double? TsuaNeto { get; set; }

        [XmlElement("KOD-MASLUL-HASHKAA")]
        public String KodMaslulHashkaa { get; set; }


    }
}
