import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { AppState } from "../shared/store/app.states";
import {BankService} from "../services/bank.service";
import {CreditService} from "../services/credit.service";
import {UserProfileService} from "../services/user-profile.service";

@Component({
  selector: 'app-loans',
  templateUrl: './loans.component.html',
  styleUrls: ['./loans.component.scss']
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
            this.isLoansLoading = false;
          },
          err => {
            this.isLoansLoading = false;
          });

      this.creditService.getAccounts$(up.Id)
        .subscribe(res => {
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

  isLoading() {
    return this.isLoansLoading;
  }
}
