using System;

namespace DataProvider.Providers.Banks.Hapoalim.Dto
{
    public class HapoalimBalanceResponse
    {
        public Object Methadata { get; set; }
        public Decimal CurrentAccountLimitsAmount { get; set; }
        public Decimal WithdrawalBalance { get; set; }
        public Decimal CurrentBalance { get; set; }
        public Decimal CreditLimitUtilizationPercent { get; set; }
        public int CreditLimitUtilizationExistanceCode { get; set; }
        public Object CreditLimitDescription { get; set; }
        public Decimal CreditLimitAmount { get; set; }
        public String FormattedCurrentAccountLimitsAmount { get; set; }
        public String FormattedCurrentBalance { get; set; }
        public String FormattedWithdrawalBalance { get; set; }
        public String FormattedCurrentDate { get; set; }
    }
}
