using System.Xml.Serialization;

namespace MaslekaReader.Model
{
    public class Mutzar
    {
        [XmlElement("YeshutNimaansofi")]
        public YeshutNimaansofi YeshutNimaansofi { get; set; }

        [XmlElement("NetuneiMutzar")]
        public NetuneiMutzar NetuneiMutzar { get; set; }

        [XmlElement("HeshbonotOPolisot")]
        public HeshbonotOPolisot HeshbonotOPolisot { get; set; }
    }
}
