import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/switchMap';
import {SeInsurProfile} from "../../models/se-insur.profile";
import {SeInsurService} from "../../services/se-insur.service";

@Component({
  selector: 'app-se-insur-view',
  templateUrl: './se-insur-view.component.html',
  styleUrls: ['./se-insur-view.component.scss']
})
export class SeInsurViewComponent implements OnInit {
  profile$: Observable<SeInsurProfile>;

  constructor(public route: ActivatedRoute,
              private seInsurService: SeInsurService) {
  }

  ngOnInit() {
    this.profile$ = this.route.paramMap
      .switchMap((params: ParamMap) =>
        this.seInsurService.getProfile$(params.get('ProfileId')));
  }

}
