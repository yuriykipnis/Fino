import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import {Observable} from 'rxjs/Observable';
import {Subscription} from 'rxjs/Subscription';
import {LoanViewModel} from "../models/loan-view.model";
import * as loanReducer from "../store/reducers/loan.reducer";
import {AppState} from "../../shared/store/app.states";
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-loans-status-bar',
  templateUrl: './loans-status-bar.component.html',
  styleUrls: ['./loans-status-bar.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoansStatusBarComponent implements OnInit {
  loansSubscription: Subscription;
  loans$: Observable<LoanViewModel[]>;
  loans: Array<LoanViewModel>;
  loanAmortisationData: any;
  loanAmortisationOptions: any;
  originalPrincipal: number;
  currentPrincipal: number;
  currentInterest: number;

  constructor(private store: Store<AppState>) {
    this.loans$ = store.select(loanReducer.getLoans);
  }

  ngOnInit() {
    this.loansSubscription = this.loans$.subscribe(res =>{
      this.originalPrincipal = 0;
      this.currentPrincipal = 0;
      this.currentInterest = 0;
      this.loans = [];
      res.forEach(l => {
        this.loans.push(l);

        this.originalPrincipal += l.OriginalAmount;
        this.currentPrincipal += l.DeptAmount;
        this.currentInterest += l.InterestAmount;
      });

      this.updateLoanAmortisation();
    });
  }

  ngOnDestroy() {
    this.loansSubscription.unsubscribe();
  }

  private updateLoanAmortisation(){
    this.loanAmortisationData = {
      labels: this.calculatePeriods(),
      datasets: [
        {
          label: 'Recommended',
          backgroundColor: '#20B2AA',
          borderColor: '#20B2AA',
          data: [
            0
          ]
        },
        {
          label: 'Current',
          backgroundColor: '#FF6347',
          borderColor: '#FF6347',
          data:  this.getDeptPerPeriod()
        }
      ]
    };

    this.loanAmortisationOptions = {
      plugins:{
        datalabels: {
          display: false,
          color: '#eeeeee',
          formatter: Math.ceil,
        },
      }
    };
  }

  private calculateNumberOfPayments(loan: LoanViewModel): number {
    let payoffYear = + loan.PayoffDate.slice(0,4);
    let payoffMonth = + loan.PayoffDate.slice(5,7);
    let payoffDay = + loan.PayoffDate.slice(8,10);

    let now = new Date();
    let payoff = new Date(payoffYear, payoffMonth, payoffDay);

    let months = (payoff.getFullYear() - now.getFullYear()) * 12;
    months -= now.getMonth() + 1;
    months += payoff.getMonth();

    return months <= 0 ? 0 : months;
  }

  private calculatePeriods(): Array<number> {
    let result = new Array<number>();
    let now = new Date();
    let payoffYear = now.getFullYear();

    this.loans.forEach(l => {
      let loanPayoffYear = Number(l.PayoffDate.slice(0,4));
      if (loanPayoffYear > payoffYear){
        payoffYear = loanPayoffYear;
      }
    });

    for (let year = now.getFullYear(); year <= payoffYear; year++){
      result.push(year);
    }

    return result;
  }

  private getDeptPerPeriod(): Array<number> {
    let result = new Array<number>();

    this.loans.forEach(l => {
      let N = this.calculateNumberOfPayments(l);
      let r_eff = Math.pow(1 + l.InterestRate/(100*12),12) - 1;
      let r_eff_monthly = Math.pow(1+r_eff, 1/12) - 1;
      let p_monthly = (l.DeptAmount*r_eff_monthly) / (1 - Math.pow(1/(1+r_eff_monthly),N));

      let periods = this.calculatePeriods();
      let dept = l.DeptAmount;

      periods.forEach(p => {
        let principal = 0;
        let interest = 0;
        let now = new Date();

        let startMonth = 0;
        if (now.getFullYear() == p){
          startMonth = now.getMonth();
        }

        for(let m = startMonth; m<12 && dept>0; m++){
          interest = interest + dept * r_eff_monthly;
          principal = principal + (p_monthly - dept * r_eff_monthly);
          dept = dept - (p_monthly - dept * r_eff_monthly);
        }

        let index = p - now.getFullYear();
        if (result.length <= index){
          result.push(Number(dept > 0 ? dept.toFixed(2) : 0));
        } else {
          result[index] += Number(dept > 0 ? dept.toFixed(2) : 0);
        }
      });
    });

    return result;
  }
}
