using System;

namespace DataProvider.Providers.Banks.Hapoalim.Dto
{
    public class HapoalimBalanceResponse
    {
        public Object Methadata { get; set; }
        public Double CurrentAccountLimitsAmount { get; set; }
        public Double WithdrawalBalance { get; set; }
        public Double CurrentBalance { get; set; }
        public Double CreditLimitUtilizationPercent { get; set; }
        public int CreditLimitUtilizationExistanceCode { get; set; }
        public Object CreditLimitDescription { get; set; }
        public Double CreditLimitAmount { get; set; }
        public String FormattedCurrentAccountLimitsAmount { get; set; }
        public String FormattedCurrentBalance { get; set; }
        public String FormattedWithdrawalBalance { get; set; }
        public String FormattedCurrentDate { get; set; }
    }
}
