import {Observable} from "rxjs/Observable";
import {CreatingAccount} from "../models/creating-account";

export interface AccountService {
  getAccounts$(userId : string, institutionName: string,
               credentials: Array<[string, string]>): Observable<CreatingAccount[]>

}
