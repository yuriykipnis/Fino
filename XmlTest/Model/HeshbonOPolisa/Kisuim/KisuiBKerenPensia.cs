using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.Kisuim
{
    public class KisuiBKerenPensia
    {
        [XmlElement("ALUT-KISUI-NECHUT")]
        public String AlutKisuiNechut { get; set; }

        [XmlElement("ALUT-KISUI-PNS-SHRM-NECHE")]
        public String AlutKisuiPnsShrmNeche { get; set; }

        [XmlElement("SHEUR-KISUY-NECHUT")]
        public String SheurKisuyNechut { get; set; }

        [XmlElement("SACHAR-KOVEA-LE-NECHUT-VE-SHEERIM")]
        public String SacharKoveaLeNechutVeSheerim { get; set; }

        [XmlElement("TAARICH-MASKORET-NECHUT-VE-SHEERIM")]
        public String TaarichMaskoretNechutVeSheerim { get; set; }

        [XmlElement("SUG-VITOR-SHAERIM")]
        public String SugVitorShaerim { get; set; }

        [XmlElement("SACH-PENSIAT-NECHUT")]
        public String SachPensiatNechut { get; set; }

        [XmlElement("ALUT-KISUY-SHEERIM")]
        public String AlutKisuySheerim { get; set; }

        [XmlElement("SHIUR-KISUY-YATOM ")]
        public String ShiurKisuyYatom { get; set; }

        [XmlElement("KITZBAT-SHEERIM-LEALMAN-O-ALMANA")]
        public String KitzbatSheerimLealmanOalmana { get; set; }

        [XmlElement("KITZBAT-SHEERIM-LEYATOM")]
        public String KitzbatSheerimLeyatom { get; set; }

        [XmlElement("KITZBAT-SHEERIM-LEHORE-NITMACH")]
        public String KitzbatSheerimLehoreNitmach { get; set; }

        [XmlElement("TAARICH-VITOR-SHEERIM")]
        public String TaarichVitorSheerim { get; set; }

        [XmlElement("TAARICH-CIUM-VITOR-SEERIM")]
        public String TaarichCiumVitorSheerim { get; set; }

        [XmlElement("SHIUR-KISUY-ALMAN-O-ALMANA")]
        public String ShiurKisuyAlmanOalmana { get; set; }

        [XmlElement("SHIUR-KISUY-HORE-NITMACH")]
        public String ShiurKisuyHoreNitmach { get; set; }

        [XmlElement("GIL-PRISHA-LEPENSIYAT-ZIKNA")]
        public String GilPrishaLEpensiyatZikna { get; set; }

        [XmlElement("SACH-PENSIYAT-ALMAN-O-ALMANA")]
        public String SachPensiyatAlmanOalmana { get; set; }

        [XmlElement("MISPAR-HODSHEI-HAVERUT-BEKEREN-HAPENSIYA")]
        public String MisparHodsheiHaverutBekerenHapensiya { get; set; }

        [XmlElement("MISPAR-HODSHEI-HAVERUT-MITZ-BEKEREN-HAPENSIYA")]
        public String MisparHodsheiHaverutMitzBekerenHapensiya { get; set; }

        [XmlElement("MENAT-PENSIA-TZVURA")]
        public String MenatPansiaTzvura { get; set; }

        [XmlElement("AHUZ-PENSIYA-TZVURA")]
        public String AhuzPansiaTzvura { get; set; }

        [XmlElement("TAARICH-TCHILAT-HAVERUT")]
        public String TaarichTchilatHaverut { get; set; }

        [XmlElement("TAARICH-ERECH-LANENTUNIM")]
        public String TaarichErechLanentunim { get; set; }

        [XmlElement("HATAVA-BITUCHIT")]
        public String HAtavaRituchit { get; set; }
    }
}
