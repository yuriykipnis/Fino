import { Output, EventEmitter } from '@angular/core';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Store } from '@ngrx/store';
import {Observable} from 'rxjs/Observable';
import {Subscription} from 'rxjs/Subscription';
import {BehaviorSubject} from 'rxjs/BehaviorSubject';
import {BankAccount} from "../models/bank-account";
import {CreditAccount} from "../models/credit-account";
import {TransactionType} from "../../models/transaction";
import {AppState} from "../../shared/store/app.states";
import * as bankAccountReducer from '../store/reducers/bank-account.reducer';
import * as creditAccountReducer from '../store/reducers/credit-account.reducer';
import {Transaction} from "../../models/transaction";

@Injectable()
export class AccountsSummaryService {
  totalInBanks: number = 0;
  totalInCredit: number = 0;
  totalIncome: number = 0;
  totalOutcome: number = 0;

  incomeMonthly: number[][];
  outcomeMonthly: number[][];

  futureObligations: number[];

  creditOutcome: number;
  checkOutcome: number;
  cashOutcome: number;
  bankTransactionOutcome: number;

  bankAccounts$: Observable<BankAccount[]>;
  creditAccounts$: Observable<CreditAccount[]>;
  allTransactions: Transaction[];
  effTransactions$: Observable<Transaction[]>;

  private _accountSummary = new BehaviorSubject<any>(0);
  summary$ = this._accountSummary.asObservable();

  constructor(private store: Store<AppState>) {
    this.bankAccounts$ = store.select(bankAccountReducer.getBankAccounts);
    this.creditAccounts$ = store.select(creditAccountReducer.getCreditAccounts);
    this.allTransactions = new Array<Transaction>();

    this.incomeMonthly = [new Array<number>(), new Array<number>()];
    this.outcomeMonthly = [new Array<number>(), new Array<number>()];

    this.handleBankAccounts();
    this.handleCreditAccounts();
  }

  private handleBankAccounts() {
    this.bankAccounts$.subscribe(res => {
      this.totalInBanks = 0;
      let now = new Date();

      res.forEach(ac => {
        let balance = !isNaN(ac.Balance) ? ac.Balance : 0;
        this.totalInBanks += balance;

        ac.Transactions.forEach(t => {
          if (!this.allTransactions.find(tf => tf.Id == t.Id)) {
            this.allTransactions.push(t);
            let tDate = new Date(t.PaymentDate);
            let monthIndex = 11 - (12 * (now.getFullYear() - tDate.getFullYear()) + now.getMonth() - tDate.getMonth());

            if (monthIndex >= 0 && monthIndex < 12) {
              if (isNaN(this.incomeMonthly[0][monthIndex])) this.incomeMonthly[0][monthIndex] = 0;
              if (isNaN(this.outcomeMonthly[0][monthIndex])) this.outcomeMonthly[0][monthIndex] = 0;

              this.incomeMonthly[0][monthIndex] += (t.Type === TransactionType.Income) ? t.Amount : 0;
              this.outcomeMonthly[0][monthIndex] += (t.Type === TransactionType.Expense) ? t.Amount : 0;

              if (monthIndex == 11){
                  this.totalIncome += (t.Type === TransactionType.Income) ? t.Amount : 0;
                  this.totalOutcome += (t.Type === TransactionType.Expense) ? t.Amount : 0;
              }
            }
          }
        });
      })

      this.onInfoUpdated();
    });
  }

  private handleCreditAccounts() {
    this.creditAccounts$.subscribe(res => {
      let now = new Date();

      res.forEach(ac => {
        ac.Transactions.filter(t => {
          if (!this.allTransactions.find(tf => tf.Id == t.Id)) {
            this.allTransactions.push(t);
            let tDate = new Date(t.PaymentDate);
            let monthIndex = 11 - (12 * (now.getFullYear() - tDate.getFullYear()) + now.getMonth() - tDate.getMonth());

            if (monthIndex >= 0 && monthIndex < 12) {
              if (isNaN(this.incomeMonthly[1][monthIndex])) this.incomeMonthly[1][monthIndex] = 0;
              if (isNaN(this.outcomeMonthly[1][monthIndex])) this.outcomeMonthly[1][monthIndex] = 0;

              this.incomeMonthly[1][monthIndex] += (t.Type === TransactionType.Income) ? t.Amount : 0;
              this.outcomeMonthly[1][monthIndex] += (t.Type === TransactionType.Expense) ? t.Amount : 0;

              if (monthIndex == 11) {
                this.totalInCredit += t.Amount;
                this.totalIncome += (t.Type === TransactionType.Income) ? t.Amount : 0;
                this.totalOutcome += (t.Type === TransactionType.Expense) ? t.Amount : 0;
              }
            }
          }
        });
      })

      this.onInfoUpdated();
    });
  }

  onInfoUpdated() {
    this._accountSummary.next(0);
  }
}
