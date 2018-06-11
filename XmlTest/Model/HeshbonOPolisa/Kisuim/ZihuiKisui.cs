using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.Kisuim
{
    public class ZihuiKisui
    {
        [XmlElement("MISPAR-KISUI-BE-YATZRAN")]
        public String MisparKisuiBeYatzran { get; set; }

        [XmlElement("SHEM-KISUI-YATZRAN")]
        public String ShemKisuiYatzran { get; set; }

        [XmlElement("SUG-KISUI-ETZEL-YATZRAN")]
        public int? SugKisuiEtzelYatzran { get; set; }

        [XmlElement("MISPAR-POLISA-O-HESHBON-NEGDI")]
        public String MisparPolisaOheshbonNegdi{ get; set; }

        [XmlElement("PirteiMivutah")]
        public PirteiMivutah PirteiMivutah { get; set; }

        [XmlElement("SchumeiBituahYesodi")]
        public SchumeiBituahYesodi SchumeiBituahYesodi { get; set; }

        [XmlElement("PirteiKisuiBeMutzar")]
        public PirteiKisuiBeMutzar PirteiKisuiBeMutzar { get; set; }

        [XmlElement("PirteiTosafot")]
        public PirteiTosafot PirteiTosafot { get; set; }

        [XmlElement("Mutav")]
        public Mutav Mutav { get; set; }

        [XmlElement("KisuiBKerenPensia")]
        public KisuiBKerenPensia KisuiBKerenPensia { get; set; }

        [XmlElement("Miktsoa-Isuk-Tachviv")]
        public MiktsoaIsukTachviv MiktsoaIsukTachviv { get; set; }
    }
}
