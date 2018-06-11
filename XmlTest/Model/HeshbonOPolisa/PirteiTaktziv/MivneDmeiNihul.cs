using System.Collections.Generic;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv
{
    public class MivneDmeiNihul
    {
        [XmlElement("PerutMivneDmeiNihul")]
        public List<PerutMivneDmeiNihul> PerutMivneDmeiNihul { get; set; }
    }
}
