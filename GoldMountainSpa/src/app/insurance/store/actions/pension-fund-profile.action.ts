import { Action } from '@ngrx/store';
import { PensionFundProfile } from "../../models/pension-fund.profile";

export const ADD_PENSION_FUND_PROFILE = 'ADD_PENSION_FUND_PROFILE';
export const PENSION_FUND_PROFILES = 'PENSION_FUND_PROFILES';
export const PENSION_FUND_PROFILE = 'PENSION_FUND_PROFILE';

export class FetchPensionFundProfiles implements Action {
  readonly type = PENSION_FUND_PROFILES;

  constructor(public payload: PensionFundProfile[]) {
  }
}

export class FetchPensionFundProfile implements Action {
  readonly type = PENSION_FUND_PROFILE;

  // constructor(public accountId: string) {
  // }
}

export class AddPensionFundProfile implements Action {
  readonly type = ADD_PENSION_FUND_PROFILE;

  constructor(public payload: PensionFundProfile) {
  }
}

export type All = FetchPensionFundProfiles | FetchPensionFundProfile | AddPensionFundProfile;
