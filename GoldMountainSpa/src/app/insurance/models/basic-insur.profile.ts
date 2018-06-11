export enum PolicyStatus {
  Active,
  Inactive
}

export class BasicInsurProfile {
  Id: string;
  ProviderName: string;
  //OwnerName: string;
  TotalSavings: number;
  DepositFee: number;
  SavingFee: number;
  PolicyStatus: PolicyStatus;

  public constructor(init?:Partial<BasicInsurProfile>) {
    Object.assign(this, init);
  }
}
