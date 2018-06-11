import { Action } from '@ngrx/store';
import { SeInsurProfile } from "../../models/se-insur.profile";

export const ADD_SE_INSUR_ACCOUNT = 'ADD_SE_INSUR_ACCOUNT';
export const SE_INSUR_ACCOUNTS = 'SE_INSUR_ACCOUNTS';
export const SE_INSUR_ACCOUNT = 'SE_INSUR_ACCOUNT';

export class FetchSeInsurProfiles implements Action {
  readonly type = SE_INSUR_ACCOUNTS;

  constructor(public payload: SeInsurProfile[]) {
  }
}

export class FetchSeInsurProfile implements Action {
  readonly type = SE_INSUR_ACCOUNT;

  // constructor(public accountId: string) {
  // }
}

export class AddSeInsurProfile implements Action {
  readonly type = ADD_SE_INSUR_ACCOUNT;

  constructor(public payload: SeInsurProfile) {
  }
}

export type All = FetchSeInsurProfiles | FetchSeInsurProfile | AddSeInsurProfile;
