import { Component, OnInit } from '@angular/core';
import {Transaction, TransactionType} from "../../../models/transaction";
import { Observable } from 'rxjs/Observable';
import {CreditAccount} from "../../models/credit-account";
import {BankAccount} from "../../models/bank-account";
import * as bankAccountReducer from '../../store/reducers/bank-account.reducer';
import * as creditAccountReducer from '../../store/reducers/credit-account.reducer';
import {AppState} from "../../../shared/store/app.states";
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-income-view',
  templateUrl: './income-view.component.html',
  styleUrls: ['./income-view.component.scss']
})
export class IncomeViewComponent implements OnInit {
  bankAccounts$: Observable<BankAccount[]>;
  creditAccounts$: Observable<CreditAccount[]>;
  period: Date;
  income: Transaction[];
  periodIncome: Transaction[];

  constructor(private store: Store<AppState>) {
    this.period = new Date();
    this.income = new Array<Transaction>();
    this.periodIncome = new Array<Transaction>();
    this.bankAccounts$ = store.select(bankAccountReducer.getBankAccounts);
    this.creditAccounts$ = store.select(creditAccountReducer.getCreditAccounts);
  }

  ngOnInit() {
    this.handleBankAccounts();
    this.handleCreditAccounts();
  }

  private handleBankAccounts() {
    this.bankAccounts$.subscribe(res => {
      res.forEach(ac => {
        ac.Transactions
          .filter(t => t.Type === TransactionType.Income)
          .forEach(t => {
            if (!this.income.find(tf => tf.Id == t.Id)) {
              this.income.push(t);
            }})
      });
      this.periodIncome = this.retrivePeriodExpenses();
    })
  }

  private handleCreditAccounts() {
    this.creditAccounts$.subscribe(res => {
      res.forEach(ac => {
        ac.Transactions
          .filter(t => t.Type === TransactionType.Income)
          .forEach(t => {
            if (!this.income.find(tf => tf.Id == t.Id)) {
              this.income.push(t);
            }})
      });
      this.periodIncome = this.retrivePeriodExpenses();
    })
  }

  getTransactionColor(type : TransactionType) {
    return type === TransactionType.Income ? "green" : "red";
  }

  getBalanceColor(balance : number) {
    return balance > 0 ? "green" : "red";
  }

  prevMonth(){
    if (this.period.getMonth() == 0) {
      this.period.setFullYear(this.period.getFullYear() - 1);
      this.period.setMonth(11);
    } else {
      this.period.setMonth(this.period.getMonth() - 1);
    }

    this.periodIncome = this.retrivePeriodExpenses();
  }

  nextMonth(){
    if (this.period.getMonth() == 11) {
      this.period.setFullYear(this.period.getFullYear() + 1);
      this.period.setMonth(0);
    } else {
      this.period.setMonth(this.period.getMonth() + 1);
    }

    this.periodIncome = this.retrivePeriodExpenses();
  }

  private retrivePeriodExpenses() {
    return this.income.filter(t => {
      let td = new Date(t.PaymentDate);
      return td.getFullYear() === this.period.getFullYear() && td.getMonth() == this.period.getMonth();
    });
  }
}
