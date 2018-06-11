using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MaslekaReader.Model
{
    public class NetuneiMutzar
    {
        [XmlElement("KOD-MEZAHE-YATZRAN")]
        public String KodMezaheYatzran { get; set; }

        [XmlElement("KOD-MEZAHE-METAFEL")]
        public String KodMezaheMetafel { get; set; }

        [XmlElement("SUG-MUTZAR")]
        public int? SugMutzar { get; set; }

        [XmlElement("MISPAR-MISLAKA")]
        public String MisparMislaka { get; set; }

        [XmlElement("STATUS-RESHOMA")]
        public int? StatusReshoma { get; set; }

        [XmlElement("MISPAR-SHORA")]
        public int? MisparShora { get; set; }

        [XmlElement("YeshutMaasik")]
        public List<YeshutMaasik> YeshutMaasik { get; set; }

        [XmlElement("YeshutLakoach")]
        public YeshutLakoach YeshutLakoach { get; set; }
    }
}
