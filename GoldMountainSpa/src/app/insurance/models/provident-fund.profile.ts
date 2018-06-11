import {BasicInsurProfile} from "./basic-insur.profile";

export class ProvidentFundProfile extends BasicInsurProfile{
  PolicyId: string;
  PolicyOpeningDate: string;
  ValidationDate: string;

  public constructor(init?:Partial<ProvidentFundProfile>) {
    super();
    Object.assign(this, init);
  }
}
