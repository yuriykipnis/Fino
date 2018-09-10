import { Action } from '@ngrx/store';
import { Loan } from "../../../models/loan";
import {LoanViewModel} from "../../models/loan-view.model";

export const ADD_LOAN = 'ADD_LOAN';
export const LOANS = 'LOANS';
export const LOAN = 'LOAN';

export class FetchLoans implements Action {
  readonly type = LOANS;

  constructor(public payload: LoanViewModel[]) {
  }
}

export class FetchLoan implements Action {
  readonly type = LOAN;

  // constructor(public accountId: string) {
  // }
}

export class AddLoan implements Action {
  readonly type = ADD_LOAN;

  constructor(public payload: LoanViewModel) {
  }
}

export type All = FetchLoans | FetchLoan | AddLoan;
