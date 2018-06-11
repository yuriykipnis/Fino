using System.Collections.Generic;
using System.Xml.Serialization;

namespace MaslekaReader.Model
{
    public class HeshbonotOPolisot
    {
        [XmlElement("HeshbonOPolisa")]
        public List<HeshbonOPolisa.HeshbonOPolisa> HeshbonOPolisa { get; set; }
    }
}
