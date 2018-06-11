using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv.Yitrot
{
    public class PerutYitraLeTkufa
    {
        [XmlElement("KOD-TECHULAT-SHICHVA")]
        public int? KodTechulatShichva { get; set; }

        [XmlElement("REKIV-ITRA-LETKUFA")]
        public int? RekivItraLetkufa { get; set; }

        [XmlElement("SUG-ITRA-LETKUFA")]
        public int? SugItraLetkufa { get; set; }

        [XmlElement("SACH-ITRA-LESHICHVA-BESHACH")]
        public Double? SachItraLeshichvaBeshach { get; set; }
    }
}
