import { Output, EventEmitter } from '@angular/core';
import { Injectable, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Store } from '@ngrx/store';
import { Subject, Observable, Subscription, BehaviorSubject } from 'rxjs';
import {BankAccount} from "../models/bank-account";
import {CreditAccount} from "../models/credit-account";
import {TransactionType} from "../../models/transaction";
import {AppState} from "../../shared/store/app.states";
import * as bankAccountReducer from '../store/reducers/bank-account.reducer';
import * as creditAccountReducer from '../store/reducers/credit-account.reducer';
import {Transaction} from "../../models/transaction";
import {AccountControlService} from './account-control.service';
import {AccountIdentifier, AccountType} from "../models/account-identifier";

@Injectable()
export class AccountsSummaryService implements OnInit, OnDestroy {
//-------------------------------------------------------
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
  allTransactions: Transaction[];
  effTransactions$: Observable<Transaction[]>;
  private _accountSummary = new BehaviorSubject<any>(0);
  summary$ = this._accountSummary.asObservable();
//-------------------------------------------------------

  viewPeriodSubscription: Subscription;
  bankAccountsSubscription: Subscription;
  creditAccountsSubscription: Subscription;
  period: Date;

  private periodIncomeSource = new Subject<number>();
  private periodExpenseSource = new Subject<number>();
  private periodBalanceSource = new Subject<number>();
  private totalBalanceSource = new Subject<number>();
  periodIncomeChanged$ = this.periodIncomeSource.asObservable();
  periodExpenseChanged$ = this.periodExpenseSource.asObservable();
  periodBalanceChanged$ = this.periodBalanceSource.asObservable();
  totalBalanceChanged$ = this.totalBalanceSource.asObservable();

  private bankAccounts$: Observable<BankAccount[]>;
  private creditAccounts$: Observable<CreditAccount[]>;
  private bankAccounts: BankAccount[];
  private creditAccounts: CreditAccount[];

  private periodIncome: number;
  private periodExpense: number;
  private periodBalance: number;
  private totalBalance: number;

  constructor(private store: Store<AppState>,
              private accountControlService: AccountControlService) {
    this.period = accountControlService.getViewPeriod();
    this.bankAccounts$ = store.select(bankAccountReducer.getBankAccounts);
    this.creditAccounts$ = store.select(creditAccountReducer.getCreditAccounts);

    this.subscribeToPeriod();
    this.subscribeToBankAccounts();
    this.subscribeToCreditAccounts();
  }

  ngOnInit(){
  }

  ngOnDestroy() {
    this.viewPeriodSubscription.unsubscribe();
    this.bankAccountsSubscription.unsubscribe();
    this.creditAccountsSubscription.unsubscribe();
  }

  private subscribeToBankAccounts() {
    this.bankAccountsSubscription = this.bankAccounts$.subscribe(res => {
      this.bankAccounts = res;
      this.countStatus();

      // res.forEach(ac => {
      //   ac.Transactions.forEach(t => {
      //     if (!this.allTransactions.find(tf => tf.Id == t.Id)) {
      //       this.allTransactions.push(t);
      //       let tDate = new Date(t.PaymentDate);
      //       let monthIndex = 11 - (12 * (this.period.getFullYear() - tDate.getFullYear()) + this.period.getMonth() - tDate.getMonth());
      //
      //       if (monthIndex >= 0 && monthIndex < 12) {
      //         if (isNaN(this.incomeMonthly[0][monthIndex])) this.incomeMonthly[0][monthIndex] = 0;
      //         if (isNaN(this.outcomeMonthly[0][monthIndex])) this.outcomeMonthly[0][monthIndex] = 0;
      //
      //         this.incomeMonthly[0][monthIndex] += (t.Type === TransactionType.Income) ? t.Amount : 0;
      //         this.outcomeMonthly[0][monthIndex] += (t.Type === TransactionType.Expense) ? t.Amount : 0;
      //
      //         if (monthIndex == 11){
      //             this.totalIncome += (t.Type === TransactionType.Income) ? t.Amount : 0;
      //             this.totalOutcome += (t.Type === TransactionType.Expense) ? t.Amount : 0;
      //         }
      //       }
      //     }
      //   });
      //});

      this.onStatusUpdated();
    });
  }

