using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa
{
    public class Halvaa
    {
        [XmlElement("YESH-HALVAA-BAMUTZAR")]
        public int? YeshHalvaaBamutzar { get; set; }

        [XmlElement("RAMAT-HALVAA")]
        public String RamatHalvaa { get; set; }

        [XmlElement("MISDAR-SIDURI-SHEL-HAHALVAA")]
        public String MisdarSiduriShelHahalvaa{ get; set; }

        [XmlElement("SCHUM-HALVAA")]
        public String SchumHalvaa { get; set; }

        [XmlElement("TAARICH-KABALAT-HALVAA")]
        public String TaarichKabalatHalvaa { get; set; }

        [XmlElement("YITRAT-HALVAA")]
        public String YitratHalvaa { get; set; }

        [XmlElement("TKUFAT-HALVAA")]
        public String TkufatHalvaa{ get; set; }

        [XmlElement("RIBIT")]
        public String Ribit { get; set; }

        [XmlElement("SUG-RIBIT")]
        public String SugRibit { get; set; }

        [XmlElement("SUG-HATZNMADA")]
        public String SugHatzmada { get; set; }

        [XmlElement("TADIRUT-HECHZER-HALVAA")]
        public String TadirutHechzerHalvaa { get; set; }

        [XmlElement("SUG-HECHZER")]
        public String SugHechzer { get; set; }

        [XmlElement("SCHUM-HECHZER-TKUFATI")]
        public String SchumHechzerTkufati { get; set; }
    }
}
