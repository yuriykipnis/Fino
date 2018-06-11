import * as actions from '../actions/se-insur-profile.action';
import { createSelector, createFeatureSelector} from '@ngrx/store';
import { SeInsurProfilesState } from "../../../shared/store/app.states";

export const initialState: SeInsurProfilesState = { profiles: [] };
export const getSeInsurProfilesState = createFeatureSelector<SeInsurProfilesState>('seInsurProfilesState');
export const getSeInsurProfiles = createSelector(
  getSeInsurProfilesState,
  (state: SeInsurProfilesState) => state.profiles
);

export function reducer(state = initialState, action: actions.All) {
  switch (action.type) {
    case actions.SE_INSUR_ACCOUNTS:
      return { profiles: action.payload };

    case actions.SE_INSUR_ACCOUNT:
      return { profiles: null };

    case actions.ADD_SE_INSUR_ACCOUNT:
      return { profiles: state.profiles.concat(action.payload) };

    default:
      return state;
  }
}
