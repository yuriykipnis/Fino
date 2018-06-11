using System;

namespace DataProvider.Providers.Banks.Hapoalim.Dto
{
    public class HapoalimTransactionResponse
    {
        public long EventDate { get; set; }
        public string FormattedEventDate { get; set; }
        public int SerialNumber { get; set; }
        public int ActivityTypeCode { get; set; }
        public string ActivityDescription { get; set; }
        public int TextCode { get; set; }
        public long ReferenceNumber { get; set; }
        public long ValueDate { get; set; }
        public string FormattedValueDate { get; set; }
        public Double EventAmount { get; set; }
        public int EventActivityTypeCode { get; set; }
        public Double CurrentBalance { get; set; }
        public int InternalLinkCode { get; set; }
        public long OriginalEventCreateDate { get; set; }
        public string FormattedOriginalEventCreateDate { get; set; }
        public string TransactionType { get; set; }
        public int DataGroupCode { get; set; }
        public BeneficiaryDetails BeneficiaryDetailsData { get; set; }
        public long ExpandedEventDate { get; set; }
        public int ExecutingBranchNumber { get; set; }
        public long EventId { get; set; }
        public string Details { get; set; }
        public string PfmDetails { get; set; }
        public string DifferentDateIndication { get; set; }
        public string RejectedDataEventPertainingIndication { get; set; }

        public class BeneficiaryDetails
        {
            public string PartyName { get; set; }
            public string PartyHeadline { get; set; }
            public string MessageDetail { get; set; }
            public string MessageHeadline { get; set; }
        }
    }
}
