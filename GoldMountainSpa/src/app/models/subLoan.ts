export class SubLoan {
  Id: string;
  OriginalAmount:number;
  PrincipalAmount:number;
  InterestAmount:number;
  DeptAmount:number;
  NextExitDate: string;
  StartDate: string;
  EndDate: string;
  InterestRate:number;

  public constructor(init?:Partial<SubLoan>) {
    Object.assign(this, init);
  }
}
