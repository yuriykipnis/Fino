using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv.Yitrot
{
    public class Yitrot
    {
        [XmlElement("TAARICH-ERECH-TZVIROT")]
        public String TaarichErechTzvorit { get; set; }

        [XmlElement("PerutYitrot")]
        public List<PerutYitrot> PerutYitrot { get; set; }

        [XmlElement("PerutYitraLeTkufa")]
        public List<PerutYitraLeTkufa> PerutYitraLeTkufa { get; set; }

        [XmlElement("YitrotShonot")]
        public YitrotShonot YitrotShonot { get; set; }
    }
}
