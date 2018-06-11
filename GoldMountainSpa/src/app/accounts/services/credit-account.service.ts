import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import { CreditAccount } from '../models/credit-account';
import {AccountService} from "./account.service";

@Injectable()
export class CreditAccountService implements AccountService {

  dataProviderUrl: String = 'http://localhost:5002/api';

  constructor(private http: HttpClient) { }

  getAccounts$(userId : string,
               bankName: string,
               credentials: Array<[string, string]>): Observable<CreditAccount[]> {
    let body = {
      UserId: userId,
      Name: bankName,
      Credentials: credentials
    };

    var response = this.http.post<CreditAccount[]>(this.dataProviderUrl + '/CreditAccount', body).map((res: any[]) => {
      let result = new Array<CreditAccount>();
      res.forEach(ca => result.push(
        new CreditAccount({
          Id: ca.id,
          Name: ca.name,
          Label: ca.name + "-" + ca.cardNumber,
          Club: ca.club,
          UserName: ca.userName,
          CardNumber: ca.cardNumber,
          ExpirationDate: ca.expirationDate,
          BankAccount: ca.bankAccount,
          BankName: ca.bankName,
          ProviderName: ca.providerName,
          IsActive: true,
          LastUpdate: ca.updatedOn
        })));
      return result;
    });

    return response;
  }

}
