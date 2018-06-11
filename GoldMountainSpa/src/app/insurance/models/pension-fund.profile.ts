import {BasicInsurProfile} from "./basic-insur.profile";

export class PensionFundProfile extends BasicInsurProfile{
  PolicyId: string;
  EmployerName: string;
  PlanName: string;
  ExpectedRetirementSavingsNoPremium: number;
  MonthlyRetirementPensionNoPremium: number;
  ExpectedRetirementSavings: number;
  MonthlyRetirementPension: number;
  YearRevenue: number;
  SaverDeposit: number;
  EmployerDeposit: number;
  PartnerSurvivors: number;
  ChildrenSurvivors: number;
  ParentSurvivors: number;
  InvalidPension: number;
  WorkDisabilityMonthly: number;
  WorkDisabilityOneTime: number;
  PolicyOpeningDate: string;
  ValidationDate: string;

  public constructor(init?:Partial<PensionFundProfile>) {
    super();
    Object.assign(this, init);
  }
}
