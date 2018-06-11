import { Action } from '@ngrx/store';
import {CreditAccount} from "../../models/credit-account";

export const ADD_CREDIT_ACCOUNT = 'ADD_CREDIT_ACCOUNT';
export const CREDIT_ACCOUNTS = 'CREDIT_ACCOUNTS';
export const CREDIT_ACCOUNT = 'CREDIT_ACCOUNT';

export class FetchCreditAccounts implements Action {
  readonly type = CREDIT_ACCOUNTS;

  constructor(public payload: CreditAccount[]) {
  }
}

export class FetchCreditAccount implements Action {
  readonly type = CREDIT_ACCOUNT;

  //constructor(public accountId: string) {}
}

export class AddCreditAccount implements Action {
  readonly type = ADD_CREDIT_ACCOUNT;

  constructor(public payload: CreditAccount) {
  }
}

export type All = FetchCreditAccounts | FetchCreditAccount | AddCreditAccount;
