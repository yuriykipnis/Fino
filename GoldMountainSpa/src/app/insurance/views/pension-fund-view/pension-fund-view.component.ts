import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/switchMap';
import {PensionFundProfile} from "../../models/pension-fund.profile";
import {PensionFundService} from "../../services/pension-fund.service";

@Component({
  selector: 'app-pension-fund-view',
  templateUrl: './pension-fund-view.component.html',
  styleUrls: ['./pension-fund-view.component.scss']
})
export class PensionFundViewComponent implements OnInit {
  profile$: Observable<PensionFundProfile>;

  constructor(public route: ActivatedRoute,
              private pensionFundService: PensionFundService) {
  }

  ngOnInit() {
    this.profile$ = this.route.paramMap
      .switchMap((params: ParamMap) =>
        this.pensionFundService.getProfile$(params.get('ProfileId')));
  }
}
