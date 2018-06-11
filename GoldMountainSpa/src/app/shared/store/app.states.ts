import { Industry } from "../../models/industry";
import { UserProfile } from "../../models/user.profile";
import {BankAccount} from "../../accounts/models/bank-account";
import {CreditAccount} from "../../accounts/models/credit-account";
import {SeInsurProfile} from "../../insurance/models/se-insur.profile";
import {StudyFundProfile} from "../../insurance/models/study-fund.profile";
import {PensionFundProfile} from "../../insurance/models/pension-fund.profile";
import {MortgageInsurProfile} from "../../insurance/models/mortgage-insur.profile";
import {ProvidentFundProfile} from "../../insurance/models/provident-fund.profile";

export interface AppState {
  industriesState: IndustriesState;
  bankAccountsState: BankAccountsState;
  creditAccountsState: CreditAccountsState;
  userProfileState: UserProfileState;
  seInsurProfilesState: SeInsurProfilesState;
  mortgageInsurProfilesState: MortgageInsurProfilesState;
  pensionFundProfilesState: PensionFundProfilesState;
  providentFundProfilesState: ProvidentFundProfilesState;
  studyFundProfilesState: StudyFundProfilesState;
}

export interface IndustriesState {
  industries: Industry[];
}

export interface UserProfileState {
  userProfile: UserProfile;
}

export interface BankAccountsState {
  accounts: BankAccount[];
}

export interface CreditAccountsState {
  accounts: CreditAccount[];
}


export interface SeInsurProfilesState {
  profiles: SeInsurProfile[];
}

export interface ProvidentFundProfilesState {
  profiles: ProvidentFundProfile[];
}

export interface MortgageInsurProfilesState {
  profiles: MortgageInsurProfile[];
}

export interface PensionFundProfilesState {
  profiles: PensionFundProfile[];
}

export interface StudyFundProfilesState {
  profiles: StudyFundProfile[];
}
