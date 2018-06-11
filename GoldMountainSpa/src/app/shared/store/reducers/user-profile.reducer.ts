import * as actions from '../actions/user-profile.action';
import { createSelector, createFeatureSelector} from '@ngrx/store';
import { UserProfileState } from "../app.states";
import {DEV_USER, UserProfile} from "../../../models/user.profile";

export const initialState: UserProfileState = { userProfile: new UserProfile() };
export const getUserProfileState = createFeatureSelector<UserProfileState>('userProfileState');
export const getUserProfile = createSelector(
  getUserProfileState,
  (state: UserProfileState) => state.userProfile
);

export function reducer(state = initialState, action: actions.All) : UserProfileState{
  switch (action.type) {
    // case actions.USER:
    //   return { userProfile: DEV_USER };

    case actions.SET_PROFILE:
      return {
        userProfile: action.payload
      };

    default: return state;
  }
}
