using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv
{
    public class PirteiOved
    {
        [XmlElement("SUG-TOCHNIT-O-CHESHBON")]
        public int? SugTochnitOcheshbon { get; set; }

        [XmlElement("MPR-MAASIK-BE-YATZRAN")]
        public String MprMaasikBeYatzran { get; set; }

        [XmlElement("STATUS-MAASIK")]
        public String StatusMaasik { get; set; }

        [XmlElement("SUG-BAAL-HAPOLISA-SHE-EINO-HAMEVUTACH")]
        public String SugBaalHapolisaSheEinoHamevutach { get; set; }

        [XmlElement("MISPAR-BAAL-POLISA-SHEEINO-MEVUTAH")]
        public String MisparBaalPolisaSheeinoMevutah { get; set; }

        [XmlElement("SHEM-BAAL-POLISA-SHEEINO-MEVUTAH")]
        public String ShemBaalPolisaSheeinoMevutah { get; set; }
    }
}
