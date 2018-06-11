import { Action } from '@ngrx/store';
import { MortgageInsurProfile } from "../../models/mortgage-insur.profile";

export const ADD_MORTGAGE_INSUR_PROFILE = 'ADD_MORTGAGE_INSUR_PROFILE';
export const MORTGAGE_INSUR_PROFILES = 'MORTGAGE_INSUR_PROFILES';
export const MORTGAGE_INSUR_PROFILE = 'MORTGAGE_INSUR_PROFILE';

export class FetchMortgageInsurProfiles implements Action {
  readonly type = MORTGAGE_INSUR_PROFILES;

  constructor(public payload: MortgageInsurProfile[]) {
  }
}

export class FetchMortgageInsurProfile implements Action {
  readonly type = MORTGAGE_INSUR_PROFILE;

  // constructor(public accountId: string) {
  // }
}

export class AddMortgageInsurProfile implements Action {
  readonly type = ADD_MORTGAGE_INSUR_PROFILE;

  constructor(public payload: MortgageInsurProfile) {
  }
}

export type All = FetchMortgageInsurProfiles | FetchMortgageInsurProfile | AddMortgageInsurProfile;
