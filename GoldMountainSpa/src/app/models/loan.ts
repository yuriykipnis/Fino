import { SubLoan } from "./subLoan";

export class Loan {
  Id: string;
  StartDate: string;
  EndDate: string;
  NextPaymentDate: string;
  OriginalAmount:number;
  DeptAmount:number;
  LastPaymentAmount:number;
  PrepaymentCommission:number;
  InterestType: string;
  LinkageType: string;
  InsuranceCompany: string;

  SubLoans: SubLoan[];

  public constructor(init?:Partial<Loan>) {
    Object.assign(this, init);
  }
}
