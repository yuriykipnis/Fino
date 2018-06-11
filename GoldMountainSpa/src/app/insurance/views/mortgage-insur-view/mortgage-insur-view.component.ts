import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/switchMap';
import {MortgageInsurService} from "../../services/mortgage-insur.service";
import {MortgageInsurProfile} from "../../models/mortgage-insur.profile";

@Component({
  selector: 'app-mortgage-insur-view',
  templateUrl: './mortgage-insur-view.component.html',
  styleUrls: ['./mortgage-insur-view.component.scss']
})
export class MortgageInsurViewComponent implements OnInit {
  profile$: Observable<MortgageInsurProfile>;

  constructor(public route: ActivatedRoute,
              private mortgageInsurService: MortgageInsurService) {
  }

  ngOnInit() {
    this.profile$ = this.route.paramMap
      .switchMap((params: ParamMap) =>
        this.mortgageInsurService.getProfile$(params.get('ProfileId')));
  }
}
