
import {InstitutionType} from "./institution";

export interface Provider {
  Id: string;
  UserId: string;
  Name: string;
  Type: InstitutionType;
  Accounts: Array<string>;
  Credentials: Array<[string,string]>;
}



