import { CurrencyPipe } from '@angular/common';
import { Component, EventEmitter, OnInit, OnDestroy, Input, Output, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {Observable} from 'rxjs/Observable';
import {AppState} from "../../shared/store/app.states";
import { Store } from '@ngrx/store';
import {Subscription} from 'rxjs';
import * as studyFundProfileReducer from '../store/reducers/study-fund-profile.reducer';
import * as seInsurProfileReducer from '../store/reducers/se-insur-profile.reducer';
import * as providentFundsProfileReducer from '../store/reducers/provident-fund-profile.reducer';
import * as mortgageInsurProfileReducer from '../store/reducers/mortgage-insur-profile.reducer';
import * as pensionFundProfileReducer from '../store/reducers/pension-fund-profile.reducer';

import {StudyFundProfile} from "../models/study-fund.profile";
import {SeInsurProfile} from "../models/se-insur.profile";
import {ProvidentFundProfile} from "../models/provident-fund.profile";
import {MortgageInsurProfile} from "../models/mortgage-insur.profile";
import {PensionFundProfile} from "../models/pension-fund.profile";



@Component({
  selector: 'app-insur-sidebar',
  templateUrl: './insur-sidebar.component.html',
  styleUrls: ['./insur-sidebar.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InsurSidebarComponent implements OnInit, OnDestroy {
  studyFundProfiles$: Observable<StudyFundProfile[]>;
  seInsurProfiles$: Observable<SeInsurProfile[]>;
  providentFundProfiles$: Observable<ProvidentFundProfile[]>;
  mortgageInsurProfiles$: Observable<MortgageInsurProfile[]>;
  pensionFundProfiles$: Observable<PensionFundProfile[]>;

  private studyFundProfilesSubscription : Subscription;
  private seInsurProfilesSubscription : Subscription;
  private pensionFundProfilesSubscription : Subscription;

  studyFundSavings: number = 0;
  seInsurSavings: number = 0;
  pensionFundSavings: number = 0;

  @Output() onProfileSelected = new EventEmitter<any>();

  constructor(private store: Store<AppState>,
              private router: Router, private route: ActivatedRoute) {
    this.studyFundProfiles$ = store.select(studyFundProfileReducer.getStudyFundProfiles);
    this.seInsurProfiles$ = store.select(seInsurProfileReducer.getSeInsurProfiles);
    this.providentFundProfiles$ = store.select(providentFundsProfileReducer.getProvidentFundProfiles);
    this.mortgageInsurProfiles$ = store.select(mortgageInsurProfileReducer.getMortgageInsurProfiles);
    this.pensionFundProfiles$ = store.select(pensionFundProfileReducer.getPensionFundProfiles);
  }

  ngOnInit() {
    this.studyFundProfilesSubscription = this.studyFundProfiles$.subscribe(res => {
      res.forEach( p => this.studyFundSavings += p.TotalSavings)
    });

    this.seInsurProfilesSubscription = this.seInsurProfiles$.subscribe(res => {
      res.forEach( p => this.seInsurSavings += p.TotalSavings)
    });

    this.pensionFundProfilesSubscription = this.pensionFundProfiles$.subscribe(res => {
      res.forEach( p => this.pensionFundSavings += p.TotalSavings)
    });
  }

  ngOnDestroy() {
    this.studyFundProfilesSubscription.unsubscribe();
    this.seInsurProfilesSubscription.unsubscribe();
    this.pensionFundProfilesSubscription.unsubscribe();
  }

  showEfundProfile(profile: any) {
    this.onProfileSelected.emit(profile);
    this.router.navigate(['studyfund/' + profile.Id], {relativeTo: this.route});
  }

  showSeInsurProfile(profile: any) {
    this.onProfileSelected.emit(profile);
    this.router.navigate(['seinsur/' + profile.Id], {relativeTo: this.route});
  }

  showPrevidentFundProfile(profile: any) {
    this.onProfileSelected.emit(profile);
    this.router.navigate(['providentfund/' + profile.Id], {relativeTo: this.route});
  }

  showMortgageInsurProfile(profile: any) {
    this.onProfileSelected.emit(profile);
    this.router.navigate(['mortgageinsur/' + profile.Id], {relativeTo: this.route});
  }

  showPensionFundProfile(profile: any) {
    this.onProfileSelected.emit(profile);
    this.router.navigate(['pensionfund/' + profile.Id], {relativeTo: this.route});
  }

  showSummary() {
    this.onProfileSelected.emit(null);
    this.router.navigate(['insurance/summary']);
  }
}

