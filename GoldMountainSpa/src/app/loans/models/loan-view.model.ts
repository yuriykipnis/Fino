export class LoanViewModel {
  Id: string;
  BankLabel: string;
  BankName: string;
  BankNumber: string;
  BankBranchNumber: string;
  BankAccountNumber: string;

  CityName: string;
  StreetName: string;
  BuildingNumber: string;

  StartDate: string;
  PayoffDate: string;
  NextPaymentDate: string;
  OriginalAmount:number;
  DeptAmount:number;
  InterestAmount:number;
  LastPaymentAmount:number;
  PrepaymentCommission:number;
  InterestRate: number;

  InterestType: string;
  IndexType: string;
  InsuranceCompany: string;


  public constructor(init?:Partial<LoanViewModel>) {
    Object.assign(this, init);
  }
}
