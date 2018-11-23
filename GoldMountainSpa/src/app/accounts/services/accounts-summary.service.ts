import { Output, EventEmitter } from '@angular/core';
import { Injectable } from '@angular/core';
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
import {BankService} from "../../services/bank.service";
import {CreditService} from "../../services/credit.service";
import {UserProfileService} from "../../services/user-profile.service";
import {UserProfile} from "../../models/user.profile";
import {InstitutionType} from '../../models/institution';

@Injectable()
export class AccountsSummaryService{
  private period: Date;
  private userProfile: UserProfile;

  private periodIncome: number;
  private periodExpense: number;
  private periodBalance: number;
  private totalBalance: number;
  private periodBankFees: number;
  private incomeMonthly: number[][];
  private expenseMonthly: number[][];
  private totalInBanks: number = 0;
  private totalInCredit: number = 0;

  private periodIncomeSource = new Subject<number>();
  private periodExpenseSource = new Subject<number>();
  private periodBalanceSource = new Subject<number>();
  private totalBalanceSource = new Subject<number>();
  private bankFeeSource = new Subject<number>();
  private monthlyBalanceSource = new Subject<{income: number[], expense: number[]}>();

  periodIncomeChanged$ = this.periodIncomeSource.asObservable();
  periodExpenseChanged$ = this.periodExpenseSource.asObservable();
  periodBalanceChanged$ = this.periodBalanceSource.asObservable();
  totalBalanceChanged$ = this.totalBalanceSource.asObservable();
  monthlyBalanceChanged$ = this.monthlyBalanceSource.asObservable();
  bankFeeChanged$ = this.bankFeeSource.asObservable();

  private viewPeriodSubscription: Subscription;
  private bankAccountsSubscription: Subscription;
  private creditAccountsSubscription: Subscription;
  private userProfileSubscription: Subscription;

  private bankAccounts$: Observable<BankAccount[]>;
  private creditAccounts$: Observable<CreditAccount[]>;
  private bankAccounts: BankAccount[];
  private creditAccounts: CreditAccount[];

  constructor(private store: Store<AppState>,
              private bankService: BankService,
              private creditService: CreditService,
              private userProfileService: UserProfileService,
              private accountControlService: AccountControlService) {
    this.periodBankFees = 0;
    this.cleanMonthlyBalance();
    this.period = accountControlService.getViewPeriod();

    this.bankAccounts$ = store.select(bankAccountReducer.getBankAccounts);
    this.creditAccounts$ = store.select(creditAccountReducer.getCreditAccounts);

    this.subscribeToPeriod();
    this.subscribeToUserProfileAccounts();
    this.subscribeToBankAccounts();
    this.subscribeToCreditAccounts();
  }

  OnDestroy() {
    this.viewPeriodSubscription.unsubscribe();
    this.bankAccountsSubscription.unsubscribe();
    this.creditAccountsSubscription.unsubscribe();
    this.userProfileSubscription.unsubscribe();
  }

  private subscribeToUserProfileAccounts() {
    this.userProfileSubscription = this.userProfileService.userProfile$.subscribe(up => {
      this.userProfile = up;
    });
  }

  private subscribeToBankAccounts() {
    this.bankAccountsSubscription = this.bankAccounts$.subscribe(res => {
      this.bankAccounts = res;
      this.countStatus();

      this.retrieveBankFees$();
      this.onStatusUpdated();
    });
  }

  private subscribeToCreditAccounts() {
    this.creditAccountsSubscription = this.creditAccounts$.subscribe(res => {
      this.creditAccounts = res;
      this.countStatus();
      this.onStatusUpdated();
    });
  }

  private subscribeToPeriod() {
    this.viewPeriodSubscription = this.accountControlService.viewPeriodChanged$.subscribe(
      period => {
        this.period = period;
        this.countStatus();

        this.retrieveBankFees$();
        this.onStatusUpdated();
      });
  }

  private cleanMonthlyBalance(){
    this.incomeMonthly = new Array<number[]>();
    this.expenseMonthly = new Array<number[]>();
    for (let i = 0; i < 11; i++) {
      this.incomeMonthly.push(new Array<number>())
      this.expenseMonthly.push(new Array<number>())
    }
  }

  private retrieveBankFees$() {
    if (!this.userProfile.Id) {
      this.periodBankFees = 0;
      this.bankFeeSource.next(this.periodBankFees);
    } else {
      this.bankService.getFees$(this.userProfile.Id, this.period).subscribe(fee => {
        this.periodBankFees = fee;
        this.bankFeeSource.next(this.periodBankFees);
      });
    }
  }

  private countStatus() {
    this.totalBalance = 0;
    this.periodIncome = 0;
    this.periodExpense = 0;
    this.totalBalance = 0;
    this.totalInBanks = 0;
    this.totalInCredit = 0;
    this.cleanMonthlyBalance();

    let now = new Date();
    if (this.bankAccounts) {
      this.bankAccounts.forEach(ba => {
        ba.Transactions.forEach(t => {
          let tDate = new Date(t.PaymentDate);
          if (this.isInCurrentPeriod(tDate)) {
            this.periodIncome += (t.Type === TransactionType.Income) ? t.Amount : 0;
            this.periodExpense += (t.Type === TransactionType.Expense) ? t.Amount : 0;
          }

          let monthIndex = 11 - (12 * (now.getFullYear() - tDate.getFullYear()) + now.getMonth() - tDate.getMonth());
          if (monthIndex >= 0 && monthIndex < 12) {
            if (isNaN(this.incomeMonthly[InstitutionType.Bank][monthIndex])) this.incomeMonthly[InstitutionType.Bank][monthIndex] = 0;
            if (isNaN(this.expenseMonthly[InstitutionType.Bank][monthIndex])) this.expenseMonthly[InstitutionType.Bank][monthIndex] = 0;
            this.incomeMonthly[InstitutionType.Bank][monthIndex] += (t.Type === TransactionType.Income) ? t.Amount : 0;
            this.expenseMonthly[InstitutionType.Bank][monthIndex] += (t.Type === TransactionType.Expense) ? t.Amount : 0;
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

          let monthIndex = 11 - (12 * (now.getFullYear() - tDate.getFullYear()) + now.getMonth() - tDate.getMonth());
          if (monthIndex >= 0 && monthIndex < 12) {
            if (isNaN(this.incomeMonthly[InstitutionType.Credit][monthIndex])) this.incomeMonthly[InstitutionType.Credit][monthIndex] = 0;
            if (isNaN(this.expenseMonthly[InstitutionType.Credit][monthIndex])) this.expenseMonthly[InstitutionType.Credit][monthIndex] = 0;
            this.incomeMonthly[InstitutionType.Credit][monthIndex] += (t.Type === TransactionType.Income) ? t.Amount : 0;
            this.expenseMonthly[InstitutionType.Credit][monthIndex] += (t.Type === TransactionType.Expense) ? t.Amount : 0;
          }
        });
      });
    }

    this.periodBalance = this.periodIncome - this.periodExpense;
  }

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
    this.monthlyBalanceSource.next({income:this.incomeMonthly[InstitutionType.Bank], expense:this.expenseMonthly[InstitutionType.Bank]});
  }

  getAllAccounts(){
    let accounts = new Array<AccountIdentifier>();
    this.bankAccounts.forEach(b => accounts.push({Id: b.Id, Type: AccountType.Bank}));
    this.creditAccounts.forEach(b => accounts.push({Id: b.Id, Type: AccountType.Credit}));
    return accounts;
  }
}
