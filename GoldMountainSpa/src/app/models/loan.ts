
export class Loan {
  Id: string;
  StartDate: string;
  EndDate: string;
  OriginalAmount: number;
  DeptAmount: number;
  InterestRate: number;

  NumberOfPrincipalPayments: number;
  NumberOfInterestPayments: number;
  NextPrincipalPayment: number;
  NextInterestPayment: number;

  NextPayment: number;
  NextPaymentDate: string;

  public constructor(init?:Partial<Loan>) {
    Object.assign(this, init);
  }
}
