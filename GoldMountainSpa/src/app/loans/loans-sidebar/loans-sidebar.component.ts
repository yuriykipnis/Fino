import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {AppState} from "../../shared/store/app.states";
import { Store } from '@ngrx/store';
import {Observable} from 'rxjs/Observable';
import {Subscription} from 'rxjs/Subscription';
import {AccountsSummaryService} from "../../accounts/services/accounts-summary.service";
import * as loanReducer from "../store/reducers/loan.reducer";
import {Loan} from "app/models/loan";

@Component({
  selector: 'app-loans-sidebar',
  templateUrl: './loans-sidebar.component.html',
  styleUrls: ['./loans-sidebar.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoansSidebarComponent implements OnInit {
  loans$: Observable<Loan[]>;

  constructor(private store: Store<AppState>,
              private router: Router, private route: ActivatedRoute,
              public accountSummaryService: AccountsSummaryService) {
    this.loans$ = store.select(loanReducer.getLoans);
  }

  ngOnInit() {

  }

  ngOnDestroy() {

  }

  openLoanView(loan: any) {
    this.router.navigate([loan.Id], {relativeTo: this.route});
  }

}
