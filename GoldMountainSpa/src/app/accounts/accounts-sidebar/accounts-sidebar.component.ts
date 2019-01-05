import {Component, Input, OnInit, ViewEncapsulation} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {AppState} from "../../shared/store/app.states";
import {Store} from '@ngrx/store';
import {Observable} from 'rxjs/Observable';
import {Subscription} from 'rxjs/Subscription';
import * as bankAccountReducer from '../store/reducers/bank-account.reducer';
import * as creditAccountReducer from '../store/reducers/credit-account.reducer';
import {BankAccount} from "../models/bank-account";
import {CreditAccount} from "../models/credit-account";
import {Subject} from 'rxjs';
import {AccountControlService} from "../services/account-control.service";
import {AccountType} from "../models/account-identifier";
import {AccountsSummaryService} from "../services/accounts-summary.service";

@Component({
  selector: 'app-accounts-sidebar',
  templateUrl: './accounts-sidebar.component.html',
  styleUrls: ['./accounts-sidebar.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AccountsSidebarComponent implements OnInit {
  bankAccounts$: Observable<BankAccount[]>;
  creditAccounts$: Observable<CreditAccount[]>;
  bankAccountsSubscription: Subscription;
  creditAccountsSubscription: Subscription;
  loadingBankStateSubscription: Subscription;
  loadingCreditStateSubscription: Subscription;
  isCreditLoading: boolean;
  isBankLoading: boolean;

  constructor(private store: Store<AppState>,
              private accountControlService: AccountControlService,
              private accountsSummaryService: AccountsSummaryService,
              private router: Router,
              private route: ActivatedRoute) {
    this.bankAccounts$ = store.select(bankAccountReducer.getBankAccounts);
    this.creditAccounts$ = store.select(creditAccountReducer.getCreditAccounts);
  }

  ngOnInit() {
    this.isCreditLoading = this.accountControlService.getIsCreditLoading();
    this.isBankLoading = this.accountControlService.getIsBankLoading();

    this.loadingBankStateSubscription = this.accountControlService.isBankAccountLoadingChanged$.subscribe(newState => {
      this.isBankLoading = newState;
    });

    this.loadingCreditStateSubscription = this.accountControlService.isCreditAccountLoadingChanged$.subscribe(newState => {
      this.isCreditLoading = newState;
    });

    this.bankAccountsSubscription = this.bankAccounts$.subscribe(res => {
      if (res.length > 0 && !this.accountControlService.getSelectedAccount()) {
        this.accountControlService.changeSelectedAccountId({Id: res[0].Id, Type: AccountType.Bank});
      }
    });

    this.creditAccountsSubscription = this.bankAccounts$.subscribe(res => {
      if (res.length > 0 && !this.accountControlService.getSelectedAccount()) {
        this.accountControlService.changeSelectedAccountId({Id: res[0].Id, Type: AccountType.Credit});
      }
    });
  }

  ngOnDestroy() {
    this.bankAccountsSubscription.unsubscribe();
    this.creditAccountsSubscription.unsubscribe();
    this.loadingBankStateSubscription.unsubscribe();
    this.loadingCreditStateSubscription.unsubscribe()
  }

  openBankAccountView(account: any) {
    this.accountControlService.changeSelectedAccountId({
      Id: account.Id,
      Type: AccountType.Bank
    });
  }

  openCreditAccountView(account: any) {
    this.accountControlService.changeSelectedAccountId({
      Id: account.Id,
      Type: AccountType.Credit
    });
  }

}

