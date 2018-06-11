import * as actions from '../actions/provident-fund-profile.action';
import { createSelector, createFeatureSelector} from '@ngrx/store';
import { ProvidentFundProfilesState } from "../../../shared/store/app.states";

export const initialState: ProvidentFundProfilesState = { profiles: [] };
export const getProvidentFundProfilesState = createFeatureSelector<ProvidentFundProfilesState>('providentFundProfilesState');
export const getProvidentFundProfiles = createSelector(
  getProvidentFundProfilesState,
  (state: ProvidentFundProfilesState) => state.profiles
);

export function reducer(state = initialState, action: actions.All) {
  switch (action.type) {
    case actions.PROVIDENT_FUND_PROFILES:
      return { profiles: action.payload };

    case actions.PROVIDENT_FUND_PROFILE:
      return { profiles: null };

    case actions.ADD_PROVIDENT_FUND_PROFILE:
      return { profiles: state.profiles.concat(action.payload) };

    default:
      return state;
  }
}
