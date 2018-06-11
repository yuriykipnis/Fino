using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa
{
    public class Kupa
    {
        [XmlElement("SUG-KUPA")]
        public int? SugKupa { get; set; }

        [XmlElement("SCHUM-KITZVAT-ZIKNA")]
        public Double? SchumKitzvatZikna{ get; set; }

        [XmlElement("KITZVAT-HODSHIT-TZFUYA")]
        public Double? KitzvatHodshitTzfuya { get; set; }

        [XmlElement("ACHUZ-TSUA-BATACHAZIT")]
        public Double? AchuzTsuaBatachazit { get; set; }

        [XmlElement("TOTAL-ITRA-TZFUYA-MECHUSHAV-LEHON-IM-PREMIOT")]
        public Double? TotalItraTzfuyaMechushavLehonImPremiot { get; set; }

        [XmlElement("TZVIRAT-CHISACHON-TZFUYA-LEHON-LELO-PREMIYOT")]
        public Double? TzviratChisachonTzfuyaLehonLeloPremiyot { get; set; }

        [XmlElement("TOTAL-SCHUM-MTZBR-TZAFUY-LEGIL-PRISHA-MECHUSHAV-LEKITZBA-IM-PREMIYOT")]
        public Double? TotalSchumMtzbrTzafuyLegilPrishaMechushavLekitzbaImPremiyot { get; set; }

        [XmlElement("TOTAL-SCHUM-MITZVTABER-TZFUY-LEGIL-PRISHA-MECHUSHAV-HAMEYOAD-LEKITZBA-LELO-PREMIYOT")]
        public Double? TotalSchumMitzvtaberTzafuyLegilPrishaMechushavHameyoadLekitzbaLeloPremiyot { get; set; }
    }
}
