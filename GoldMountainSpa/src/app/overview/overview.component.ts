import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import {Router} from '@angular/router';
import {NavigationEnd} from '@angular/router';
import {AfterViewInit} from '@angular/core';
import {AccountsSummaryService} from "../accounts/services/accounts-summary.service";
import {Subscription} from 'rxjs';
import {OnDestroy} from '@angular/core';
import {Months} from '../models/months';
import {UserProfileService} from "../services/user-profile.service";
import {OverviewService} from '../services/overview.service';
import {Overview} from "../models/overview";

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class OverviewComponent implements OnInit, OnDestroy {
  overviewData : Overview;
  private netWorthExpenseData : any;
  private netWorthIncomeData : any;
  private cashFlowData : any;
  private mortgagesData : any;
  private loansData : any;

  private netWorthIncomeOptions:any;
  private netWorthExpenseOptions:any;

  private mortgagesOptions:any;
  private loansOptions:any;
  private cashFlowDataOptions:any;

  private userProfileSubscription: Subscription;

  private isLoading: boolean;

  private ExpenseColors : string[] = [
    "#FFD700",
    "#FFA500",
    "#FA8072",
    "#FF7F50",
    "#FF6347",
    "#FFA07A",
    "#EE82EE",
    "#DDA0DD",
    "#FFFF00",
    "#DC143C",
  ];

  private IncomeColors : string[] = [
    "#00CED1",
    "#20B2AA",
    "#5F9EA0",
    "#66CDAA",
    "#87CEFA",
    "#6495ED",
    "#3CB371",
    "#008B8B",
    "#1E90FF",
    "#32CD32",
  ];

  constructor(private accountSummaryService: AccountsSummaryService,
              private userProfileService: UserProfileService,
              private overviewService: OverviewService) {
  }

  ngOnInit() {
    this.userProfileSubscription = this.userProfileService.userProfile$.subscribe(up => {
      if (!up || !up.Id ) { return; }

      this.isLoading = true;
      this.overviewService.getOverview$(up.Id)
        .subscribe(res => {
            this.overviewData = res;
            this.generateNetWorthExpenseData();
            this.generateNetWorthIncomeData();
            this.generateCashFlowData(res.CashFlowIncomes, res.CashFlowExpenses);
            this.generateMortgageData();
            this.generateLoansData();
            this.isLoading = false;
          },
          err => {
            this.isLoading = false;
          });
    });

    //
    // this.accountSummarySubscription = this.accountSummaryService.monthlyBalanceChanged$.subscribe(data => {
    //   this.generateCashFlowData(data);
    // });
  }

  ngOnDestroy() {
    this.userProfileSubscription.unsubscribe();
  }

  private generateNetWorthExpenseData() {
    let labels: string[] = [];
    let data: number[] = [];
    let totalExpense = 0;
    let expenseColors = [];
    let borderColor = [];
    let borderWidth = [];

    let iter = 0;
    this.overviewData.InstitutionOverviews.forEach(ins => {
      labels.push(ins.Label);
      data.push(this.overviewData.NetWorthExpenses[ins.Label]);
      totalExpense += this.overviewData.NetWorthExpenses[ins.Label];
      expenseColors.push(this.ExpenseColors[iter % 10]);
      borderColor.push("#cccccc");
      borderWidth.push(1);

      iter++;
    });

    this.netWorthExpenseData = {
      labels: labels,
      datasets: [
        {
          data: data,
          backgroundColor: expenseColors,
          borderColor: borderColor,
          borderWidth: borderWidth
        }]
    };

    this.netWorthExpenseOptions = {
      title: {
        display: false,
        text: 'My Title',
        fontSize: 16
      },
      legend: {
        display: true,
        position: 'right'
      },
      cutoutPercentage: 70,
      plugins:{
        doughnutlabel: {
          labels: [
            {
              text: 'Expense',
              font: {
                size: '20'
              }
            },
            {
              text: totalExpense.toFixed(2),
              font: {
                size: '16'
              },
              color: '#d22a77'
            }
          ]
        }
      }
    };

  }

  private generateNetWorthIncomeData() {
    let labels: string[] = [];
    let data: number[] = [];
    let totalIncome = 0;
    let incomeColors = [];
    let borderColor = [];
    let borderWidth = [];

    let iter = 0;
    this.overviewData.InstitutionOverviews.forEach(ins => {
      labels.push(ins.Label);
      data.push(this.overviewData.NetWorthIncomes[ins.Label]);
      totalIncome += this.overviewData.NetWorthIncomes[ins.Label];
      incomeColors.push(this.IncomeColors[iter % 10]);
      borderColor.push("#cccccc");
      borderWidth.push(1);
      iter++;
    });

    this.netWorthIncomeData = {
      labels: labels,
      datasets: [{
        data: data,
        backgroundColor: incomeColors,
        borderColor: borderColor,
        borderWidth: borderWidth,
      }]
    };

    this.netWorthIncomeOptions = {
      title: {
        display: false,
        text: 'My Title',
        fontSize: 16
      },
      legend: {
        display: true,
        position: 'right'
      },
      cutoutPercentage: 70,
      plugins:{
        doughnutlabel: {
          labels: [
            {
              text: 'Income',
              font: {
                size: '20'
              }
            },
            {
              text: totalIncome.toFixed(2),
              font: {
                size: '16'
              },
              color: '#8fac67'
            }
          ]
        }
      }
    };
  }

  private generateCashFlowData(income: number[], expense: number[]) {
    this.cashFlowData = {
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
          backgroundColor: '#8fac67',
          borderColor: '#8fac67',
          data: [
            income[Months[this.getIndexOfMonthsAgo(5)]],
            income[Months[this.getIndexOfMonthsAgo(4)]],
            income[Months[this.getIndexOfMonthsAgo(3)]],
            income[Months[this.getIndexOfMonthsAgo(2)]],
            income[Months[this.getIndexOfMonthsAgo(1)]],
            income[Months[this.getIndexOfMonthsAgo(0)]]
          ]
        },
        {
          label: 'Expense',
          backgroundColor: '#d22a77',
          borderColor: '#d22a77',
          data: [
            expense[Months[this.getIndexOfMonthsAgo(5)]],
            expense[Months[this.getIndexOfMonthsAgo(4)]],
            expense[Months[this.getIndexOfMonthsAgo(3)]],
            expense[Months[this.getIndexOfMonthsAgo(2)]],
            expense[Months[this.getIndexOfMonthsAgo(1)]],
            expense[Months[this.getIndexOfMonthsAgo(0)]]
          ]
        }
      ]
    };

    this.cashFlowDataOptions = {
      // legend: {
      //   display: false,
      // },
      scales: {
        xAxes: [{
        }],
        yAxes: [{
          ticks: { min: 0 }
        }]
      }
    };
  }

  private generateMortgageData() {
    this.mortgagesData = {
      labels: ['Principal','Interest','Indexing'],
      datasets: [
        {
          data: [300, 150, 70],
          backgroundColor: [
            "#00CED1",
            "#FA8072",
            "#FFD700"
          ],
          borderColor: [
            "#cccccc",
            "#cccccc",
            "#cccccc",
          ],
          borderWidth: [
            1,1,1
          ],
        }]
    };

    this.mortgagesOptions = {
      legend: {
        display: true,
        position: 'right'
      }
    };
  }

  private generateLoansData() {
    this.loansData = {
      labels: ['Principal','Interest','Indexing'],
      datasets: [
        {
          data: [300, 150, 70],
          backgroundColor: [
            "#00CED1",
            "#FA8072",
            "#FFD700"
          ],
          borderColor: [
            "#cccccc",
            "#cccccc",
            "#cccccc",
          ],
          borderWidth: [
            1,1,1
          ],
        }]
    };

    this.loansOptions = {
      legend: {
        display: true,
        position: 'right'
      }
    };
  }

  private getIndexOfMonthsAgo(monthsPast:number): number {
    let nowMonth = new Date().getMonth();
    let rel = nowMonth-monthsPast + 1;
    if (rel > 0) {
      return rel;
    }

    return 12 + rel;
  }

  private getBalanceColor(balance : number)  {
    if (balance === NaN){
      return "#8fac67";
    }
    return balance >= 0 ? "#8fac67" : "#d22a77";
  }

}
