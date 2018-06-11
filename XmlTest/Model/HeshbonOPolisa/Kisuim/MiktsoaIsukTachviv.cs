using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.Kisuim
{
    public class MiktsoaIsukTachviv
    {
        [XmlElement("TACHVIVIM-O-ISUKIM")]
        public String TechvivimOisukim { get; set; }

        [XmlElement("KOD-MIKTZOA")]
        public String KodMiktzoa { get; set; }

        [XmlElement("TCHUM-ISUK-CHADASH")]
        public String TchumIsukChadash { get; set; }
    }
}
