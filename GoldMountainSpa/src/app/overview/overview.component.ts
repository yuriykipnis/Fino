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
import {Overview, LoanOverview} from "../models/overview";

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
            this.generateMortgageData(res.MortgageOverview);
            this.generateLoansData(res.LoanOverview);
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
        },
        datalabels: {
          display: false
        },
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
        },
        datalabels: {
          display: false
        },
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
          backgroundColor: '#20B2AA',
          borderColor: '#20B2AA',
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
          backgroundColor: '#FF7F50',
          borderColor: '#FF7F50',
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
      },
      plugins:{
        datalabels: {
          display: false
        },
      }
    };
  }

  private generateMortgageData(mortgage: LoanOverview) {
    this.mortgagesData = {
      labels: ['Principal','Interest'],
      datasets: [
        {
          data: [mortgage.Principal, mortgage.Interest],
          backgroundColor: [
            "#20B2AA",
            "#FF7F50",
            "#FF6347"
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
      },
      plugins:{
        datalabels: {
          display: false,
          color: '#eeeeee',
          anchor: 'center',
          clamp: false,
          formatter: Math.ceil,
        },
      }
    };
  }

  private generateLoansData(loans: LoanOverview) {
    this.loansData = {
      labels: ['Principal','Interest'],
      datasets: [
        {
          data: [loans.Principal, loans.Interest],
          backgroundColor: [
            "#20B2AA",
            "#FF7F50",
            "#FF6347"
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
      },
      plugins:{
        datalabels: {
          display: false,
          color: '#eeeeee',
          anchor: 'center',
          clamp: false,
          formatter: Math.ceil,
        },
      }
    };
  }

  private getIndexOfMonthsAgo(monthsPast:number): number {
    let nowMonth = new Date().getMonth();
    let rel = nowMonth-monthsPast;
    if (rel > 0) {
      return rel;
    }

    return 12 + rel;
  }

  private getBalanceColor(balance : number)  {
    if (balance === NaN){
      return "#20B2AA";
    }
    return balance >= 0 ? "#20B2AA" : "#FF6347";
  }

}
