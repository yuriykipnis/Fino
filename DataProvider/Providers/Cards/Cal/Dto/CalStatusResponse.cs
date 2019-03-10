using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Providers.Cards.Cal.Dto
{
    public class CalStatusResponse
    {
        public Status Status { get; set; }
    }

    public class Status
    {
        public String Description { get; set; }
        public String Message { get; set; }
        public Boolean Succeeded { get; set; }
    }
}
