import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import { BankAccount } from '../models/bank-account';
import { AccountService } from "./account.service";
import {environment} from "../../../environments/environment";

@Injectable()
export class BankAccountService implements AccountService {

  constructor(private http: HttpClient) { }

  getAccounts$(userId : string,
               bankName: string,
               credentials: Array<[string, string]>): Observable<BankAccount[]> {
    let body = {
      UserId: userId,
      Name: bankName,
      Credentials: credentials
    };

    var response = this.http.post<BankAccount[]>(environment.api.dataProviderUrl + '/BankAccounts', body).map((res: any[]) => {
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
          LastUpdate: r.updatedOn,
          Transactions: []
        })));
      return result;
    });
    return response;

    // let result = new Array<BankAccount>();
    // result.push(
    //     new BankAccount({
    //       Label: '12-123-123032',
    //       ProviderName: 'Hapoalim',
    //       BankNumber: 12,
    //       BranchNumber: 972,
    //       AccountNumber: 130323,
    //       Balance: 2332.23,
    //       IsActive: true,
    //       LastUpdate: 12,
    //       Transactions: []
    //     }));
    //
    // return result;
  }

}
