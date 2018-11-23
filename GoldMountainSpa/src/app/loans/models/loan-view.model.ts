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
  LastPaymentAmount:number;
  PrepaymentCommission:number;
  InterestType: string;
  LinkageType: string;
  InsuranceCompany: string;

  public constructor(init?:Partial<LoanViewModel>) {
    Object.assign(this, init);
  }
}
