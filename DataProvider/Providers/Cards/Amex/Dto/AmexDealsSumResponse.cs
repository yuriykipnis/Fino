using System;
using System.Collections.Generic;

namespace DataProvider.Providers.Cards.Amex.Dto
{
  public class AmexDealsSumResponse
  {
    private IList<DealsSumInfo> CardsTotal { get; set; }
    public object Stage { get; set; }
    public object ReturnCode { get; set; }
    public object Message { get; set; }
    public object DisplayProperties { get; set; }
    public int TablePageNum { get; set; }
    public bool IsError { get; set; }
    public bool IsCaptcha { get; set; }
    public bool IsButton { get; set; }
    public object SiteName { get; set; }

    public class DealsSumInfo
    {
      public string CardName { get; set; }
      public long CardNumberTail { get; set; }
      public bool IsValid { get; set; }
      public int ServiceType { get; set; }
      public int AccountBank{ get; set; }
      public int Shiuch { get; set; }
      public int PaymentDate { get; set; }
      public object TotalSumList { get; set; }
      public string StatementDate { get; set; }
      public Decimal TotalDolarCharge { get; set; }
      public Decimal TotalEuroCharge { get; set; }
      public Decimal TotalShekelCharge { get; set; }
      public Decimal TotalNormalShekelCharge { get; set; }
      public Decimal TotalPaymentsShekelCharge { get; set; }
      public Decimal TotalKPaymentsShekelCharge { get; set; }
      public Decimal TotalCreditShekelCharge { get; set; }
      public Decimal TotalShekelAbroadCharge { get; set; }
      public Decimal TotalRefundCharge { get; set; }
      public Decimal TotalDelayedCharge { get; set; }
      public Decimal FlexibleAmountCharge { get; set; }
      public Decimal OverloadAmountCharge { get; set; }
      public String OnlyCardName { get; set; }
    }
  }
}
