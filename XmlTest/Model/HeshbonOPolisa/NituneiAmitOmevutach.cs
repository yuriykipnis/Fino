using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa
{
    public class NituneiAmitOmevutach
    {
        [XmlElement("KOD-ZIHUY-LAKOACH")]
        public int? KodZihuyLakoach { get; set; }

        [XmlElement("MISPAR-ZIHUY")]
        public String MisparZihuy { get; set; }
    }
}
