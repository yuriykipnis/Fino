using System;
using System.Collections.Generic;

namespace DataProvider.Providers.Cards.Cal.Dto
{
    public class CalGetCardsResponse
    {
        public IEnumerable<CalAccountResponse> BankAccounts { get; set; } = new List<CalAccountResponse>();
        public CalCustomerInfoResponse CustomerInfo { get; set; }
        public CalStatusResponse Response { get; set; }
    }

    public class CalCustomerInfoResponse
    {
        public String Id { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public String UserName { get; set; }
    }
}
