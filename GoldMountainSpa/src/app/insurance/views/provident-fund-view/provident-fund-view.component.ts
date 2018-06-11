import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/switchMap';
import {ProvidentFundProfile} from "../../models/provident-fund.profile";
import {ProvidentFundService} from "../../services/provident-fund.service";

@Component({
  selector: 'app-provident-fund-view',
  templateUrl: './provident-fund-view.component.html',
  styleUrls: ['./provident-fund-view.component.scss']
})
export class ProvidentFundViewComponent implements OnInit {
  profile$: Observable<ProvidentFundProfile>;

  constructor(public route: ActivatedRoute,
              private providentFundService: ProvidentFundService) {
  }

  ngOnInit() {
    this.profile$ = this.route.paramMap
      .switchMap((params: ParamMap) =>
        this.providentFundService.getProfile$(params.get('ProfileId')));
  }
}
