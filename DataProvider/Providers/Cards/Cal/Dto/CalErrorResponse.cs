using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Providers.Cards.Cal.Dto
{
    public class CalErrorResponse
    {
        public CalError Response { get; set; }
    }

    public class CalError
    {
        public String Description { get; set; }
        public String Message { get; set; }
        public String Succeed { get; set; }
    }
}
