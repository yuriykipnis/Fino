using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.Kisuim
{
    public class SchumeiBituahYesodi
    {
        [XmlElement("KOD-MUTZAR-LEFI-KIDUD-ACHID-LAYESODI")]
        public String PirteyTvia { get; set; }

        [XmlElement("SUG-HATZMADA-SCHUM-BITUAH")]
        public int? SugHatzmadaShcumBituah { get; set; }

        [XmlElement("SUG-HATZMADA-DMEI-BITUAH")]
        public int? SugHatzmadaDmeiBituah { get; set; }

        [XmlElement("SUG-MASLUL-LEBITUAH")]
        public int? SugMaslulLebituah { get; set; }

        [XmlElement("IND-SCHUM-BITUAH-KOLEL-CHISACHON")]
        public String IndShcumBituahKolelChisachon { get; set; }

        [XmlElement("SCHUM-BITUACH-LEMASLUL")]
        public Double? SchumBituachLemaslul { get; set; }

        [XmlElement("MISPAR-MASKOROT")]
        public int? MisparMaskorot { get; set; }

        [XmlElement("ACHUZ-HAKTZAA-LE-CHISACHON")]
        public Double? AchuzHaktzaaLeChisachon { get; set; }

        [XmlElement("TIKRAT-GAG-HATAM-LEMIKRE-MAVET")]
        public Double? TikratGagHatamLemikreMavet { get; set; }

        [XmlElement("TIKRAT-GAG-HATAM-LE-O-K-A")]
        public String TikratGagHatamLeoka { get; set; }

        [XmlElement("SCHUM-BITUAH-LEMAVET")]
        public Double? SchumBituahLemavet { get; set; }
    }
}
