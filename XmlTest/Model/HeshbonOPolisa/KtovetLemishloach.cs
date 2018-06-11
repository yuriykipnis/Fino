using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa
{
    public class KtovetLemishloach
    {
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
    }
}
