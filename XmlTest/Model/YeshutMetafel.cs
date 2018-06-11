using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model
{
    public class YeshutMetafel
    {
        [XmlElement("KOD-MEZAHE-METAFEL")]
        public String KodMezaheYatzran { get; set; }

        [XmlElement("SHEM-METAFEL")]
        public String ShemMetafel{ get; set; }

        [XmlElement("IshKesherYeshutMetafel")]
        public IshKesher IshKesherYeshutMetafel { get; set; }
    }
}
