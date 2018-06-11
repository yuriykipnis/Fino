using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model
{
    public class ReshumatSgira
    {
        [XmlElement("KAMUT-YATZRANIM")]
        public String KamutYatzranim { get; set; }

        [XmlElement("KAMUT-METAFELIM")]
        public String KamutMetafelim { get; set; }

        [XmlElement("KAMUT-MUTZARIM")]
        public String KamutMutzarim { get; set; }

        [XmlElement("KAMUT-YESHUYOT-MAASIK")]
        public String KamutYeshuyotMaasik { get; set; }

        [XmlElement("KAMUT-YESHUYOT-MEFITZ")]
        public String KamutYeshuyotMefitz { get; set; }

        [XmlElement("MISPAR-YESHUYUT-LAKOACH-BAKOVETZ")]
        public String MisparYeshuyotLakoachBakovetz { get; set; }

        [XmlElement("KAMUT-POLISOT")]
        public String KamutPolisot { get; set; }
    }
}
