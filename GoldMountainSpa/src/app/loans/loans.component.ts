import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { AppState } from "../shared/store/app.states";
import {BankService} from "../services/bank.service";
import {CreditService} from "../services/credit.service";
import {UserProfileService} from "../services/user-profile.service";
import * as fromLoanActions from "./store/actions/loan.action";
import {LoanViewModel} from "./models/loan-view.model";
import {Mortgage} from '../models/mortgage';
import {BankAccount} from "../accounts/models/bank-account";

@Component({
  selector: 'app-loans',
  templateUrl: './loans.component.html',
  styleUrls: ['./loans.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoansComponent implements OnInit, OnDestroy {

  private userProfileSubscription: Subscription;
  isLoansLoading: boolean = false;

  constructor(private store: Store<AppState>,
              private bankService: BankService,
              private creditService: CreditService,
              private userProfileService: UserProfileService) {
  }

  ngOnInit() {
    this.isLoansLoading = true;

    this.userProfileSubscription = this.userProfileService.userProfile$.subscribe(up => {
      if (!up || !up.Id) {
        return;
      }

      this.bankService.getAccounts$(up.Id)
        .subscribe(res => {
            let loans = new Array<LoanViewModel>();
            res.forEach(r => {
              r.Mortgages.forEach(loan => {
                loans.push(this.generateLoanViewModel(r, loan));
              })
            });
            this.store.dispatch(new fromLoanActions.FetchLoans(loans));
            this.isLoansLoading = false;
          },
          err => {
            this.isLoansLoading = false;
          });
    });
  }

  ngOnDestroy() {
    this.userProfileSubscription.unsubscribe();
  }

  private generateLoanViewModel(account: BankAccount, loan: Mortgage) : LoanViewModel{
    let loanView = new LoanViewModel({
      Id: loan.Id,
      BankLabel: account.Label,
      BankName: account.ProviderName,
      BankNumber: account.BankNumber,
      BankBranchNumber: account.BranchNumber,
      BankAccountNumber: account.AccountNumber,

      CityName: loan.AssetCity,
      StreetName: loan.AssetStreet,
      BuildingNumber: loan.AssetBuildingNumber,

      StartDate: loan.StartDate,
      PayoffDate: loan.EndDate,
      NextPaymentDate: loan.NextPaymentDate,
      OriginalAmount: loan.OriginalAmount,
      DeptAmount: loan.DeptAmount,
      InterestAmount: loan.InterestAmount,
      InterestRate: loan.InterestRate,
      PrepaymentCommission: loan.PrepaymentCommission,
      InterestType: loan.InterestType,
      IndexType: loan.IndexType,
      InsuranceCompany: loan.InsuranceCompany,
    });

    return loanView;
  }
  isLoading() {
    return this.isLoansLoading;
  }
}
