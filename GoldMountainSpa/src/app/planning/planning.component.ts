import { Component, OnInit, OnDestroy } from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {Transaction} from "../models/transaction";
import {AuthService} from "../auth/auth.service";

@Component({
  selector: 'app-budget',
  templateUrl: './planning.component.html',
  styleUrls: ['./planning.component.scss']
})
export class PlanningComponent implements OnInit, OnDestroy {

  transactionItems: Transaction[];

  constructor(private router: Router, private route: ActivatedRoute,
              private authService: AuthService)  {

  }

  onRowSelect(event) {
    if (event.data.Id) {
      //this.router.navigate([event.data.Id], {relativeTo: this.route});
    }
  }

  ngOnInit() {
    //this.authService.login();
  }

  ngOnDestroy(): void {
  }

}

