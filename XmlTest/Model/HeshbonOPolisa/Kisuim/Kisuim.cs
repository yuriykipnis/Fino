using System.Collections.Generic;
using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.Kisuim
{
    public class Kisuim
    {
        [XmlElement("ZihuiKisui")]
        public List<ZihuiKisui> ZihuiKisui { get; set; }
    }
}
