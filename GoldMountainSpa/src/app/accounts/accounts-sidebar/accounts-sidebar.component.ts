import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {AppState} from "../../shared/store/app.states";
import { Store } from '@ngrx/store';
import {Observable} from 'rxjs/Observable';
import {Subscription} from 'rxjs/Subscription';
import * as bankAccountReducer from '../store/reducers/bank-account.reducer';
import * as creditAccountReducer from '../store/reducers/credit-account.reducer';
import {BankAccount} from "../models/bank-account";
import {CreditAccount} from "../models/credit-account";
import {TransactionType} from "../../models/transaction";
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

  constructor(private store: Store<AppState>,
              private router: Router, private route: ActivatedRoute,
              public accountSummaryService: AccountsSummaryService) {
    this.bankAccounts$ = store.select(bankAccountReducer.getBankAccounts);
    this.creditAccounts$ = store.select(creditAccountReducer.getCreditAccounts);
  }

  ngOnInit() {

  }

  ngOnDestroy() {

  }

  openBankAccountView(account: any) {
    this.router.navigate(['bankaccount/' + account.Id], {relativeTo: this.route});
  }

  openCreditAccountView(account: any) {
    this.router.navigate(['creditaccount/' + account.Id], {relativeTo: this.route});
  }

  showSummary() {
    this.router.navigate(['accounts/summary']);
  }

  showIncome(){
    this.router.navigate(['accounts/income']);
  }

  showExpense(){
    this.router.navigate(['accounts/expense']);
  }
}

