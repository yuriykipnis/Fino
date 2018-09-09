import { ActionReducerMap, ActionReducer, MetaReducer } from '@ngrx/store';
import { AppState } from "./app.states";
import * as IndustryReducer from './reducers/industry.reducer';
import * as UserReducer from './reducers/user-profile.reducer';
import * as CreditAccountReducer from '../../accounts/store/reducers/credit-account.reducer';
import * as BankAccountReducer from '../../accounts/store/reducers/bank-account.reducer';
import * as SeInsurProfileReducer from '../../insurance/store/reducers/se-insur-profile.reducer';
import * as MortgageInsurProfileReducer from '../../insurance/store/reducers/mortgage-insur-profile.reducer';
import * as PensionFundProfileReducer from '../../insurance/store/reducers/pension-fund-profile.reducer';
import * as StudyFundProfileReducer from '../../insurance/store/reducers/study-fund-profile.reducer';
import * as ProvidentFundProfileReducer from '../../insurance/store/reducers/provident-fund-profile.reducer';
import * as LoanReducer from '../../loans/store/reducers/loan.reducer';
import { environment } from "../../../environments/environment";

export const reducers: ActionReducerMap<AppState> = {
  industriesState: IndustryReducer.reducer,
  bankAccountsState: BankAccountReducer.reducer,
  creditAccountsState: CreditAccountReducer.reducer,
  seInsurProfilesState: SeInsurProfileReducer.reducer,
  mortgageInsurProfilesState: MortgageInsurProfileReducer.reducer,
  pensionFundProfilesState: PensionFundProfileReducer.reducer,
  studyFundProfilesState: StudyFundProfileReducer.reducer,
  providentFundProfilesState: ProvidentFundProfileReducer.reducer,
  userProfileState: UserReducer.reducer,
  loansState: LoanReducer.reducer
};

export const metaReducers: MetaReducer<AppState>[] = !environment.production ? [logger] : [];

export function logger(reducer: ActionReducer<AppState>): ActionReducer<AppState> {
  return function(state: AppState, action: any): AppState {
    console.log('state', state);
    console.log('action', action);
    return reducer(state, action);
  };
}

