import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { AppState } from "../shared/store/app.states";
import {BankService} from "../services/bank.service";
import {CreditService} from "../services/credit.service";
import {UserProfileService} from "../services/user-profile.service";
import * as fromBankActions from './store/actions/bank-account.action';
import * as fromCreditActions from './store/actions/credit-account.action';
import {AccountControlService} from "./services/account-control.service";

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AccountsComponent implements OnInit, OnDestroy {
  private userProfileSubscription: Subscription;
  isBankAccountsLoading: boolean = false;
  isCreditAccountsLoading: boolean = false;

  constructor(private store: Store<AppState>,
              private bankService: BankService,
              private creditService: CreditService,
              private userProfileService: UserProfileService,
              private accountControlService: AccountControlService) {
  }

  sleep= function pause(numberMillis) {
    var now = new Date();
    var exitTime = now.getTime() + numberMillis;
    while (true) {
      now = new Date();
      if (now.getTime() > exitTime)
        return;
    }
  };

  ngOnInit() {
    this.isBankAccountsLoading = true;
    this.isCreditAccountsLoading = true;
    this.accountControlService.changeLoadingState(true);

    this.userProfileSubscription = this.userProfileService.userProfile$.subscribe(up => {
      if (!up || !up.Id) {
        this.store.dispatch(new fromBankActions.FetchBankAccounts([]));
        this.store.dispatch(new fromCreditActions.FetchCreditAccounts([]));
        return;
      }

      //this.accountControlService.changeLoadingState(true);
      this.bankService.getAccounts$(up.Id)
        .subscribe(res => {
            this.store.dispatch(new fromBankActions.FetchBankAccounts(res));
            this.accountControlService.changeLoadingState(false);
          },
          err => {
            this.isBankAccountsLoading = false;
            this.accountControlService.changeLoadingState(false);
          });

      this.creditService.getAccounts$(up.Id)
        .subscribe(res => {
            this.store.dispatch(new fromCreditActions.FetchCreditAccounts(res));
            this.isCreditAccountsLoading = false;
            //this.accountControlService.changeLoadingState(false);
          },
          err => {
            this.isCreditAccountsLoading = false;
            //this.accountControlService.changeLoadingState(false);
          });
    });
  }

  ngOnDestroy() {
    this.userProfileSubscription.unsubscribe();
  }

  isLoading() {
    return this.isBankAccountsLoading;
    //return this.isBankAccountsLoading || this.isCreditAccountsLoading;
  }

  getBalanceColor(balance : number)
  {
    return balance >= 0 ? "#8fac67" : "#d22a77";
  }

}

