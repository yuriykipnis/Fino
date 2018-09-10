import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import {AccountsSummaryService} from "../services/accounts-summary.service";
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-status-sidebar',
  templateUrl: './status-sidebar.component.html',
  styleUrls: ['./status-sidebar.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class StatusSidebarComponent implements OnInit, OnDestroy {

  private periodIncomeSubscription: Subscription;
  private periodExpenseSubscription: Subscription;
  private periodBalanceSubscription: Subscription;
  private totalBalanceSubscription: Subscription;
  private monthlyBalanceSubscription: Subscription;
  private bankFeeSubscription: Subscription;

  periodIncome:number;
  periodExpense:number;
  periodBalance:number;
  totalBalance:number;
  bankFee:number;
  monthlyBalance: any;

  constructor(private accountSummaryService: AccountsSummaryService) {

  }

  ngOnInit() {
    this.periodIncomeSubscription = this.accountSummaryService.periodIncomeChanged$.subscribe(
      periodIncome => {
        this.periodIncome = periodIncome;
      });

    this.periodExpenseSubscription = this.accountSummaryService.periodExpenseChanged$.subscribe(
      periodExpense => {
        this.periodExpense = periodExpense;
      });

    this.periodBalanceSubscription = this.accountSummaryService.periodBalanceChanged$.subscribe(
      periodBalance => {
        this.periodBalance = periodBalance;
      });

    this.totalBalanceSubscription = this.accountSummaryService.totalBalanceChanged$.subscribe(
      totalBalance => {
        this.totalBalance = totalBalance;
      });

    this.monthlyBalanceSubscription = this.accountSummaryService.monthlyBalanceChanged$.subscribe(
      monthlyBalance => {
        this.updateMonthlyBalance(monthlyBalance.income, monthlyBalance.expense);
      });

    this.bankFeeSubscription = this.accountSummaryService.bankFeeChanged$.subscribe(
      bankFee => {
        this.bankFee = bankFee;
      });
  }

  ngOnDestroy() {
    this.periodIncomeSubscription.unsubscribe();
    this.periodExpenseSubscription.unsubscribe();
    this.periodBalanceSubscription.unsubscribe();
    this.totalBalanceSubscription.unsubscribe();
    this.monthlyBalanceSubscription.unsubscribe();
    this.bankFeeSubscription.unsubscribe();
  }

  updateMonthlyBalance(income: number[], expense: number[]){
    this.monthlyBalance = {
      labels: ['Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep'],
      datasets: [
        {
          label: 'Income',
          backgroundColor: '#8fac67',
          borderColor: '#8fac67',
          data: [
            income[6],
            income[7],
            income[8],
            income[9],
            income[10],
            income[11],
          ]
        },
        {
          label: 'Expense',
          backgroundColor: '#d22a77',
          borderColor: '#d22a77',
          data: [
            expense[6],
            expense[7],
            expense[8],
            expense[9],
            expense[10],
            expense[11],
          ]
        }
      ]
    }
  }

  getBalanceColor(balance : number)
  {
    return balance >= 0 ? "#8fac67" : "#d22a77";
  }

}
