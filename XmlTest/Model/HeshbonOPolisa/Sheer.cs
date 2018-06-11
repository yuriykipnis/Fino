using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa
{
    public class Sheer
    {
        [XmlElement("SUG-ZIKA")]
        public String SugZika { get; set; }

        [XmlElement("KOD-ZIHUI-SHEERIM")]
        public String KodZihuiSheerim { get; set; }

        [XmlElement("MISPAR-ZIHUY-SHEERIM")]
        public String MisparZihuySheerim { get; set; }

        [XmlElement("SHEM-PRATI-SHEERIM")]
        public String ShemPratiSheerim { get; set; }

        [XmlElement("SHEM-MISHPACHA-SHEERIM")]
        public String ShemMishpachaSheerim { get; set; }

        [XmlElement("SHEM-MISHPAHA-KODEM")]
        public String ShemMishpoahaKodem { get; set; }

        [XmlElement("TAARICH-LEIDA")]
        public String TaarichLeida { get; set; }
    }
}
