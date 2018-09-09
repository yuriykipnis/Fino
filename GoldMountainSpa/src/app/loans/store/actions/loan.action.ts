import { Action } from '@ngrx/store';
import { Loan } from "../../../models/loan";

export const ADD_LOAN = 'ADD_LOAN';
export const LOANS = 'LOANS';
export const LOAN = 'LOAN';

export class FetchLoans implements Action {
  readonly type = LOANS;

  constructor(public payload: Loan[]) {
  }
}

export class FetchLoan implements Action {
  readonly type = LOAN;

  // constructor(public accountId: string) {
  // }
}

export class AddLoan implements Action {
  readonly type = ADD_LOAN;

  constructor(public payload: Loan) {
  }
}

export type All = FetchLoans | FetchLoan | AddLoan;
