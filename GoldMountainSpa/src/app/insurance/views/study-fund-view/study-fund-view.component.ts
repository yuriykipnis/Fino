import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/switchMap';
import {StudyFundProfile} from "../../models/study-fund.profile";
import {StudyFundService} from "../../services/study-fund.service";

@Component({
  selector: 'app-efund-view',
  templateUrl: './study-fund-view.component.html',
  styleUrls: ['./study-fund-view.component.scss']
})
export class StudyFundViewComponent implements OnInit, OnDestroy {
  profile$: Observable<StudyFundProfile>;

  constructor(public route: ActivatedRoute,
              private studyFundService: StudyFundService) {
  }

  ngOnInit() {
    this.profile$ = this.route.paramMap
      .switchMap((params: ParamMap) =>
        this.studyFundService.getProfile$(params.get('ProfileId')));
  }

  ngOnDestroy() {
  }
}
