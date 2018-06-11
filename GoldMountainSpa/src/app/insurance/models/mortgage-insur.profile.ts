import {BasicInsurProfile} from "./basic-insur.profile";

export class MortgageInsurProfile extends BasicInsurProfile{
  PolicyId: string;
  PlanName: string;
  WorkDisabilityMonthly: number;
  WorkDisabilityOneTime: number;
  PolicyOpeningDate: string;
  ValidationDate: string;
  Coverage: MortgageCoverage[];

  public constructor(init?:Partial<MortgageInsurProfile>) {
    super();
    Object.assign(this, init);
  }
}

export class MortgageCoverage {
  CoverageName: string;
  Amount: number;
  DueDate: number;
  ActualFee: number;
}
