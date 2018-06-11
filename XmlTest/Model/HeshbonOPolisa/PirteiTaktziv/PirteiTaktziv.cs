using System.Collections.Generic;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv
{
    public class PirteiTaktziv
    {
        [XmlElement("PirteiOved")]
        public PirteiOved PirteiOved { get; set; }

        [XmlElement("PirteiHaasaka")]
        public PirteiHaasaka PirteiHaasaka { get; set; }

        [XmlElement("PerutHafrashotLePolisa")]
        public List<PerutHafrashotLePolisa> PerutHafrashotLePolisa { get; set; }

        [XmlElement("PerutMasluleiHashkaa")]
        public List<PerutMasluleiHashkaa> PerutMasluleiHashkaa { get; set; }

        [XmlElement("NetuneiGvia")]
        public NetuneiGvia NetuneiGvia { get; set; }

        [XmlElement("PirteiHafkadaAchrona")]
        public PirteiHafkadaAchrona PirteiHafkadaAchrona { get; set; }

        [XmlElement("HafkadotShnatiyot")]
        public HafkadotShnatiyot HafkadotShnatiyot { get; set; }

        [XmlElement("ChovotPigurim")]
        public ChovotPigurim ChovotPigurim { get; set; }

        [XmlElement("PerutHotzaot")]
        public PerutHotzaot PerutHotzaot { get; set; }

        [XmlElement("PerutYitrotLesofShanaKodemet")]
        public PerutYitrotLesofShanaKodemet PerutYitrotLesofShanaKodemet { get; set; }

        [XmlElement("BlockItrot")]
        public BlockItrot BlockItrot { get; set; }
    }
}
