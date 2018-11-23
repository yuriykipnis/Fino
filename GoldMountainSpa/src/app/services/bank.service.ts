import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';
import { RequestOptions, RequestMethod, Headers, ResponseContentType } from '@angular/http';
import { BankAccount } from "../accounts/models/bank-account";
import { Transaction } from "../models/transaction";
import {environment} from "../../environments/environment";
import {Mortgage} from '../models/mortgage';
import {AccountService} from './account.service';
import {Loan} from "../models/loan";

@Injectable()
export class BankService implements AccountService{

  constructor(private http: HttpClient) {  }

  getAccounts$(userId : string): Observable<BankAccount[]> {
    let url = environment.api.clientApiUrl + '/user/' + userId + '/bankAccounts';
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    var response = this.http.get<BankAccount[]>(url, {headers: headers}).map((res: any[]) => {
      let result = new Array<BankAccount>();
      res.forEach(r => {
        let newAccount = new BankAccount({
          Id: r.id,
          Label: r.label,
          ProviderName: r.providerName,
          BankNumber: r.bankNumber,
          BranchNumber: r.branchNumber,
          AccountNumber: r.accountNumber,
          Balance: r.balance,
          Transactions: new Array<Transaction>(),
          Mortgages: new Array<Mortgage>(),
          Loans: new Array<Loan>(),
          IsActive: true,
          LastUpdate: r.updatedOn
        });

        r.transactions.forEach(t => {
          newAccount.Transactions
            .push(new Transaction({
              Id: t.id,
              PaymentDate: t.paymentDate,
              PurchaseDate: t.purchaseDate,
              Description: t.description,
              ProviderName: t.providerName,
              Amount: t.amount,
              CurrentBalance: 0,
              Type: t.type
            }))
        });

        r.mortgages.forEach(l => {
          let newMortgage = new Mortgage({
            Id: l.loanId,
            StartDate: l.startDate,
            EndDate: l.endDate,
            NextPaymentDate: l.nextPaymentDate,
            OriginalAmount: l.originalAmount,
            DeptAmount: l.deptAmount,
            PrepaymentCommission: l.prepaymentCommission,
            InterestType: l.interestType,
            LinkageType: l.linkageType,
            InsuranceCompany: l.insuranceCompany,
            AssetCity: l.asset.cityName,
            AssetStreet: l.asset.streetName,
            AssetBuildingNumber: l.asset.buildingNumber,
          });
          newAccount.Mortgages.push(newMortgage);
        });
          // l.subLoans.forEach(sl => {
          //   let newSubLoan = new SubLoan({
          //     Id: sl.id,
          //     OriginalAmount: sl.originalAmount,
          //     PrincipalAmount: sl.principalAmount,
          //     InterestAmount: sl.interestAmount,
          //     DeptAmount: sl.deptAmount,
          //     NextExitDate: sl.nextExitDate,
          //     StartDate: sl.startDate,
          //     EndDate: sl.endDate,
          //     InterestRate: sl.interestRate,
          //   });
          //   newMortgage.SubLoans.push(newSubLoan);
          // });

        r.loans.forEach(l => {
          let newLoan = new Loan({
            Id: l.loanId,
            StartDate: l.startDate,
            EndDate: l.endDate,
            OriginalAmount: l.originalAmount,
            DeptAmount: l.deptAmount,
            InterestRate: l.interestRate,
            NumberOfPrincipalPayments: l.numberOfPrincipalPayments,
            NumberOfInterestPayments: l.numberOfInterestPayments,
            NextPrincipalPayment: l.nextPrincipalPayment,
            NextInterestPayment: l.nextInterestPayment,
            NextPaymentDate: l.nextPaymentDate,
            NextPayment: l.nextPayment
          });
          newAccount.Loans.push(newLoan);
        });

        result.push(newAccount);
      });
      return result;
    });

    return response;
  }

  getExistingAccounts$(userId : string,
               institutionName: string,
               credentials: Array<[string, string]>): Observable<BankAccount[]> {
    let body = {
      UserId: userId,
      Name: institutionName,
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
  }

  getAccount$(accountId : string): Observable<BankAccount> {
    let url = environment.api.clientApiUrl + '/BankAccounts/' + accountId;
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    var response = this.http.get<BankAccount>(url, {headers: headers}).map((res: any) => {
      let result = new BankAccount({
          Id: res.id,
          Label: res.label,
          BankNumber: res.bankNumber,
          BranchNumber: res.branchNumber,
          AccountNumber: res.accountNumber,
          ProviderName: res.providerName,
          Balance: res.balance,
          IsActive: true,
          LastUpdate: res.updatedOn,
          Transactions: []
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

    let url = environment.api.clientApiUrl + '/accounts/' + accountId + '/transactions';
    var response = this.http.get<Transaction[]>(url, {params: params, headers: headers}).map((res: any) => {
      let result = new Array<Transaction>();
      res.forEach(t => result.push(
        new Transaction({
          Id: t.id,
          PaymentDate: t.paymentDate,
          PurchaseDate: t.purchaseDate,
          Description: t.description,
          ProviderName: t.providerName,
          Amount: t.amount,
          CurrentBalance: t.currentBalance,
          Type: t.type
        })));
      return result;
    });

    return response;
  }

  getAllTransactions$(userId : string, period : Date): Observable<Transaction[]> {
    let year = period.getFullYear();
    let month = period.getMonth() + 1;
    let params = new HttpParams()
      .set('aggregated', "true")
      .set('year', year.toString())
      .set('month', month.toString());
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    let url = environment.api.clientApiUrl + '/user/' + userId + '/transactions';
    var response = this.http.get<Transaction[]>(url, {params: params, headers: headers}).map((res: any) => {
      let result = new Array<Transaction>();
      res.forEach(t => result.push(
        new Transaction({
          Id: t.id,
          PaymentDate: t.paymentDate,
          PurchaseDate: t.purchaseDate,
          Description: t.description,
          ProviderName: t.providerName,
          Amount: t.amount,
          CurrentBalance: t.currentBalance,
          Type: t.type
        })));
      return result;
    });

    return response;
  }

  getFees$(userId : string, period : Date): Observable<number> {
    let year = period.getFullYear();
    let month = period.getMonth() + 1;
    let params = new HttpParams()
      .set('aggregated', "true")
      .set('year', year.toString())
      .set('month', month.toString());
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    let url = environment.api.clientApiUrl + '/user/' + userId + '/banks/fees';
    var response = this.http.get<number>(url, {params: params, headers: headers}).map((res: any) => {
      return res;
    });

    return response;
  }
}
