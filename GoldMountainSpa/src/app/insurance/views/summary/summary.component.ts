import { Component, OnInit, OnDestroy } from '@angular/core';
import {ProvidentFundProfile} from "../../models/provident-fund.profile";
import {SeInsurProfile} from "../../models/se-insur.profile";
import {StudyFundProfile} from "../../models/study-fund.profile";
import {MortgageInsurProfile} from "../../models/mortgage-insur.profile";
import {PensionFundProfile} from "../../models/pension-fund.profile";
import {Observable} from 'rxjs/Observable';
import {Subscription} from 'rxjs/Subscription';
import {AppState} from "../../../shared/store/app.states";
import { Store } from '@ngrx/store';
import * as studyFundProfileReducer from '../../store/reducers/study-fund-profile.reducer';
import * as seInsurProfileReducer from '../../store/reducers/se-insur-profile.reducer';
import * as providentFundsProfileReducer from '../../store/reducers/provident-fund-profile.reducer';
import * as mortgageInsurProfileReducer from '../../store/reducers/mortgage-insur-profile.reducer';
import * as pensionFundProfileReducer from '../../store/reducers/pension-fund-profile.reducer';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.scss']
})
export class SummaryComponent implements OnInit, OnDestroy {
  data: any;
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

  constructor(private store: Store<AppState>) {
    this.studyFundProfiles$ = store.select(studyFundProfileReducer.getStudyFundProfiles);
    this.seInsurProfiles$ = store.select(seInsurProfileReducer.getSeInsurProfiles);
    this.providentFundProfiles$ = store.select(providentFundsProfileReducer.getProvidentFundProfiles);
    this.mortgageInsurProfiles$ = store.select(mortgageInsurProfileReducer.getMortgageInsurProfiles);
    this.pensionFundProfiles$ = store.select(pensionFundProfileReducer.getPensionFundProfiles);
  }

  ngOnInit() {
    this.studyFundProfilesSubscription = this.studyFundProfiles$.subscribe(res => {
        res.forEach( p => this.studyFundSavings += p.TotalSavings)
        this.changeData();
      });

    this.seInsurProfilesSubscription = this.seInsurProfiles$.subscribe(res => {
      res.forEach( p => this.seInsurSavings += p.TotalSavings)
      this.changeData();
    });

    this.pensionFundProfilesSubscription = this.pensionFundProfiles$.subscribe(res => {
      res.forEach( p => this.pensionFundSavings += p.TotalSavings)
      this.changeData();
    });

    this.data = {
      labels: ['Study Fund','Senior Employee Insurance','Pension Fund'],
      datasets: [
        {
          data: [this.studyFundSavings, this.seInsurSavings, this.pensionFundSavings],
          backgroundColor: [
            "#FF6384",
            "#36A2EB",
            "#FFCE56"
          ],
          hoverBackgroundColor: [
            "#FF6384",
            "#36A2EB",
            "#FFCE56"
          ]
        }]
    };
  }

  ngOnDestroy() {
    this.studyFundProfilesSubscription.unsubscribe();
    this.seInsurProfilesSubscription.unsubscribe();
    this.pensionFundProfilesSubscription.unsubscribe();
  }

  changeData() {
    this.data = {
      labels: ['Study Fund','Senior Employee Insurance','Pension Fund'],
      datasets: [
        {
          data: [this.studyFundSavings, this.seInsurSavings, this.pensionFundSavings],
          backgroundColor: [
            "#FF6384",
            "#36A2EB",
            "#FFCE56"
          ],
          hoverBackgroundColor: [
            "#FF6384",
            "#36A2EB",
            "#FFCE56"
          ]
        }]
    };
  }
}
