using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.Kisuim
{
    public class Mutav
    {
        [XmlElement("SUG-ZIHUY-MUTAV")]
        public int? SugZihuyMutav { get; set; }

        [XmlElement("KOD-ZIHUY-MUTAV")]
        public int? KodZihuyMutav { get; set; }

        [XmlElement("MISPAR-ZIHUY-MUTAV")]
        public String MisparZihuyMutav { get; set; }

        [XmlElement("SHEM-PRATI-MUTAV")]
        public String ShemPratiMutav { get; set; }

        [XmlElement("SHEM-MISHPACHA-MUTAV")]
        public String ShemMishpahaMutav { get; set; }

        [XmlElement("SUG-ZIKA")]
        public int? SugZika { get; set; }

        [XmlElement("ACHUZ-MUTAV")]
        public Double? AchuzMutav { get; set; }

        [XmlElement("HAGDARAT-MUTAV")]
        public int? HagdaratMutav { get; set; }

        [XmlElement("MAHUT-MUTAV")]
        public int? MahutMutav { get; set; }
    }
}
