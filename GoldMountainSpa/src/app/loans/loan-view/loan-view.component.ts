import { Component, OnInit, OnDestroy, Input, ViewEncapsulation  } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { LoanService } from "../services/loan.service";
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Store } from '@ngrx/store';
import 'rxjs/add/operator/switchMap';
import {UserProfileService} from "../../services/user-profile.service";
import {AppState} from "../../shared/store/app.states";
import * as fromLoanActions from "../store/actions/loan.action";
import {LoanControlService} from '../services/loan-control.service';
import {LoanViewModel} from "../models/loan-view.model";

@Component({
  selector: 'app-loan-view',
  templateUrl: './loan-view.component.html',
  styleUrls: ['./loan-view.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoanViewComponent implements OnInit, OnDestroy {
  loan: LoanViewModel;
  loanId: string;
  loanPrincipalPayments = new Array<number>();
  loanInterestPayments = new Array<number>();

  loanAmortisationData: any;
  loanAmortisationOptions: any;

  private userProfileSubscription: Subscription;
  private loanSelectedSubscription: Subscription;

  constructor(private store: Store<AppState>,
              private router: Router, public route: ActivatedRoute,
              private loanService: LoanService,
              private loanControlService: LoanControlService,
              private userProfileService: UserProfileService) {
    this.loan = loanControlService.getSelectedLoan();
    if (this.loan !== null) {
      this.updateLoanAmortisation();
    }
  }

  ngOnInit() {
    this.loanSelectedSubscription = this.loanControlService.selectedLoanChanged$.subscribe(loan => {
      //Mortgage new loan into this view
      this.loan = loan;
      this.updateLoanAmortisation();
    });

    this.userProfileSubscription = this.userProfileService.userProfile$.subscribe(up => {
      if (!up || !up.Id) {
        this.store.dispatch(new fromLoanActions.FetchLoans([]));
        return;
      }
    });
  }

  ngOnDestroy() {
    this.loanSelectedSubscription.unsubscribe();
    this.userProfileSubscription.unsubscribe();
  }

  updateLoanAmortisation(){
    this.calculateLoanPayments();

    this.loanAmortisationData = {
      labels: this.getLoanPeriods(),
      datasets: [
        {
          label: 'Interest',
          backgroundColor: '#FF6347',
          borderColor: '#FF6347',
          data: this.loanInterestPayments
        },
        {
          label: 'Principal',
          backgroundColor: '#20B2AA',
          borderColor: '#20B2AA',
          data: this.loanPrincipalPayments
        },
        //"#8fac67" : "#d22a77"
        //'#0066cc' : '#99ccff'

      ]
    };

    this.loanAmortisationOptions = {
      scales: {
        xAxes: [{
          stacked: true,
        }],
        yAxes: [{
          stacked: true,
        }]
      },
      plugins:{
        datalabels: {
          display: false,
          color: '#eeeeee',
          formatter: Math.ceil,
        },
      }
    };
  }

  private getLoanPeriods(): Array<number> {
    let result = new Array<number>();
    let now = new Date();
    let payoff = this.loan.PayoffDate;
    let payoffYear = + payoff.slice(0,4);

    for (let year = now.getFullYear(); year <= payoffYear; year++){
      result.push(year);
    }

    return result;
  }

  private getNumberOfPayments(): number {
    let payoffYear = + this.loan.PayoffDate.slice(0,4);
    let payoffMonth = + this.loan.PayoffDate.slice(5,7);
    let payoffDay = + this.loan.PayoffDate.slice(8,10);

    let now = new Date();
    let payoff = new Date(payoffYear, payoffMonth, payoffDay);

    let months = (payoff.getFullYear() - now.getFullYear()) * 12;
    months -= now.getMonth() + 1;
    months += payoff.getMonth();

    return months <= 0 ? 0 : months;
  }

  private calculateLoanPayments() {
    this.loanPrincipalPayments = new Array<number>();
    this.loanInterestPayments = new Array<number>();

    let periods = this.getLoanPeriods();
    let r_eff = Math.pow(1 + this.loan.InterestRate/(100*12),12) - 1;
    let r_eff_monthly = Math.pow(1+r_eff, 1/12) - 1;
    let N = this.getNumberOfPayments();
    let p_monthly = (this.loan.DeptAmount*r_eff_monthly) / (1 - Math.pow(1/(1+r_eff_monthly),N));

    let dept = this.loan.DeptAmount;
    periods.forEach(p => {
      let principal = 0;
      let interest = 0;
      let now = new Date();

      let startMonth = 0;
      if (now.getFullYear() == p){
        startMonth = now.getMonth();
      }

      for(let m = startMonth; m<12; m++){
        interest = interest + dept * r_eff_monthly;
        principal = principal + (p_monthly - dept * r_eff_monthly);
        dept = dept - (p_monthly - dept * r_eff_monthly);
      }

      if (interest < 0 ){ interest = 0; }

      this.loanPrincipalPayments.push(Number(principal.toFixed(2)));
      this.loanInterestPayments.push(Number(interest.toFixed(2)));
    });
  }
}
