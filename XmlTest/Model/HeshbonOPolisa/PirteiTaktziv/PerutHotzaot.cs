using System.Xml.Serialization;

namespace MaslekaReader.Model.HeshbonOPolisa.PirteiTaktziv
{
    public class PerutHotzaot
    {
        [XmlElement("HotzaotBafoalLehodeshDivoach")]
        public HotzaotBafoalLehodeshDivoach HotzaotBafoalLehodeshDivoach { get; set; }

        [XmlElement("MivneDmeiNihul")]
        public MivneDmeiNihul MivneDmeiNihul { get; set; }
    }
}
