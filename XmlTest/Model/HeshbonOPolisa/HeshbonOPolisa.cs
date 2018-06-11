using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa
{
    public class HeshbonOPolisa
    {
        //[XmlElement("ASMACHTA-MEKORIT")]
        //public String AsmachtaMekorit { get; set; }

        [XmlElement("MISPAR-POLISA-O-HESHBON")]
        public String MisparPolisaOheshbon { get; set; }

        [XmlElement("SHEM-TOCHNIT")]
        public String ShemTohnit { get; set; }

        [XmlElement("KIDOD-ACHID")]
        public String KidodAchid { get; set; }

        [XmlElement("MPR-MEFITZ-BE-YATZRAN")]
        public String MprMefitzBeYatzran { get; set; }

        [XmlElement("TAARICH-NECHONUT")]
        public String TaarichNechonut { get; set; }

        [XmlElement("TAARICH-HITZTARFUT-MUTZAR")]
        public String TaarichHitztarfutMutzar { get; set; }

        [XmlElement("TAARICH-HITZTARFUT-RISHON")]
        public String TaarichHitztarfutRishon { get; set; }

        [XmlElement("SUG-KEREN-PENSIA")]
        public String SugKerenPensia { get; set; }

        [XmlElement("PENSIA-VATIKA-O-HADASHA")]
        public String PensiaVatikaOhadasha { get; set; }

        [XmlElement("TAARICH-IDKUN-STATUS")]
        public String TaarichIdkunStatus { get; set; }

        [XmlElement("STATUS-POLISA-O-CHESHBON")]
        public int? StatusPolisaOcheshbon { get; set; }

        [XmlElement("TAARICH-TCHILA-RISK-ZMANI")]
        public String TaarichTchilaRiskZmani { get; set; }

        [XmlElement("TOM-TOKEF-RISK-ZMANI")]
        public String TomTokefRiskZmani { get; set; }

        [XmlElement("SUG-POLISA")]
        public int? SugPolisa { get; set; }

        [XmlElement("SUG-TOCHNIT-O-CHESHBON")]
        public int? SugTochnitOcheshbon { get; set; }

        [XmlElement("MADAD-BASIS")]
        public String MadadBasis { get; set; }

        [XmlElement("AZMADA-LEALVAHA")]
        public int? AzmadaLealvaha { get; set; }

        [XmlElement("TAARICH-ACHRON-MOTAV-MUVET")]
        public String TaarichAchtonMotavMuvet { get; set; }

        [XmlElement("KOLEL-ZAKAUT-AGACH")]
        public int? KolelZakautAgach { get; set; }

        [XmlElement("SHIOR-AGACH-MEUADOT")]
        public String ShiorAgachMeuadot { get; set; }

        [XmlElement("AVTACHT-TESOA")]
        public int? AvtachtTesoa { get; set; }

        [XmlElement("TAARICH-CIUM-AVTACHT-TESOA")]
        public String ATaarichCiumAvtachtTesoa { get; set; }

        [XmlElement("MISPAR-GIMLAOT")]
        public String MisparGimlaot { get; set; }

        [XmlElement("KAYAM-KISUY-HIZONI")]
        public int? KayamKisuyHizoni { get; set; }

        [XmlElement("KISUY-ISHY-KVOZATI")]
        public String KisuyIshyKvozati { get; set; }

        [XmlElement("KtovetLemishloach")]
        public KtovetLemishloach KtovetLemishloach { get; set; }

        [XmlElement("NituneiAmitOmevutach")]
        public NituneiAmitOmevutach NituneiAmitOmevutach { get; set; }

        [XmlElement("NetuneiSheerim")]
        public NetuneiSheerim NetuneiSheerim { get; set; }

        [XmlElement("PerutShiabudIkul")]
        public PerutShiabudIkul PerutShiabudIkul { get; set; }

        [XmlElement("Halvaa")]
        public Halvaa Halvaa { get; set; }

        [XmlElement("PirteyTvia")]
        public PirteyTvia PirteyTvia { get; set; }

        [XmlElement("YitraLefiGilPrisha")]
        public YitraLefiGilPrisha YitraLefiGilPrisha { get; set; }

        [XmlElement("Tsua")]
        public Tsua Tsua { get; set; }

        [XmlElement("PirteiTaktziv")]
        public PirteiTaktziv.PirteiTaktziv PirteiTaktziv { get; set; }

        [XmlElement("PerutMeyupeKoach")]
        public PerutMeyupeKoach PerutMeyupeKoach { get; set; }

        [XmlElement("Kisuim")]
        public List<Kisuim.Kisuim> Kisuim { get; set; }
    }
}
