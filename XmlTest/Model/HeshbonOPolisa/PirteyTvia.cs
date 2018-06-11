using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa
{
    public class PirteyTvia
    {
        [XmlElement("YESH-TVIA")]
        public int? Mispar { get; set; }

        [XmlElement("MISPAR-TVIA-BE-YATZRAN")]
        public String MisparTviaBeYatzran { get; set; }

        [XmlElement("MISPAR-KISUI-BE-YATZRAN")]
        public String MisparKisuiBeYatzran { get; set; }

        [XmlElement("SHEM-KISUI-BE-YATZRAN")]
        public String ShemKisuiBeYatzran { get; set; }

        [XmlElement("SUG-HATVIAA")]
        public String SugHatviaa { get; set; }

        [XmlElement("OFEN-TASHLUM")]
        public String OfenTashlum { get; set; }

        [XmlElement("KOD-STATUS-TVIAA")]
        public String KodStatusTviaa { get; set; }

        [XmlElement("TAARICH-STATUS-TVIA")]
        public String TaarichStatusTvia { get; set; }

        [XmlElement("TAARICH-TECHILAT-TASHLUM")]
        public String TaarichTechilatTashlum { get; set; }

        [XmlElement("ACHUZ-MEUSHAR-O-K-A-SHICHRUR")]
        public String AchuzMeusharOkaShichrur { get; set; }

        [XmlElement("SCHUM-TVIA-MEUSHAR")]
        public String SchumTviaMeushar{ get; set; }

        [XmlElement("ACHUZ-NECHUT")]
        public String AchuzNechut { get; set; }
    }
}
