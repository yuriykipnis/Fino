using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa
{
    public class YitraLefiGilPrisha
    {
        [XmlElement("GIL-PRISHA")]
        public Double? GilPrisha { get; set; }

        [XmlElement("TOTAL-CHISACHON-MITZTABER-TZAFUY")]
        public Double? TotalChisachonMitzaberTzafuy { get; set; }

        [XmlElement("TZVIRAT-CHISACHON-CHAZUYA-LELO-PREMIYOT")]
        public Double? TzviratChisachonChazuyaLeloPremiyot { get; set; }

        [XmlElement("MEKADEM-MOVTACH-LEPRISHA")]
        public Double? MekademMovtachLeprisha { get; set; }

        [XmlElement("MEKADEM-HAVTACHST-TOCHELET")]
        public Double? MekademHavtochsyTochelet { get; set; }

        [XmlElement("MEKADEM-HAVTACHST-TOCHELETPRISHA")]
        public Double? MekademHavtachstTocheletprisha { get; set; }

        [XmlElement("SHEM-MASLOL")]
        public String ShemMaslol { get; set; }

        [XmlElement("MEKADEM-HAVTACHAT-TSUA")]
        public int? MekademHavtachatTsua { get; set; }

        [XmlElement("MEKADEM-HAVTACHAT-TSUATKUFA")]
        public int? MekademHavtachatTsuatkufa { get; set; }

        [XmlElement("TKUFAT-HAGBALA-BESHANIM")]
        public String TkufatHagbalaBeshanim { get; set; }

        [XmlElement("TOCHELET-MASHPIA-KITZBA")]
        public int? TocheletMashpiaKitzba { get; set; }

        [XmlElement("TSUA-MASHPIA-KITZBA")]
        public int? TsuaMashpiaKitzba { get; set; }

        [XmlElement("SHEUR-PNS-ZIKNA-TZFUYA")]
        public Double? SheurPnsZiknaTzfuya { get; set; }

        [XmlElement("Kupot")]
        public Kupot Kupot { get; set; }
    }
}
