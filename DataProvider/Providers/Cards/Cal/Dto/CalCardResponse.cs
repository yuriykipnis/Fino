using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Providers.Cards.Cal.Dto
{
    public class CalCardResponse 
    {
        public String CardStatus { get; set; }
        public String CardType { get; set; }
        public Boolean CreditProductsInd { get; set; }
        public String DebitDate { get; set; }
        public Boolean GoodCardHolderOwnerInd { get; set; }
        public String Id { get; set; }
        public Boolean IsCalChoiceCard { get; set; }
        public Boolean IsEffectiveInd { get; set; }
        public Boolean IsInternalPermitted { get; set; }
        public Boolean IsIssuedByCalInd { get; set; }
        public String LastFourDigits { get; set; }
        public String NumType { get; set; }
        public String NumTypeDescription { get; set; }
        public String OwnerFirstName { get; set; }
        public String OwnerLastName { get; set; }
        public String PrivateBrandCode { get; set; }
        public String TypeCode { get; set; }
    }
}
