using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model
{
    public class YeshutLakoach
    {
        [XmlElement("SUG-MEZAHE-LAKOACH")]
        public int? SugMezaheLakoah{ get; set; }

        [XmlElement("MISPAR-ZIHUY-LAKOACH")]
        public String MisparZihuyLakoah { get; set; }

        [XmlElement("SHEM-PRATI")]
        public String ShemPrati { get; set; }

        [XmlElement("SHEM-MISHPACHA-KODEM")]
        public String ShemMishpachaKodem { get; set; }

        [XmlElement("SHEM-MISHPACHA")]
        public String ShemMishpaha { get; set; }

        [XmlElement("MIN")]
        public int? Min { get; set; }

        [XmlElement("TAARICH-LEYDA")]
        public String TaarichLeyda { get; set; }

        [XmlElement("PTIRA")]
        public int? Ptira { get; set; }

        [XmlElement("TAARICH-PTIRA")]
        public String TaarichPtira { get; set; }

        [XmlElement("MATZAV-MISHPACHTI")]
        public int? MatzavMishpachti { get; set; }

        [XmlElement("ERETZ")]
        public String Eretz { get; set; }

        [XmlElement("SHEM-YISHUV")]
        public String ShemYisuv { get; set; }

        [XmlElement("SEMEL-YESHUV")]
        public String SemelYeshuv { get; set; }

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

        [XmlElement("MISPAR-YELADIM")]
        public int? MisparYeladim { get; set; }
    }
}
