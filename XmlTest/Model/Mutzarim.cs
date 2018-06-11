using System.Collections.Generic;
using System.Xml.Serialization;

namespace MaslekaReader.Model
{
    public class Mutzarim
    {
        [XmlElement("Mutzar")]
        public List<Mutzar> Mutzar { get; set; } = new List<Mutzar>();
    }
}
