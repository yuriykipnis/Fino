import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, Params, ActivatedRoute, ParamMap, RoutesRecognized } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import {Subscription} from 'rxjs/Subscription';
import {AppState} from "../shared/store/app.states";
import { Store } from '@ngrx/store';
import {UserProfileService} from "../services/user-profile.service";
import {StudyFundService} from "./services/study-fund.service";
import {SeInsurService} from "./services/se-insur.service";
import {ProvidentFundService} from "./services/provident-fund.service";
import {MortgageInsurService} from "./services/mortgage-insur.service";
import {PensionFundService} from "./services/pension-fund.service";
import 'rxjs/add/operator/switchMap';

import * as fromStudyFundActions from './store/actions/study-fund-profile.action';
import * as fromSeInsurActions from './store/actions/se-insur-profile.action';
import * as fromProvidentActions from './store/actions/provident-fund-profile.action';
import * as fromMortgageInsurActions from './store/actions/mortgage-insur-profile.action';
import * as fromPensionFundActions from './store/actions/pension-fund-profile.action';


@Component({
  selector: 'app-insurance',
  templateUrl: './insurance.component.html',
  styleUrls: ['./insurance.component.scss']
})
export class InsuranceComponent implements OnInit, OnDestroy {
  private userProfileSubscription: Subscription;
  studyFundSavings: number = 0;
  seInsurSavings: number = 0;
  pensionFundSavings: number = 0;

  studyFundLoading: Boolean = false;
  seInsurLoading: Boolean = false;
  providentFundLoading: Boolean = false;
  mortgageInsurLoading: Boolean = false;
  pensionFundLoading: Boolean = false;

  constructor(private route: ActivatedRoute, private router: Router,
              private store: Store<AppState>,
              private studyFundService: StudyFundService,
              private seInsurService: SeInsurService,
              private providentFundService: ProvidentFundService,
              private mortgageInsurService: MortgageInsurService,
              private pensionFundService: PensionFundService,
              private userProfileService: UserProfileService) {
  }

  ngOnInit() {
    this.studyFundLoading = true;
    this.seInsurLoading = true;
    this.providentFundLoading = true;
    this.mortgageInsurLoading = true;
    this.pensionFundLoading = true;

    this.userProfileSubscription = this.userProfileService.userProfile$.subscribe(up => {
      if (!up || !up.Id) {
        this.store.dispatch(new fromStudyFundActions.FetchStudyFundProfiles([]));
        this.store.dispatch(new fromSeInsurActions.FetchSeInsurProfiles([]));
        this.store.dispatch(new fromProvidentActions.FetchProvidentFundProfiles([]));
        this.store.dispatch(new fromMortgageInsurActions.FetchMortgageInsurProfiles([]));
        this.store.dispatch(new fromPensionFundActions.FetchPensionFundProfiles([]));
        return;
      }

      this.studyFundService.getAccounts$(up.Id, up.PassportId)
        .subscribe(res => {
            res.forEach(sf => {
              this.studyFundSavings += sf.TotalSavings;
            });
            this.store.dispatch(new fromStudyFundActions.FetchStudyFundProfiles(res));
            this.studyFundLoading = false;
          },
          err => {
            this.studyFundLoading = false;
          });

      this.seInsurService.getAccounts$(up.Id, up.PassportId)
        .subscribe(res => {
            res.forEach(si => {
              this.seInsurSavings += si.TotalSavings;
            });
            this.store.dispatch(new fromSeInsurActions.FetchSeInsurProfiles(res));
            this.seInsurLoading = false;
          },
          err => {
            this.seInsurLoading = false;
          });

      this.providentFundService.getAccounts$(up.Id, up.PassportId)
        .subscribe(res => {
            this.store.dispatch(new fromProvidentActions.FetchProvidentFundProfiles(res));
            this.providentFundLoading = false;
          },
          err => {
            this.providentFundLoading = false;
          });

      this.mortgageInsurService.getAccounts$(up.Id, up.PassportId)
        .subscribe(res => {
            this.store.dispatch(new fromMortgageInsurActions.FetchMortgageInsurProfiles(res));
            this.mortgageInsurLoading = false;
          },
          err => {
            this.mortgageInsurLoading = false;
          });

      this.pensionFundService.getAccounts$(up.Id, up.PassportId)
        .subscribe(res => {
            res.forEach(pf => {
              this.pensionFundSavings += pf.TotalSavings;
            });
            this.store.dispatch(new fromPensionFundActions.FetchPensionFundProfiles(res));
            this.pensionFundLoading = false;
          },
          err => {
            this.pensionFundLoading = false;
          });
    });
  }

  ngOnDestroy() {
    this.userProfileSubscription.unsubscribe();
    this.store.dispatch(new fromStudyFundActions.FetchStudyFundProfiles([]));
    this.store.dispatch(new fromSeInsurActions.FetchSeInsurProfiles([]));
    this.store.dispatch(new fromProvidentActions.FetchProvidentFundProfiles([]));
    this.store.dispatch(new fromMortgageInsurActions.FetchMortgageInsurProfiles([]));
    this.store.dispatch(new fromPensionFundActions.FetchPensionFundProfiles([]));
  }

  isLoading() {
    return this.studyFundLoading || this.seInsurLoading || this.providentFundLoading ||
      this.pensionFundLoading || this.mortgageInsurLoading;
  }
}
