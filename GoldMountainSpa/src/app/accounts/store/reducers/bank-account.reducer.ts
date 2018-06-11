import * as actions from '../actions/bank-account.action';
import { createSelector, createFeatureSelector} from '@ngrx/store';
import { BankAccountsState } from "../../../shared/store/app.states";

export const initialState: BankAccountsState = { accounts: [] };
export const getBankAccountsState = createFeatureSelector<BankAccountsState>('bankAccountsState');
export const getBankAccounts = createSelector(
  getBankAccountsState,
  (state: BankAccountsState) => state.accounts
);

export function reducer(state = initialState, action: actions.All) {
  switch (action.type) {
    case actions.BANK_ACCOUNTS:
      return { accounts: action.payload };

    case actions.BANK_ACCOUNT:
      return { accounts: null };

    case actions.ADD_BANK_ACCOUNT:
      return { accounts: state.accounts.concat(action.payload) };

    default:
      return state;
  }
}
