import * as actions from '../actions/credit-account.action';
import { createSelector, createFeatureSelector} from '@ngrx/store';
import { CreditAccountsState } from "../../../shared/store/app.states";

export const initialState: CreditAccountsState = { accounts: [] };
export const getCreditAccountsState = createFeatureSelector<CreditAccountsState>('creditAccountsState');
export const getCreditAccounts = createSelector(
  getCreditAccountsState,
  (state: CreditAccountsState) => state.accounts
);

export function reducer(state = initialState, action: actions.All) : CreditAccountsState{
  switch (action.type) {
    case actions.CREDIT_ACCOUNTS:
      return { accounts: action.payload };

    case actions.CREDIT_ACCOUNT:
      return { accounts: null };

    case actions.ADD_CREDIT_ACCOUNT:
      return { accounts: state.accounts.concat(action.payload) };

    default:
      return state;
  }
}
