import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {AppState} from "../../shared/store/app.states";
import { Store } from '@ngrx/store';
import {Observable} from 'rxjs/Observable';
import {Subscription} from 'rxjs/Subscription';
import {AccountsSummaryService} from "../../accounts/services/accounts-summary.service";
import * as loanReducer from "../store/reducers/loan.reducer";
import {Loan} from "app/models/loan";
import {LoanControlService} from "../services/loan-control.service";
import {AccountType} from "../../accounts/models/account-identifier";
import {LoanViewModel} from '../models/loan-view.model';
import {SubLoan} from "../../models/subLoan";

@Component({
  selector: 'app-loans-sidebar',
  templateUrl: './loans-sidebar.component.html',
  styleUrls: ['./loans-sidebar.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoansSidebarComponent implements OnInit {
  loans$: Observable<LoanViewModel[]>;
  loansSubscription: Subscription;

  constructor(private store: Store<AppState>,
              private router: Router, private route: ActivatedRoute,
              public loanControlService: LoanControlService) {
    this.loans$ = store.select(loanReducer.getLoans);
  }

  ngOnInit() {
    this.loansSubscription = this.loans$.subscribe(res =>{
      if (res.length > 0 && !this.loanControlService.getSelectedLoan()) {
        this.loanControlService.changeSelectedLoan(res[0]);
      }
    });
  }

  ngOnDestroy() {
    this.loansSubscription.unsubscribe();
  }

  openLoanView(loan: any) {
    this.loanControlService.changeSelectedLoan(loan);
  }


}
