import {SubLoan} from "../../models/subLoan";

export class LoanViewModel {
  Id: string;
  BankLabel: string;
  BankName: string;
  BankNumber: string;
  BankBranchNumber: string;
  BankAccountNumber: string;

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

  SubLoans: SubLoan[];

  public constructor(init?:Partial<LoanViewModel>) {
    Object.assign(this, init);
  }
}
