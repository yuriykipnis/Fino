using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa
{
    public class Kupot
    {
        [XmlElement("Kupa")]
        public List<Kupa> Kupa { get; set; }
    }
}
