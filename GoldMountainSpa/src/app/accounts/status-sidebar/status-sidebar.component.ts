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
  periodIncome:number;
  periodExpense:number;
  periodBalance:number;
  totalBalance:number;

  constructor(private accountSummaryService: AccountsSummaryService) {
    this.periodIncomeSubscription = accountSummaryService.periodIncomeChanged$.subscribe(
      periodIncome => {
        this.periodIncome = periodIncome;
      });

    this.periodExpenseSubscription = accountSummaryService.periodExpenseChanged$.subscribe(
      periodExpense => {
        this.periodExpense = periodExpense;
      });

    this.periodBalanceSubscription = accountSummaryService.periodBalanceChanged$.subscribe(
      periodBalance => {
        this.periodBalance = periodBalance;
      });

    this.totalBalanceSubscription = accountSummaryService.totalBalanceChanged$.subscribe(
      totalBalance => {
        this.totalBalance = totalBalance;
      });
  }
  statusData: any;

  ngOnInit() {
    this.statusData = {
      labels: ['Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep'],
      datasets: [
        {
          label: 'Income',
          backgroundColor: '#8fac67',
          borderColor: '#8fac67',
          data: [65, 59, 80, 81, 56, 55, 50]
        },
        {
          label: 'Expense',
          backgroundColor: '#d22a77',
          borderColor: '#d22a77',

          data: [28, 48, 40, 19, 86, 27, 40]
        }
      ]
    }
  }

  ngOnDestroy() {
    this.periodIncomeSubscription.unsubscribe();
    this.periodExpenseSubscription.unsubscribe();
    this.periodBalanceSubscription.unsubscribe();
    this.totalBalanceSubscription.unsubscribe();
  }

  getBalanceColor(balance : number)
  {
    return balance >= 0 ? "#8fac67" : "#d22a77";
  }

}
