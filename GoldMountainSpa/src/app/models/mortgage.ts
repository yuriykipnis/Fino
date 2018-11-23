import { SubLoan } from "./subLoan";

export class Mortgage {
  Id: string;
  StartDate: string;
  EndDate: string;
  NextPaymentDate: string;
  OriginalAmount:number;
  DeptAmount:number;
  PrepaymentCommission:number;
  InterestType: string;
  LinkageType: string;
  InsuranceCompany: string;

  AssetCity: string;
  AssetStreet: string;
  AssetBuildingNumber: string;

  public constructor(init?:Partial<Mortgage>) {
    Object.assign(this, init);
  }
}
