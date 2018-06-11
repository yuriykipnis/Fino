using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv
{
    public class NetuneiGvia
    {
        [XmlElement("SHEM-MESHALEM")]
        public String ShemMeshalem { get; set; }

        [XmlElement("SUG-TEUDA-MESHALEM")]
        public int? SugTeudaMeshalem { get; set; }

        [XmlElement("MISPAR-ZIHUY-MESHALEM")]
        public String MisparZihuyMeshalem { get; set; }

        [XmlElement("KOD-EMTZAEI-TASHLUM")]
        public int? KodImtzaeiTashlum { get; set; }

        [XmlElement("TADIRUT-TASHLUM")]
        public int? TadirutTachlum { get; set; }

        [XmlElement("CHODESH-YECHUS")]
        public int? ChodeshYechus { get; set; }

        [XmlElement("YOM-GVIYA-BECHODESH")]
        public int? YomGviyaBechodesh { get; set; }

        [XmlElement("OFEN-HATZMADAT-GVIA")]
        public int? OfenHatzmadaGvia { get; set; }

        [XmlElement("ACHUZ-TAT-SHNATIYOT")]
        public Double? AchuzTatShnatiyot { get; set; }
    }
}
