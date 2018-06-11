using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv
{
    public class PerutMivneDmeiNihul
    {
        [XmlElement("GOVA-DMEI-NIHUL-NIKBA-AL-PI-HOTZAOT-BAPOAL")]
        public int? GovaDmeiNihulNikbaAlPiHotzaotBapoal { get; set; }

        [XmlElement("SUG-HOTZAA")]
        public int? SugHotzaa { get; set; }

        [XmlElement("KOD-MASLUL-DMEI-NIHUL")]
        public String KodMaslulDmeiHihul { get; set; }

        [XmlElement("MEAFYENEI-MASLUL-DMEI-NIHULL")]
        public int? MeafyeneiMaslulDmeiNihul { get; set; }
        
        [XmlElement("SHEUR-DMEI-NIHUL")]
        public Double? SheurDmeiNihul { get; set; }

        [XmlElement("TAARICH-IDKUN-SHEUR-DNHL")]
        public String TaarichIdkunSheurDnhl { get; set; }

        [XmlElement("DMEI-NIHUL-ACHIDIM")]
        public int? DmeiNihulAchidim { get; set; }

        [XmlElement("KOD-MASLUL-HASHKAA-BAAL-DMEI-NIHUL-YECHUDIIM")]
        public String KodMaslulHashkaaBaalDmeiNihulYechudim { get; set; }

        [XmlElement("OFEN-HAFRASHA")]
        public int? OfenHafrasha { get; set; }

        [XmlElement("SCHUM-MAX-DNHL-HAFKADA")]
        public String SchumMaxDnhlHafkada { get; set; }

        [XmlElement("SACH-DMEI-NIHUL-MASLUL")]
        public String SachDmeiNihulMaslul { get; set; }

        [XmlElement("DMEI-NIHUL-ACHERIM")]
        public Double? DmeiNihulAcherim { get; set; }

        [XmlElement("KENAS-MESHICHAT-KESAFIM")]
        public int? KenasMeshichatKesafim { get; set; }

        [XmlElement("KAYEMET-HATAVA")]
        public int? KayemetHatava { get; set; }

        [XmlElement("SUG-HATAVA")]
        public int? SugHatava { get; set; }

        [XmlElement("ACHOZ-HATAVA")]
        public String AchozHatava { get; set; }

        [XmlElement("TAARICH-SIUM-HATAVA")]
        public String TaarichSiumHatava { get; set; }
    }
}
