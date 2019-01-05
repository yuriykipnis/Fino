import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import {AccountsSummaryService} from "../services/accounts-summary.service";
import {Subscription} from 'rxjs';
import {Months} from "../../models/months";

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
  monthlyBalanceOptions:any;

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
      labels: [
        Months[this.getIndexOfMonthsAgo(5)],
        Months[this.getIndexOfMonthsAgo(4)],
        Months[this.getIndexOfMonthsAgo(3)],
        Months[this.getIndexOfMonthsAgo(2)],
        Months[this.getIndexOfMonthsAgo(1)],
        Months[this.getIndexOfMonthsAgo(0)]],
      datasets: [
        {
          label: 'Income',
          backgroundColor: '#20B2AA',
          borderColor: '#20B2AA',
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
          backgroundColor: '#FF6347',
          borderColor: '#FF6347',
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
    this.monthlyBalanceOptions = {
      plugins:{
        datalabels: {
          display: false,
        }
      },
      tooltips: {
        callbacks: {
          label: function(tooltipItem, data) {
            var label = data.datasets[tooltipItem.datasetIndex].label || '';
            if (label) {
              label += ': ';
            }
            label += tooltipItem.yLabel.toFixed(2);
            return label;
          }
        }
      }
    };
  }

  private getIndexOfMonthsAgo(monthsPast:number): number {
    let nowMonth = new Date().getMonth();
    let rel = nowMonth-monthsPast;
    if (rel >= 0) {
      return rel;
    }

    return 12 + rel;
  }

  getBalanceColor(balance : number)
  {
    return balance >= 0 ? "#8fac67" : "#d22a77";
  }

}
