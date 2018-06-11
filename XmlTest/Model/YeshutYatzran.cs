using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model
{
    public class YeshutYatzran
    {
        [XmlElement("KOD-MEZAHE-YATZRAN")]
        public String KodMezaheYatzran { get; set; }

        [XmlElement("SHEM-YATZRAN")]
        public String ShemYatzran { get; set; }

        [XmlElement("MEZAHE-LAKOACH-MISLAKA")]
        public String MezaheLakoachMislaka { get; set; }

        [XmlElement("IshKesherYeshutYatzran")]
        public IshKesher IshKesherYeshutYatzran { get; set; }

        [XmlElement("YeshutMetafel")]
        public YeshutMetafel YeshutMetafel { get; set; }

        [XmlElement("Mutzarim")]
        public Mutzarim Mutzarim { get; set; }
    }
}
