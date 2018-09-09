import * as actions from '../actions/loan.action';
import { createSelector, createFeatureSelector} from '@ngrx/store';
import { LoansState } from "../../../shared/store/app.states";

export const initialState: LoansState = { loans: [] };
export const getLoansState = createFeatureSelector<LoansState>('loansState');
export const getLoans = createSelector(
  getLoansState,
  (state: LoansState) => state.loans
);

export function reducer(state = initialState, action: actions.All) {
  switch (action.type) {
    case actions.LOANS:
      return { loans: action.payload };

    case actions.LOAN:
      return { loans: null };

    case actions.ADD_LOAN:
      return { loans: state.loans.concat(action.payload) };

    default:
      return state;
  }
}
