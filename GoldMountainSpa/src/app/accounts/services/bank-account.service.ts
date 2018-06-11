import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import { BankAccount } from '../models/bank-account';
import { AccountService } from "./account.service";

@Injectable()
export class BankAccountService implements AccountService {

  clientApiUrl: String = 'http://localhost:5001/api';
  dataProviderUrl: String = 'http://localhost:5002/api';

  constructor(private http: HttpClient) { }

  getAccounts$(userId : string,
               bankName: string,
               credentials: Array<[string, string]>): Observable<BankAccount[]> {
    let body = {
      UserId: userId,
      Name: bankName,
      Credentials: credentials
    };

    var response = this.http.post<BankAccount[]>(this.dataProviderUrl + '/BankAccounts', body).map((res: any[]) => {
      let result = new Array<BankAccount>();
      res.forEach(r => result.push(
        new BankAccount({
          Id: r.id,
          Label: r.label,
          ProviderName: r.providerName,
          BankNumber: r.bankNumber,
          BranchNumber: r.branchNumber,
          AccountNumber: r.accountNumber,
          Balance: r.balance,
          IsActive: true,
          LastUpdate: r.updatedOn
        })));
      return result;
    });

    return response;
  }

}
