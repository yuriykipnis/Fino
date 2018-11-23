import { Component, OnInit, OnDestroy, Input, ViewEncapsulation  } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { LoanService } from "../services/loan.service";
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Store } from '@ngrx/store';
import 'rxjs/add/operator/switchMap';
import {Mortgage} from '../../models/mortgage';
import {UserProfileService} from "../../services/user-profile.service";
import {AppState} from "../../shared/store/app.states";
import * as fromLoanActions from "../store/actions/loan.action";
import * as loanReducer from "../store/reducers/loan.reducer";
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
  loanAmortisation: any;

  private userProfileSubscription: Subscription;
  private loanSelectedSubscription: Subscription;

  constructor(private store: Store<AppState>,
              private router: Router, public route: ActivatedRoute,
              private loanService: LoanService,
              private loanControlService: LoanControlService,
              private userProfileService: UserProfileService) {

    this.loan = loanControlService.getSelectedLoan();

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
    this.loanAmortisation = {
      labels: [2018, 2019, 2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027],
      datasets: [
        {
          label: 'Interest',
          backgroundColor: '#0066cc',
          borderColor: '#0066cc',
          data: [
            70, 63, 56, 49, 42, 35, 28, 21, 14, 7
          ]
        },
        //"#8fac67" : "#d22a77"
        //'#0066cc' : '#99ccff'
        {
          label: 'Principal',
          backgroundColor: '#99ccff',
          borderColor: '#99ccff',
          data: [
            30, 37, 44, 51, 58, 65, 72, 79, 86, 93
          ]
        }
      ]
    }
  }
}
