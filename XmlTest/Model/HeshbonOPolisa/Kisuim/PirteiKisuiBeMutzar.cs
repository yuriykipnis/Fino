using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.Kisuim
{
    public class PirteiKisuiBeMutzar
    {
        [XmlElement("KOD-MIUTZAR-LAKISUY")]
        public String KodMiutzarLakisuy { get; set; }

        [XmlElement("SUG-MEVUTACH")]
        public int? SugMivutach { get; set; }

        [XmlElement("SUG-KISUY-BITOCHI")]
        public int? SugKisuyBitochi { get; set; }

        [XmlElement("TAARICH-TCHILAT-KISUY")]
        public String TaarichTchilatKisuy { get; set; }

        [XmlElement("TAARICH-TOM-KISUY")]
        public String TaarichTomKisuy { get; set; }

        [XmlElement("KOD-NISPACH-KISUY")]
        public String KodNispachKisuy { get; set; }

        [XmlElement("SUG-ISUK")]
        public String SugIsuk { get; set; }

        [XmlElement("KOLEL-PRENZISA")]
        public int? KolelPrenzisa{ get; set; }

        [XmlElement("TAARICH-HAFSAKAT-TASHLUM")]
        public String Pirtei { get; set; }

        [XmlElement("ACHUZ-ME-SCM-BTH-YESODI")]
        public String AchuzMeScmBthYesodi { get; set; }

        [XmlElement("ACHUZ-MESACHAR")]
        public String AchuzMeshachar { get; set; }

        [XmlElement("OFEN-TASHLUM-SCHUM-BITUACH")]
        public int? OfenTashlumChumBituach { get; set; }

        [XmlElement("SCHUM-BITUACH")]
        public Double? SchumBituach{ get; set; }

        [XmlElement("MESHALEM-HAKISUY")]
        public int? MeshalemHakisuy { get; set; }

        [XmlElement("KOD-ISHUN")]
        public int? KodIshun { get; set; }

        [XmlElement("IND-CHITUM")]
        public int? IndChitum { get; set; }

        [XmlElement("TAARICH-CHITUM")]
        public String TaarichChitum { get; set; }

        [XmlElement("HACHRAGA")]
        public int? Hachraga { get; set; }

        [XmlElement("SUG-HACHRAGA")]
        public String SugHachraga { get; set; }

        [XmlElement("TKUFAT-ACHSHARA")]
        public String TkufatAchshara{ get; set; }

        [XmlElement("TKUFAT-HAMTANA-CHODASHIM")]
        public int? TkufatHamtanaChodashim { get; set; }

        [XmlElement("HANACHA")]
        public int? Hanacha { get; set; }

        [XmlElement("HATNAYA-LAHANACHA-DMEI-BITUAH")]
        public int? HatnayaLahanachaDmeiBituah { get; set; }

        [XmlElement("DMEI-BITUAH-LETASHLUM-BAPOAL")]
        public Double? DmeiBituahLetashlumBapoal { get; set; }

        [XmlElement("SUG-HANACHA-KISUY")]
        public int? SugHanachKisuy { get; set; }

        [XmlElement("SHIUR-HANACHA-BEKISUI")]
        public Double? ShiurHanachaBekisui { get; set; }

        [XmlElement("ERECH-HANACHA-BEKISUI")]
        public String ErechHanachaBekisui { get; set; }

        [XmlElement("TADIRUT-SHINUY-DMEI-HABITUAH-BESHANIM")]
        public String TadirutShinuyDmeiHabituahBeshanim { get; set; }

        [XmlElement("TAARICH-IDKUN-HABA-SHEL-DMEI-HABITUAH")]
        public String TaarichIdkunNabaShelDmeiHabituah { get; set; }
    }
}
