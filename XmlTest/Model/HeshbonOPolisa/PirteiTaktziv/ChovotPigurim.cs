using System.Collections.Generic;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv
{
    public class ChovotPigurim
    {
        [XmlElement("ChovPigur")]
        public List<ChovPigur> ChovPigur { get; set; }
    }
}
