import { Action } from '@ngrx/store';
import { UserProfile } from "../../../models/user.profile";

export const USER = 'USER';
export const SET_PROFILE = 'SET_PROFILE';

export class FetchProfile implements Action {
  readonly type = USER;
}

export class SetProfile implements Action {
  readonly type = SET_PROFILE;
  constructor(public payload: UserProfile) {
  }
}

export type All = FetchProfile | SetProfile;
