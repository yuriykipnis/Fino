using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv
{
    public class PerutYitrotLesofShanaKodemet
    {
        [XmlElement("YITRAT-SOF-SHANA")]
        public Double? YitratSofShana { get; set; }

        [XmlElement("ERECH-PIDYON-SOF-SHANA")]
        public String ErechPidyonSofShana { get; set; }

        [XmlElement("ERECH-MESOLAK-SOF-SHANA")]
        public String ErechMesolakSofShana { get; set; }

        [XmlElement("YISKON-YITRAT-KESAFIM")]
        public String YiskonYitratKesafim { get; set; }
    }
}
