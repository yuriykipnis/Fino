import { Component, OnInit, OnDestroy } from '@angular/core';
import {ChartModule} from 'primeng/chart';
import {AccountsSummaryService} from "../services/accounts-summary.service";
import {Observable} from 'rxjs/Observable';
import {Subscription} from 'rxjs/Subscription';

@Component({
  selector: 'app-accounts-summary',
  templateUrl: './accounts-summary.component.html',
  styleUrls: ['./accounts-summary.component.scss']
})
export class AccountsSummaryComponent implements OnInit, OnDestroy {
  incomeOutcomeData: any;
  incomeData:any;
  expenseData: any;
  accountSummaryUpdateSubscription: Subscription;

  constructor(public accountSummaryService: AccountsSummaryService) {
    this.accountSummaryUpdateSubscription = this.accountSummaryService.summary$
      .subscribe(s => {
        this.updateIncomeOutcomeData();
        this.updateIncomeData();
        this.updateExpenseData();
      });
  }

  ngOnInit() {
    this.updateIncomeOutcomeData();
  }

  ngOnDestroy() {
    this.accountSummaryUpdateSubscription.unsubscribe();
  }


  updateIncomeOutcomeData(){
    this.incomeOutcomeData = {
      labels: ['June', 'July', 'August' ,'September' , 'October' ,'November', 'December','January', 'February', 'March', 'April', 'May'],
      datasets: [
        {
          label: 'Income',
          backgroundColor: '#9CCC65',
          borderColor: '#7CB342',
          data: this.accountSummaryService.incomeMonthly[0]
        },
        {
          label: 'Expense',
          backgroundColor: '#cc3830',
          borderColor: '#b3232f',
          data: this.accountSummaryService.outcomeMonthly[0]
        }
      ]
    };
  }

  updateIncomeData() {
    this.incomeData = {
      labels: ['Credit', 'Bank', 'Cash', 'Checks'],
      datasets: [
        {
          label: 'Income Source',
          data: [this.accountSummaryService.totalInCredit, this.accountSummaryService.totalInBanks, 0, 0],
          backgroundColor: [
            "#FF6384",
            "#36A2EB",
            "#FFCE56",
            "#35ffcc",
          ],
          hoverBackgroundColor: [
            "#ff475d",
            "#1a80eb",
            "#ffe839",
            "#1cffda",
          ]
        }
      ]
    };
  }

  updateExpenseData() {
    this.expenseData = {
      labels: ['Credit', 'Bank', 'Cash', 'Checks'],
      datasets: [
        {
          label: 'Expense Source',
          data: [this.accountSummaryService.totalInCredit, this.accountSummaryService.totalInBanks, 0, 0],
          backgroundColor: [
            "#FFCE56",
            "#FF6384",
            "#35ffcc",
            "#36A2EB",
          ],
          hoverBackgroundColor: [
            "#ffe839",
            "#ff475d",
            "#1cffda",
            "#1a80eb",
          ]
        }
      ]
    };
  }
}