  private subscribeToCreditAccounts() {
    this.creditAccountsSubscription = this.creditAccounts$.subscribe(res => {
      this.creditAccounts = res;
      this.countStatus();

      // res.forEach(ac => {
      //   ac.Transactions.filter(t => {
      //     if (!this.allTransactions.find(tf => tf.Id == t.Id)) {
      //       this.allTransactions.push(t);
      //       let tDate = new Date(t.PaymentDate);
      //       let monthIndex = 11 - (12 * (this.period.getFullYear() - tDate.getFullYear()) + this.period.getMonth() - tDate.getMonth());
      //
      //       if (monthIndex >= 0 && monthIndex < 12) {
      //         if (isNaN(this.incomeMonthly[1][monthIndex])) this.incomeMonthly[1][monthIndex] = 0;
      //         if (isNaN(this.outcomeMonthly[1][monthIndex])) this.outcomeMonthly[1][monthIndex] = 0;
      //
      //         this.incomeMonthly[1][monthIndex] += (t.Type === TransactionType.Income) ? t.Amount : 0;
      //         this.outcomeMonthly[1][monthIndex] += (t.Type === TransactionType.Expense) ? t.Amount : 0;
      //
      //         if (monthIndex == 11) {
      //           this.totalInCredit += t.Amount;
      //           this.totalIncome += (t.Type === TransactionType.Income) ? t.Amount : 0;
      //           this.totalOutcome += (t.Type === TransactionType.Expense) ? t.Amount : 0;
      //         }
      //       }
      //     }
      //   });
      // })

      this.onStatusUpdated();
    });
  }

  private subscribeToPeriod() {
    this.viewPeriodSubscription = this.accountControlService.viewPeriodChanged$.subscribe(
      period => {
        this.period = period;
        this.countStatus();
        this.onStatusUpdated();
      });
  }

  countStatus() {
    this.totalBalance = 0;
    this.periodIncome = 0;
    this.periodExpense = 0;
    this.totalBalance = 0;
    this.totalInBanks = 0;
    this.totalInCredit = 0;

    if (this.bankAccounts) {
      this.bankAccounts.forEach(ba => {
        ba.Transactions.forEach(t => {
          let tDate = new Date(t.PaymentDate);
          if (this.isInCurrentPeriod(tDate)) {
            this.periodIncome += (t.Type === TransactionType.Income) ? t.Amount : 0;
            this.periodExpense += (t.Type === TransactionType.Expense) ? t.Amount : 0;
          }
        });

        let balance = !isNaN(ba.Balance) ? ba.Balance : 0;
        this.totalInBanks += balance
        this.totalBalance += ba.Balance;
      });
    }

    if (this.creditAccounts) {
      this.creditAccounts.forEach(ca => {
        ca.Transactions.forEach(t => {
          let tDate = new Date(t.PaymentDate);
          if (this.isInCurrentPeriod(tDate)) {
            this.periodIncome += (t.Type === TransactionType.Income) ? t.Amount : 0;
            this.periodExpense += (t.Type === TransactionType.Expense) ? t.Amount : 0;
          }
          if (this.isInLatestPeriod(tDate)) {
            this.totalInCredit += (t.Type === TransactionType.Income) ? t.Amount : 0;
            this.totalInCredit -= (t.Type === TransactionType.Expense) ? t.Amount : 0;
            this.totalBalance += (t.Type === TransactionType.Income) ? t.Amount : 0;
            this.totalBalance -= (t.Type === TransactionType.Expense) ? t.Amount : 0;
          }
        });
      });
    }

    this.periodBalance = this.periodIncome - this.periodExpense;
  };

  isInCurrentPeriod(date: Date) : boolean {
    return this.period.getMonth() === date.getMonth() &&
           this.period.getFullYear() === date.getFullYear();
  }

  isInLatestPeriod(date: Date) : boolean {
    let now = new Date();
    return now.getMonth() === date.getMonth() &&
      now.getFullYear() === date.getFullYear();
  }

  onStatusUpdated() {
    this.periodIncomeSource.next(this.periodIncome);
    this.periodExpenseSource.next(this.periodExpense);
    this.periodBalanceSource.next(this.periodBalance);
    this.totalBalanceSource.next(this.totalBalance);
  }

  getAllAccounts(){
    let accounts = new Array<AccountIdentifier>();
    this.bankAccounts.forEach(b => accounts.push({Id: b.Id, Type: AccountType.Bank}));
    this.creditAccounts.forEach(b => accounts.push({Id: b.Id, Type: AccountType.Credit}));
    return accounts;
  }
}
