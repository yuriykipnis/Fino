using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa
{
    public class PerutShiabudIkul
    {
        [XmlElement("HUTAL-SHIABUD")]
        public int? HutalShiabud { get; set; }

        [XmlElement("HUTAL-IKUL")]
        public int? HutalIkul { get; set; }

    }
}
