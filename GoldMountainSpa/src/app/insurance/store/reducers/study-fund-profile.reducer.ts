import * as actions from '../actions/study-fund-profile.action';
import { createSelector, createFeatureSelector} from '@ngrx/store';
import { StudyFundProfilesState } from "../../../shared/store/app.states";

export const initialState: StudyFundProfilesState = { profiles: [] };
export const getStudyFundProfilesState = createFeatureSelector<StudyFundProfilesState>('studyFundProfilesState');
export const getStudyFundProfiles = createSelector(
  getStudyFundProfilesState,
  (state: StudyFundProfilesState) => state.profiles
);


export function reducer(state = initialState, action: actions.All) {
  switch (action.type) {
    case actions.STUDY_FUND_PROFILES:
      return { profiles: action.payload };

    case actions.STUDY_FUND_PROFILE:
      return { profiles: null };

    case actions.ADD_STUDY_FUND_PROFILE:
      return { profiles: state.profiles.concat(action.payload) };

    default:
      return state;
  }
}
