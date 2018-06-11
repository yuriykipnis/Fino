using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model
{
    public class KoteretKovetz
    {
        [XmlElement("SUG-MIMSHAK")]
        public int SugMimshak { get; set; }

        [XmlElement("MISPAR-GIRSAT-XML")]
        public String MisparGirsatXml { get; set; }

        [XmlElement("TAARICH-BITZUA")]
        public String TaarichBitzua { get; set; }

        [XmlElement("KOD-SVIVAT-AVODA")]
        public int KodSvivatAvoda { get; set; }

        [XmlElement("KIVUN-MIMSHAK-XML")]
        public int KivunMimshakXml { get; set; }

        [XmlElement("KOD-SHOLEACH")]
        public String KodSholeah { get; set; }

        [XmlElement("SHEM-SHOLEACH")]
        public String ShemSholeah { get; set; }

        [XmlElement("KOD-MEZAHE-METAFEL")]
        public String KodMezaheMetafel { get; set; }

        [XmlElement("SHEM-METAFEL")]
        public String ShemMetafel { get; set; }

        [XmlElement("MEZAHE-HAAVARA")]
        public String MezaheHaavara { get; set; }

        [XmlElement("MISPAR-HAKOVETZ")]
        public String MisparHakovez { get; set; }
    }
}
