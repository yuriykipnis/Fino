using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv
{
    public class HotzaotBafoalLehodeshDivoach
    {
        [XmlElement("SHEUR-DMEI-NIHUL-HAFKADA")]
        public Double? SheurDmeiNihulHafkada { get; set; }

        [XmlElement("TOTAL-DMEI-NIHUL-HAFKADA")]
        public Double? TotalDmeiNihulHafkada { get; set; }

        [XmlElement("SHEUR-DMEI-NIHUL-TZVIRA")]
        public Double? SheurDmeiNihulTzvira { get; set; }

        [XmlElement("TOTAL-DMEI-NIHUL-TZVIRA")]
        public Double? TotalDmeiNihulTzvira { get; set; }

        [XmlElement("SACH-DMEI-NIHUL-ACHERIM")]
        public Double? SachDmeiNihulAcherim { get; set; }

        [XmlElement("HOTZOT-NIHUL-ASHKAOT")]
        public String HotzotNihulAshkaot { get; set; }

        [XmlElement("TOTAL-DMEI-NIHUL-POLISA-O-HESHBON")]
        public Double? TotalDmeiNihulPolisaOheshbon { get; set; }

        [XmlElement("DEMI-AAVARAT-MASLOL")]
        public String DmeiAavaratMaslol { get; set; }

        [XmlElement("DMEI-NIUL-MENAEL-TIKIM")]
        public String DemiNihulMenaelTikim { get; set; }

        [XmlElement("OFEN-GEVIAT-DMEI-BITUACH")]
        public int? OfenGeviatDmeiBituach { get; set; }

        [XmlElement("SACH-DMEI-BITUAH-SHENIGBOO")]
        public Double? SachDemiBitualShnigboo { get; set; }
    }
}
