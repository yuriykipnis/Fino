import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import {Transaction, TransactionType} from "../../../models/transaction";
import { Observable } from 'rxjs/Observable';
import {CreditAccount} from "../../models/credit-account";
import {BankAccount} from "../../models/bank-account";
import * as bankAccountReducer from '../../store/reducers/bank-account.reducer';
import * as creditAccountReducer from '../../store/reducers/credit-account.reducer';
import {AppState} from "../../../shared/store/app.states";
import { Store } from '@ngrx/store';
import {TreeNode, } from 'primeng/api';

@Component({
  selector: 'app-expense-view',
  templateUrl: './expense-view.component.html',
  styleUrls: ['./expense-view.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ExpenseViewComponent implements OnInit {
  bankAccounts$: Observable<BankAccount[]>;
  creditAccounts$: Observable<CreditAccount[]>;
  period: Date;
  expense: Transaction[];
  periodExpense: Transaction[];

  constructor(private store: Store<AppState>) {
    this.period = new Date();
    this.expense = new Array<Transaction>();
    this.periodExpense = new Array<Transaction>();
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
          .filter(t => t.Type === TransactionType.Expense)
          .forEach(t => {
            if (!this.expense.find(tf => tf.Id == t.Id)) {
              this.expense.push(t);
            }})
      });
      this.periodExpense = this.retrivePeriodExpenses();
    })
  }

  private handleCreditAccounts() {
    this.creditAccounts$.subscribe(res => {
      res.forEach(ac => {
        ac.Transactions
          .filter(t => t.Type === TransactionType.Expense)
          .forEach(t => {
            if (!this.expense.find(tf => tf.Id == t.Id)) {
              this.expense.push(t);
            }})
      });
      this.periodExpense = this.retrivePeriodExpenses();
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

    this.periodExpense = this.retrivePeriodExpenses();
  }

  nextMonth(){
    if (this.period.getMonth() == 11) {
      this.period.setFullYear(this.period.getFullYear() + 1);
      this.period.setMonth(0);
    } else {
      this.period.setMonth(this.period.getMonth() + 1);
    }

    this.periodExpense = this.retrivePeriodExpenses();
  }

  retrivePeriodExpenses (){
    return this.expense.filter(t => {
      let td = new Date(t.PaymentDate);
      return td.getFullYear() === this.period.getFullYear() && td.getMonth() == this.period.getMonth();
    });

  }


  // private retrivePeriodExpenses() : TreeNode[]{
  //
  //   return this.expense
  //     .filter(t => {
  //       let td = new Date(t.PaymentDate);
  //       return td.getFullYear() === this.period.getFullYear() && td.getMonth() == this.period.getMonth();
  //     })
  //     .reduce((result, trs) => {
  //       let parent = result.find(el => el.data.description === trs.Description);
  //       if (parent){
  //         parent.data.charge += trs.Amount;
  //         parent.children.push({data:
  //           {description: trs.Description, charge: trs.Amount, date: trs.PaymentDate}
  //         });
  //       }
  //       else {
  //         let newParent = {
  //           data:{description: trs.Description, charge: trs.Amount},
  //           children: []
  //         };
  //         newParent.children.push({data:
  //           {description: trs.Description, charge: trs.Amount, date: trs.PaymentDate}
  //         });
  //         result.push(newParent);
  //       }
  //
  //       return result;
  //     }, new Array<TreeNode>())
  //     .reduce((result, node) => {
  //       if (node.children.length == 1) {
  //         result.push(node.children[0]);
  //       }
  //       else {
  //         result.push(node);
  //       }
  //
  //       return result;
  //     }, new Array<TreeNode>());
  // }
}
