import * as actions from '../actions/pension-fund-profile.action';
import { createSelector, createFeatureSelector} from '@ngrx/store';
import {PensionFundProfilesState} from "../../../shared/store/app.states";

export const initialState: PensionFundProfilesState = { profiles: [] };
export const getPensionFundProfilesState = createFeatureSelector<PensionFundProfilesState>('pensionFundProfilesState');
export const getPensionFundProfiles = createSelector(
  getPensionFundProfilesState,
  (state: PensionFundProfilesState) => state.profiles
);

export function reducer(state = initialState, action: actions.All) {
  switch (action.type) {
    case actions.PENSION_FUND_PROFILES:
      return { profiles: action.payload };

    case actions.PENSION_FUND_PROFILE:
      return { profiles: null };

    case actions.ADD_PENSION_FUND_PROFILE:
      return { profiles: state.profiles.concat(action.payload) };

    default:
      return state;
  }
}
