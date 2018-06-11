using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv
{
    public class PirteiHaasaka
    {
        [XmlElement("KOD-CHISHUV-SACHAR-POLISA-O-HESHBON")]
        public String KodChishuvSacharPolisaOheshbon { get; set; }

        [XmlElement("SACHAR-POLISA")]
        public String SacharPolisa { get; set; }

        [XmlElement("KOD-OFEN-HATZMADA")]
        public String KodOfenHatzmada { get; set; }

        [XmlElement("TAARICH-MASKORET")]
        public String TaarichMaskoret { get; set; }

        [XmlElement("ZAKAUT-LELO-TNAI")]
        public String ZakautLeloTnai { get; set; }

        [XmlElement("SEIF-14")]
        public String Seif14 { get; set; }

        [XmlElement("TAARICH-TCHILAT-TASHLUM")]
        public String TaarichTchilatTashlumim { get; set; }
    }
}
