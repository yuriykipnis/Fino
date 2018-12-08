using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldMountainApi.Models
{
    public class LoanOverviewDto
    {
        public Decimal Principal { get; set; }
        public Decimal Interest { get; set; }
        public Decimal Commission { get; set; }
    }
}
