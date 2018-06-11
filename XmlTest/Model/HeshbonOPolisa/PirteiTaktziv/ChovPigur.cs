using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv
{
    public class ChovPigur
    {
        [XmlElement("KAYAM-CHOV-O-PIGUR")]
        public int? KayamChovOpigur { get; set; }

        [XmlElement("TAARICH-TECHILAT-PIGUR")]
        public String TaarichTechilatPigur { get; set; }

        [XmlElement("TAARICH-TECHILAT-PIGUR-NOCHECHI")]
        public String TaarichTechilatPigurNochechi { get; set; }

        [XmlElement("MISPAR-CHODSHEI-PIGUR")]
        public String MisparChodesheiPigur{ get; set; }

        [XmlElement("SUG-HOV")]
        public String SugHov { get; set; }

        [XmlElement("TOTAL-CHOVOT-O-PIGURIM")]
        public String TotalChovotOpigurim { get; set; }

        [XmlElement("KSAFIM-LO-MESHUYACHIM-MAASIK")]
        public String KsafimLoMechuyachimMaasik { get; set; }
    }
}
