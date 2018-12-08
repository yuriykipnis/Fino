export class Overview {
  NetWorth: number;
  TotalMortgage: number;
  NetWorthExpenses: number[];
  NetWorthIncomes: number[];
  CashFlowExpenses: number[];
  CashFlowIncomes: number[];

  Loans: Array<number[]>;
  Mortgages: Array<number[]>;
  InstitutionOverviews: Array<InstitutionOverview>;
  MortgageOverview: LoanOverview;
  LoanOverview: LoanOverview;
  NumberOfMortgages: number;
  NumberOfLoans: number;

  public constructor(init?:Partial<Overview>) {
    Object.assign(this, init);
  }
}

export class InstitutionOverview {
  Label: string;
  ProviderName: string;
  Balance: number;

  public constructor(init?:Partial<InstitutionOverview>) {
    Object.assign(this, init);
  }
}

export class LoanOverview {
  Principal: number;
  Interest: number;

  public constructor(init?:Partial<LoanOverview>) {
    Object.assign(this, init);
  }
}
