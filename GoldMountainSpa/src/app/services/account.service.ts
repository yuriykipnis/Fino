import {Observable} from "rxjs/Observable";
import {CreatingAccount} from "../accounts/models/creating-account";

export interface AccountService {
  getExistingAccounts$(userId : string, institutionName: string,
               credentials: Array<[string, string]>): Observable<CreatingAccount[]>

}
