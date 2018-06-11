import { Action } from '@ngrx/store';
import { ProvidentFundProfile } from "../../models/provident-fund.profile";

export const ADD_PROVIDENT_FUND_PROFILE = 'ADD_PROVIDENT_FUND_PROFILE';
export const PROVIDENT_FUND_PROFILES = 'PROVIDENT_FUND_PROFILES';
export const PROVIDENT_FUND_PROFILE = 'PROVIDENT_FUND_PROFILE';

export class FetchProvidentFundProfiles implements Action {
  readonly type = PROVIDENT_FUND_PROFILES;

  constructor(public payload: ProvidentFundProfile[]) {
  }
}

export class FetchProvidentFundProfile implements Action {
  readonly type = PROVIDENT_FUND_PROFILE;

  // constructor(public accountId: string) {
  // }
}

export class AddProvidentFundProfile implements Action {
  readonly type = ADD_PROVIDENT_FUND_PROFILE;

  constructor(public payload: ProvidentFundProfile) {
  }
}

export type All = FetchProvidentFundProfiles | FetchProvidentFundProfile | AddProvidentFundProfile;
