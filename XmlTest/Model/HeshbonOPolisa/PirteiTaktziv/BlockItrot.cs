using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv
{
    public class BlockItrot
    {
        [XmlElement("Yitrot")]
        public Yitrot.Yitrot Yitrot { get; set; }
    }
}
