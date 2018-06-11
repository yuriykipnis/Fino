using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.Kisuim
{
    public class PirteiMivutah
    {
        [XmlElement("SUG-TEUDA")]
        public int? SugTeuda { get; set; }

        [XmlElement("MISPAR-ZIHUY-LAKOACH")]
        public String MisparZihuyLakoach { get; set; }
    }
}
