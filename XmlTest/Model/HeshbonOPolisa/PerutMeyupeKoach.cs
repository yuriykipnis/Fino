using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa
{
    public class PerutMeyupeKoach
    {
        [XmlElement("KAYAM-MEYUPE-KOACH")]
        public int? KayamMeyupeKoach { get; set; }

        [XmlElement("SUG-ZIHUY ")]
        public String SugZihuy { get; set; }

        [XmlElement("MISPAR-ZIHUY")]
        public String MisparZihuy { get; set; }

        [XmlElement("SHEM-MEYUPE-KOACH")]
        public String ShemMeyupeKoach { get; set; }
    }
}
