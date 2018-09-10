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
import {Loan} from '../models/loan';
import {BankAccount} from "../accounts/models/bank-account";
import {SubLoan} from "../models/subLoan";

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
            res.forEach(r => {
              let loans = new Array<LoanViewModel>();
              r.Loans.forEach(loan => {
                loans.push(this.generateLoanViewModel(r, loan));
              })

              this.store.dispatch(new fromLoanActions.FetchLoans(loans));
            });

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

  private generateLoanViewModel(account: BankAccount, loan: Loan) : LoanViewModel{
    let loanView = new LoanViewModel({
      Id: loan.Id,
      BankLabel: account.Label,
      BankName: account.ProviderName,
      BankNumber: account.BankNumber,
      BankBranchNumber: account.BranchNumber,
      BankAccountNumber: account.AccountNumber,

      StartDate: loan.StartDate,
      PayoffDate: loan.EndDate,
      NextPaymentDate: loan.NextPaymentDate,
      OriginalAmount: loan.OriginalAmount,
      DeptAmount: loan.DeptAmount,
      LastPaymentAmount:loan.LastPaymentAmount,
      PrepaymentCommission: loan.PrepaymentCommission,
      InterestType: loan.InterestType,
      LinkageType: loan.LinkageType,
      InsuranceCompany: loan.InsuranceCompany,

      SubLoans: loan.SubLoans
    });

    return loanView;
  }
  isLoading() {
    return this.isLoansLoading;
  }
}
