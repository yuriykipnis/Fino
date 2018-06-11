using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model
{
    public class YeshutNimaansofi
    {
        [XmlElement("KOD-NIMAAN")]
        public int? KodNimaan { get; set; }

        [XmlElement("SUG-MEZAHE-NIMAAN")]
        public int? SugNimaan { get; set; }

        [XmlElement("MISPAR-ZIHUI-NIMAAN")]
        public String MisparZihuiNimaan { get; set; }

        [XmlElement("MISPAR-ZIHUI-ETZEL-YATZRAN-NIMAAN")]
        public String MisparZihuiEtzelYatzranNimaan { get; set; }
    }
}
