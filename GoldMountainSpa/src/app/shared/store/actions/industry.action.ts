import { Action } from '@ngrx/store';

export const INDUSTRIES = 'INDUSTRIES';

export class FetchIndustries implements Action {
  readonly type = INDUSTRIES;
}

export type All = FetchIndustries;
