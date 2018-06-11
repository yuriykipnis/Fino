import {BasicInsurProfile} from "./basic-insur.profile";

export class SeInsurProfile extends BasicInsurProfile{
  PolicyId: string;
  EmployerName: string;
  PlanName: string;
  ExpectedRetirementSavingsNoPremium: number;
  MonthlyRetirementPensionNoPremium: number;
  ExpectedRetirementSavings: number;
  MonthlyRetirementPension: number;
  YearRevenue: number;
  DeathInsuranceMonthlyAmount: number;
  DeathInsuranceAmount: number;
  PolicyOpeningDate: string;
  ValidationDate: string;

  public constructor(init?:Partial<SeInsurProfile>) {
    super();
    Object.assign(this, init);
  }
}
