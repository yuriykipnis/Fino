import * as actions from '../actions/mortgage-insur-profile.action';
import { createSelector, createFeatureSelector} from '@ngrx/store';
import { MortgageInsurProfilesState } from "../../../shared/store/app.states";

export const initialState: MortgageInsurProfilesState = { profiles: [] };
export const getMortgageInsurProfilesState = createFeatureSelector<MortgageInsurProfilesState>('mortgageInsurProfilesState');
export const getMortgageInsurProfiles = createSelector(
  getMortgageInsurProfilesState,
  (state: MortgageInsurProfilesState) => state.profiles
);

export function reducer(state = initialState, action: actions.All) {
  switch (action.type) {
    case actions.MORTGAGE_INSUR_PROFILES:
      return { profiles: action.payload };

    case actions.MORTGAGE_INSUR_PROFILE:
      return { profiles: null };

    case actions.ADD_MORTGAGE_INSUR_PROFILE:
      return { profiles: state.profiles.concat(action.payload) };

    default:
      return state;
  }
}
