import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {AppState} from "../../shared/store/app.states";
import { Store } from '@ngrx/store';
import {Observable} from 'rxjs/Observable';
import {Subscription} from 'rxjs/Subscription';
import * as loanReducer from "../store/reducers/loan.reducer";
import {LoanControlService} from "../services/loan-control.service";
import {LoanViewModel} from '../models/loan-view.model';
import {AccountControlService} from "../../accounts/services/account-control.service";

@Component({
  selector: 'app-loans-sidebar',
  templateUrl: './loans-sidebar.component.html',
  styleUrls: ['./loans-sidebar.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoansSidebarComponent implements OnInit {
  loans$: Observable<LoanViewModel[]>;
  mortgages: Array<{key: string, loans: LoanViewModel[]}>;
  loansSubscription: Subscription;
  loadingStateSubscription: Subscription;
  isLoading: boolean;

  constructor(private store: Store<AppState>,
              private router: Router, private route: ActivatedRoute,
              private loanControlService: LoanControlService,
              private accountControlService: AccountControlService) {
    this.loans$ = store.select(loanReducer.getLoans);

  }

  ngOnInit() {
    this.isLoading = this.accountControlService.getIsLoading();
    this.loadingStateSubscription = this.accountControlService.isLoadingChanged$.subscribe(newState => {
      this.isLoading = newState;
    });

    this.loansSubscription = this.loans$.subscribe(res =>{
      this.mortgages = new Array<{key: string, loans: LoanViewModel[]}>();
      res.forEach(l => {
        let key : string = l.CityName + "-" + l.StreetName + "-" + l.BuildingNumber;
        let loans = this.mortgages.find(el => { return el.key === key });
        if(!loans) {
          this.mortgages.push({key: key, loans: new Array<LoanViewModel>()});
          loans = this.mortgages.find(el => { return el.key === key });
        }
        loans.loans.push(l);
      });

      if (res.length > 0 && this.loanControlService.getSelectedLoan() === null) {
        this.loanControlService.changeSelectedLoan(res[0]);
      }
    });
  }

  ngOnDestroy() {
    this.loansSubscription.unsubscribe();
    this.loadingStateSubscription.unsubscribe();
  }

  private openLoanView(loan: any) {
    this.loanControlService.changeSelectedLoan(loan);
  }

  private isSelected(loan: LoanViewModel) {
    return loan.Id == this.loanControlService.getSelectedLoan().Id;
  }

}
