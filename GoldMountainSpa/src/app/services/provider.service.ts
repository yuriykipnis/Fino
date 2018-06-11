import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import {BankAccount} from "../accounts/models/bank-account";
import {CreditAccount} from "../accounts/models/credit-account";
import {CreatingAccount} from "../accounts/models/creating-account";
import {InstitutionType} from "../models/institution";
import {Transaction} from "../models/transaction";

@Injectable()
export class ProviderService {
  dataProviderUrl: String = 'http://localhost:5002/api';

  constructor(private http: HttpClient) {
  }

  addProvider$(userId: string, institutionName: string, institutionType: InstitutionType,
               credentials:Array<[string,string]>,
               bankAccounts: Array<CreatingAccount>,
               creditAccounts: Array<CreatingAccount>): any {
    let body = {
      UserId: userId,
      Name: institutionName,
      Type: institutionType,
      Credentials: credentials,
      BankAccounts: bankAccounts as BankAccount[],
      CreditAccounts: creditAccounts  as CreditAccount[],
    }

    var response = this.http.post(this.dataProviderUrl + '/provider', body).map((res: any) => {
      if (institutionType == InstitutionType.Bank) {
        return this.parseBankAccountsResponse(res);
      }
      else if (institutionType == InstitutionType.Credit) {
        return this.parseCreditAccountsResponse(res);
      }
    });

    return response;
  }

  parseBankAccountsResponse(data: any){
    let result = new Array<BankAccount>();
    let accounts = data.bankAccounts;

    accounts.forEach(r => {
      let bankAccount = new BankAccount({
        Id: r.id,
        Label: r.label,
        ProviderName: r.providerName,
        BankNumber: r.bankNumber,
        BranchNumber: r.branchNumber,
        AccountNumber: r.accountNumber,
        Balance: r.balance,
        Transactions: new Array<Transaction>(),
        IsActive: true,
        LastUpdate: r.updatedOn
      });

      r.transactions.forEach(t => {
        bankAccount.Transactions.push(new Transaction({
          Id: t.id,
          PaymentDate: t.paymentDate,
          PurchaseDate: t.purchaseDate,
          Description: t.description,
          Amount: t.amount,
          CurrentBalance: 0,
          Type: t.type
        }))
      });
      result.push(bankAccount)
    });
    return result;
  }

  parseCreditAccountsResponse(data:any){
    let result = new Array<CreditAccount>();
    let accounts = data.creditAccounts;

    accounts.forEach(r => {
      let creditAccount = new CreditAccount({
        Id: r.id,
        Name: r.name,
        Label: r.name + "-" + r.cardNumber,
        Club: r.club,
        UserName: r.userName,
        CardNumber: r.cardNumber,
        ExpirationDate: r.expirationDate,
        BankAccount: r.bankAccount,
        BankName: r.bankName,
        ProviderName: r.providerName,
        Transactions: new Array<Transaction>(),
        IsActive: true,
        LastUpdate: r.updatedOn
      });

      r.transactions.forEach(t => {
        creditAccount.Transactions.push(new Transaction({
          Id: t.id,
          PaymentDate: t.paymentDate,
          PurchaseDate: t.purchaseDate,
          Description: t.description,
          Amount: t.amount,
          CurrentBalance: 0,
          Type: t.type
        }))
      });
      result.push(creditAccount);
    });
    return result;
  }
}
