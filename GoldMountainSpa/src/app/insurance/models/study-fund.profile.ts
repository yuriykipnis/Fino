import {BasicInsurProfile} from "./basic-insur.profile";

export class StudyFundProfile extends BasicInsurProfile{
  PolicyId: string;
  EmployerName:string;
  PlanName:string;
  YearRevenue: number;
  SaverDeposit: number;
  EmployerDeposit: number;
  WithdrawalDate: string;
  PolicyOpeningDate: string;
  ValidationDate: string;

  public constructor(init?:Partial<StudyFundProfile>) {
    super();
    Object.assign(this, init);
  }
}
