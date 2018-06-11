using System;
using System.Collections.Generic;

namespace GoldMountainShared.Models
{
    public class InstitutionDto
    {
        public String Name { get; set; } = String.Empty;
        public IEnumerable<String> Credentials { get; set; }
        public Boolean IsSupported { get; set; } = false;
        public InstitutionType Type { get; set; }
    }

    public enum InstitutionType
    {
        Bank,
        Credit,
        Insur
    }
}
