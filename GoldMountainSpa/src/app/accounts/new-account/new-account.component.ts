import { Component, HostBinding, OnInit, OnDestroy } from '@angular/core';
import { slideInOutAnimation } from '../../animations/slide-in-out.animations';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import {Institution, InstitutionType} from "../../models/institution";
import { Observable } from 'rxjs';
import { TextboxQuestion } from "./textboxQuestion";
import { ProviderService } from "../../services/provider.service";
import { Store } from '@ngrx/store';
import {UserProfileService} from "../../services/user-profile.service";
import {AddBankAccount } from "../store/actions/bank-account.action";
import {AppState} from '../../shared/store/app.states';
import {UserProfile} from "../../models/user.profile";
import {InstitutionService} from "../../services/institution.service";
import {AddCreditAccount} from "../store/actions/credit-account.action";
import {CreatingAccount} from "../models/creating-account";
import {CreditAccount} from "../models/credit-account";
import {BankAccount} from "../models/bank-account";
import {BankService} from "../../services/bank.service";
import {CreditService} from "../../services/credit.service";
import {AccountService} from '../../services/account.service';

@Component({
  selector: 'app-new-account',
  templateUrl: './new-account.component.html',
  styleUrls: ['./new-account.component.scss'],
  animations: [slideInOutAnimation],
})
export class NewAccountComponent implements OnInit, OnDestroy {
  institutions: Institution[];
  accounts: CreatingAccount[];
  selectedInstitution: Institution;
  userProfile: UserProfile;
  errorMessage: String;

  @HostBinding('@slideInOutAnimation') slideInOutAnimation = '';
  credentialsForm: FormGroup;
  isLoading = false;
  isSaving = false;

  constructor(private router: Router, public route: ActivatedRoute,
              private store: Store<AppState>,
              private institutionService: InstitutionService,
              private bankService: BankService,
              private creditService: CreditService,
              private providerService: ProviderService,
              private userProfileService: UserProfileService) {
    this.isLoading = false;
    this.isSaving = false;
    this.errorMessage = null;
  }

  ngOnInit() {
    this.userProfileService.userProfile$.subscribe(up => {
      this.userProfile = up;
      this.institutionService.getInstitutions$().subscribe(
        res => {
          this.institutions = res;
          if (!this.selectedInstitution) {
            this.selectedInstitution = res.find(elem => elem.IsSupported);
            this.credentialsForm = this.getCredentialsForm(this.selectedInstitution);
          }
        },
        err => {
        });
    });
  }

  ngOnDestroy() {
  }

  onSwitchToggle(account) {
    account.IsActive = !account.IsActive;
  }

  onSwitchItem(item: Institution) {
    this.selectedInstitution = item;
    this.credentialsForm = this.getCredentialsForm(this.selectedInstitution);
    this.accounts = null;
  }

  load() {
    let credentials = this.credentialsForm.value;
    this.isLoading = true;
    this.errorMessage = null;
    let accountService : AccountService = null;

    if (this.selectedInstitution.Type === InstitutionType.Bank) {
      accountService = this.bankService;
    } else if(this.selectedInstitution.Type === InstitutionType.Credit) {
      accountService = this.creditService;
    }

    accountService.getExistingAccounts$(this.userProfile.Id, this.selectedInstitution.Name, credentials).subscribe(
      res => {
        this.accounts = res;
        this.isLoading = false;
      },
      err => {
        this.isLoading = false;
        this.errorMessage = err.error;
      });
  }

  save() {
    this.isSaving = true;
    this.addProvider();
  }

  cancel() {
    this.router.navigate(['../'], {relativeTo: this.route});
  }

  private getCredentialsForm(item: Institution): FormGroup {
    let questions: TextboxQuestion[] = [];

    item.Credentials.forEach(c => {
      questions.push(new TextboxQuestion(c, "", c));
    });

    return this.toFormGroup(questions);
  }

  private toFormGroup(questions: TextboxQuestion[] ) {
    let group: any = {};

    questions.forEach(question => {
      group[question.key] = new FormControl(question.value || '', Validators.required);
    });
    return new FormGroup(group);
  }

  private addProvider(): any {
    let credentials = this.credentialsForm.value;
    let newAccounts = this.accounts.filter(ac => ac.IsActive);

    if (this.selectedInstitution.Type === InstitutionType.Bank){
      this.providerService.addProvider$(this.userProfile.Id, this.selectedInstitution.Name, this.selectedInstitution.Type, credentials, newAccounts, [])
        .subscribe(res => {
            this.isSaving = false;
            res.forEach(newAccount => {
              this.store.dispatch(new AddBankAccount(newAccount));
            });

            this.router.navigate(['../'], {relativeTo: this.route});
          },
          err => {
            this.isSaving = false;
            //this.alertsService.handleServerError({err, key: this.streamName});
          });
    }
    else if (this.selectedInstitution.Type === InstitutionType.Credit){
        this.providerService.addProvider$(this.userProfile.Id, this.selectedInstitution.Name, this.selectedInstitution.Type, credentials, [], newAccounts)
          .subscribe(res => {
              this.isSaving = false;
              res.forEach(newAccount => {
                this.store.dispatch(new AddCreditAccount(newAccount));
              });

              this.router.navigate(['../'], {relativeTo: this.route});
            },
            err => {
              this.isSaving = false;
              //this.alertsService.handleServerError({err, key: this.streamName});
            });
    }
  }

  private isBankAccount(account : CreatingAccount) : boolean {
    let result = account instanceof BankAccount;
    return result;
  }

  private isCreditAccount(account : CreatingAccount) : boolean {
    let result = account instanceof CreditAccount;
    return result;
  }
}
