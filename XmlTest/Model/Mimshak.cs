using System.Xml.Serialization;

namespace MaslekaReader.Model
{
    public class Mimshak
    {
        [XmlElement("KoteretKovetz")]
        public KoteretKovetz KoteretKovetz { get; set; }

        [XmlElement("YeshutYatzran")]
        public YeshutYatzran YeshutYatzran { get; set; }

        [XmlElement("ReshumatSgira")]
        public ReshumatSgira ReshumatSgira { get; set; }
    }
}
