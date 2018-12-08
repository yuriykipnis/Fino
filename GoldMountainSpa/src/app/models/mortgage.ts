export class Mortgage {
  Id: string;
  StartDate: string;
  EndDate: string;
  NextPaymentDate: string;
  OriginalAmount:number;
  DeptAmount:number;
  InterestAmount:number;
  PrepaymentCommission:number;
  InterestRate: number;
  InterestType: string;
  IndexType: string;
  InsuranceCompany: string;

  AssetCity: string;
  AssetStreet: string;
  AssetBuildingNumber: string;

  public constructor(init?:Partial<Mortgage>) {
    Object.assign(this, init);
  }
}
