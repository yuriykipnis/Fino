using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv.Yitrot
{
    public class YitrotShonot
    {
        [XmlElement("MOED-NEZILUT-TAGMULIM")]
        public String MoedNezilutTagmulim { get; set; }

        [XmlElement("YITRAT-KASPEY-TAGMULIM")]
        public Double? YitratKaspeyTagmulim { get; set; }

        [XmlElement("TZVIRAT-PITZUIM-PTURIM-MAAVIDIM-KODMIM")]
        public String TzviratPitzuimPturimMaavidimKodmim { get; set; }

        [XmlElement("ERECH-PIDION-PITZUIM-LEKITZBA-MAAVIDIM-KODMIM")]
        public String ErechPidionPitzuimLekitzbaMaavidimKodmim { get; set; }

        [XmlElement("TZVIRAT-PITZUIM-MAAVIDIM-KODMIM-BERETZEF-KITZBA")]
        public String TzviratPitzuimMaavidimKodmimBeretzefKitzba { get; set; }

        [XmlElement("TZVIRAT-PITZUIM-MAAVIDIM-KODMIM-BERETZEF-ZECHUYOT")]
        public String TzviratPitzuimMaavidimKodmimBeretzefZechuyot { get; set; }

        [XmlElement("TZVIRAT-PITZUIM-31-12-1999-LEKITZBA")]
        public String TzviratPitzuim31121999Lekitzba { get; set; }

        [XmlElement("ERECH-PIDION-PITZUIM-MAASIK-NOCHECHI")]
        public Double? ErechPidionMaasikNochechi { get; set; }

        [XmlElement("ERECH-PIDION-MARKIV-PITZUIM-LEMAS-NOCHECHI")]
        public Double? ErechPedionMarkivPitzuimLemasNochechi { get; set; }

        [XmlElement("ERECH-PIDION-PITZUIM-MAAVIDIM-KODMIM-RETZEF-ZEHUYUT")]
        public String ErechPidionPitzuimMaavidimKodmimRetzefZehuyut { get; set; }

        [XmlElement("ERECH-PIDION-PITZUIM-LEHON-MAAVIDIM-KODMIM")]
        public String ErechPidionPitzuimLehonMaavidimKodmim { get; set; }

        [XmlElement("KAYAM-RETZEF-PITZUIM-KITZBA")]
        public int? KayamRetzefPitzuimKitzba { get; set; }

        [XmlElement("KAYAM-RETZEF-ZECHUYOT-PITZUIM")]
        public int? KayamRetzefZechuyotPitzuim { get; set; }
    }
}
