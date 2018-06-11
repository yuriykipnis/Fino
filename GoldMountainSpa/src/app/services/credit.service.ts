import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { RequestOptions, RequestMethod, Headers, ResponseContentType } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';
import {CreditAccount} from "../accounts/models/credit-account";
import {Transaction} from "../models/transaction";

@Injectable()
export class CreditService {

  clientApiUrl: String = 'http://localhost:5001/api';
  dataProviderUrl: String = 'http://localhost:5002/api';

  constructor(private http: HttpClient) {  }

  getAccounts$(userId : string): Observable<CreditAccount[]> {
    let url = this.clientApiUrl + '/user/' + userId + '/CreditAccounts';
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    var response = this.http.get<CreditAccount[]>(url, {headers: headers}).map((res: any[]) => {
      let result = new Array<CreditAccount>();
      res.forEach(ca => {
        var newAccount = new CreditAccount({
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
          Transactions: new Array<Transaction>(),
          IsActive: true,
          LastUpdate: ca.updatedOn
        });

        ca.transactions.forEach(t => {
          newAccount.Transactions
            .push(new Transaction({
              Id: t.id,
              PaymentDate: t.paymentDate,
              PurchaseDate: t.purchaseDate,
              Description: t.description,
              Amount: t.amount,
              CurrentBalance: 0,
              Type: t.type
            }))
        });
        result.push(newAccount);
      });
      return result;
    });

    return response;
  }

  getTransactions$(accountId : string, period : Date): Observable<Transaction[]> {
    let year = period.getFullYear();
    let month = period.getMonth() + 1;
    let params = new HttpParams()
      .set('year', year.toString())
      .set('month', month.toString());
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    let url = this.clientApiUrl + '/accounts/' + accountId + '/transactions';
    var response = this.http.get<Transaction[]>(url, {params: params, headers: headers}).map((res: any) => {
      let result = new Array<Transaction>();
      res.forEach(t => result.push(
        new Transaction({
          Id: t.id,
          PaymentDate: t.paymentDate,
          PurchaseDate: t.purchaseDate,
          Description: t.description,
          Amount: t.amount,
          CurrentBalance: 0,
          Type: t.type
        })));
      return result;
    });

    return response;
  }
}
