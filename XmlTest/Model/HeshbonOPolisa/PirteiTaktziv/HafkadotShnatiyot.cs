using System;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv
{
    public class HafkadotShnatiyot
    {
        [XmlElement("TOTAL-HAFKADOT-OVED-TAGMULIM-SHANA-NOCHECHIT")]
        public String TotalHafkadotOvedTagmulimShanaNochechit { get; set; }

        [XmlElement("TOTAL-HAFKADOT-MAAVID-TAGMULIM-SHANA-NOCHECHIT")]
        public String TotalHafkadotMaavidTagmulimShanaNochechit { get; set; }

        [XmlElement("TOTAL-HAFKADOT-PITZUIM-SHANA-NOCHECHIT")]
        public String TotalHafkadotPitzuimTagmulimShanaNochechit { get; set; }
    }
}
