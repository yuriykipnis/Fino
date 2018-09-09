import { Component, OnInit, OnDestroy, Input, ViewEncapsulation  } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { LoanService } from "../services/loan.service";
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Store } from '@ngrx/store';
import 'rxjs/add/operator/switchMap';
import {Loan} from '../../models/loan';
import {UserProfileService} from "../../services/user-profile.service";
import {AppState} from "../../shared/store/app.states";
import * as fromLoanActions from "../store/actions/loan.action";
import * as loanReducer from "../store/reducers/loan.reducer";

@Component({
  selector: 'app-loan-view',
  templateUrl: './loan-view.component.html',
  styleUrls: ['./loan-view.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoanViewComponent implements OnInit, OnDestroy {
  loans$: Observable<Loan[]>;
  loan$: Observable<Loan>;
  loanId: string;
  private userProfileSubscription: Subscription;

  constructor(private store: Store<AppState>,
              private router: Router, public route: ActivatedRoute,
              private loanService: LoanService,
              private userProfileService: UserProfileService) {
    this.loans$ = store.select(loanReducer.getLoans);
  }

  ngOnInit() {
    this.loan$ = this.retriveLoan();

    this.userProfileSubscription = this.userProfileService.userProfile$.subscribe(up => {
      if (!up || !up.Id) {
        this.store.dispatch(new fromLoanActions.FetchLoans([]));
        return;
      }
    });
  }

  ngOnDestroy() {
    this.userProfileSubscription.unsubscribe();
  }

  private retriveLoan() : Observable<Loan>{
    return this.route.paramMap
      .switchMap((params: ParamMap) => {
        this.loanId = params.get('LoanId');
        return this.loans$.map(loans => loans.find( loan => loan.Id == this.loanId));
      });
  }

}
