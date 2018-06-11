import * as actions from '../actions/industry.action';
import { createSelector, createFeatureSelector} from '@ngrx/store';
import { IndustriesState } from "../app.states";
import { INDUSTRIES } from "../../../models/industry";

export const initialState: IndustriesState = { industries: [] };
export const getIndustriesState = createFeatureSelector<IndustriesState>('industriesState');
export const getIndustries = createSelector(
  getIndustriesState,
  (state: IndustriesState) => state.industries
);

export function reducer(state = initialState, action: actions.All) : IndustriesState{
  switch (action.type) {
    case actions.INDUSTRIES:
      return { industries: INDUSTRIES };

    default:
      return state;
  }
}
