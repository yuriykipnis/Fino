using System.Collections.Generic;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa
{
    public class NetuneiSheerim
    {
        [XmlElement("Sheer")]
        public List<Sheer> Sheer { get; set; }
    }
}
