import { Action } from '@ngrx/store';
import {StudyFundProfile} from "../../models/study-fund.profile";

export const ADD_STUDY_FUND_PROFILE = 'ADD_STUDY_FUND_PROFILE';
export const STUDY_FUND_PROFILES = 'STUDY_FUND_PROFILES';
export const STUDY_FUND_PROFILE = 'STUDY_FUND_PROFILE';

export class FetchStudyFundProfiles implements Action {
  readonly type = STUDY_FUND_PROFILES;

  constructor(public payload: StudyFundProfile[]) {
  }
}

export class FetchStudyFundProfile implements Action {
  readonly type = STUDY_FUND_PROFILE;

  // constructor(public accountId: string) {
  // }
}

export class AddStudyFundProfile implements Action {
  readonly type = ADD_STUDY_FUND_PROFILE;

  constructor(public payload: StudyFundProfile) {
  }
}

export type All = FetchStudyFundProfiles | FetchStudyFundProfile | AddStudyFundProfile;
