import { Action } from '@ngrx/store';
import { BankAccount } from "../../models/bank-account";

export const ADD_BANK_ACCOUNT = 'ADD_BANK_ACCOUNT';
export const BANK_ACCOUNTS = 'BANK_ACCOUNTS';
export const BANK_ACCOUNT = 'BANK_ACCOUNT';

export class FetchBankAccounts implements Action {
  readonly type = BANK_ACCOUNTS;

  constructor(public payload: BankAccount[]) {
  }
}

export class FetchBankAccount implements Action {
  readonly type = BANK_ACCOUNT;

  // constructor(public accountId: string) {
  // }
}

export class AddBankAccount implements Action {
  readonly type = ADD_BANK_ACCOUNT;

  constructor(public payload: BankAccount) {
  }
}

export type All = FetchBankAccounts | FetchBankAccount | AddBankAccount;
