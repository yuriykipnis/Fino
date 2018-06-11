using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model
{
    public class YeshutMaasik
    {
        [XmlElement("MPR-MAASIK-BE-YATZRAN")]
        public String MprMaasikBeYatzran { get; set; }

        [XmlElement("SUG-MEZAHE-MAASIK")]
        public int? SugMezaheMaasik { get; set; }

        [XmlElement("MISPAR-MEZAHE-MAASIK")]
        public String MisparMizaheMaasik { get; set; }

        [XmlElement("MISPAR-TIK-NIKUIIM")]
        public String MisparTikNikuiim { get; set; }

        [XmlElement("SHEM-MAASIK")]
        public String ShemMaasik { get; set; }

        [XmlElement("ERETZ")]
        public String Eretz { get; set; }

        [XmlElement("SHEM-YISHUV")]
        public String ShemYisuv { get; set; }

        [XmlElement("SHEM-RECHOV")]
        public String ShemRehov { get; set; }

        [XmlElement("MISPAR-BAIT")]
        public String MisparBait { get; set; }

        [XmlElement("MISPAR-KNISA")]
        public String MissparKnisa { get; set; }

        [XmlElement("MISPAR-DIRA")]
        public String MissparDira { get; set; }

        [XmlElement("MIKUD")]
        public String Mikud { get; set; }

        [XmlElement("TA-DOAR")]
        public String TaDoar { get; set; }

        [XmlElement("MISPAR-TELEPHONE-KAVI")]
        public String MisparTelephoneKavi { get; set; }

        [XmlElement("MISPAR-SHLUCHA")]
        public String MisparShluha { get; set; }

        [XmlElement("MISPAR-CELLULARI")]
        public String MisparSellulari { get; set; }

        [XmlElement("MISPAR-FAX")]
        public String MisparFax { get; set; }

        [XmlElement("E-MAIL")]
        public String Email { get; set; }

        [XmlElement("HEAROT")]
        public String Hearot { get; set; }
    }
}
