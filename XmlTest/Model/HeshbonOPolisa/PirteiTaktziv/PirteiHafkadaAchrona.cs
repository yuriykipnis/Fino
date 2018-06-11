using System.Collections.Generic;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv
{
    public class PirteiHafkadaAchrona
    {
        [XmlElement("PirutPirteiHafkadaAchrona")]
        public List<PirutPirteiHafkadaAchrona> PirutPirteiHafkadaAchrona { get; set; }
    }
}
