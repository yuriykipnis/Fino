import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { AppState } from "../../shared/store/app.states";
import { Router } from '@angular/router';
import { AuthService } from "../auth.service";
import * as fromActions from '../../shared/store/actions/user-profile.action';

@Component({
  selector: 'app-login-callback',
  templateUrl: './login-callback.component.html',
  styleUrls: ['./login-callback.component.scss']
})

export class LoginCallbackComponent implements OnInit {

  constructor(private store: Store<AppState>,
              public router: Router,
              private authService: AuthService) {
  }

  ngOnInit() {
    this.authService.handleLoginCallback();
    this.router.navigate(['/']);
  }
}
