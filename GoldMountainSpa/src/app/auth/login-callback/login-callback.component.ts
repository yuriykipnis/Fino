import { Component, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { AppState } from "../../shared/store/app.states";
import { Router, NavigationEnd } from '@angular/router';
import { AuthService } from "../auth.service";

@Component({
  selector: 'app-login-callback',
  templateUrl: './login-callback.component.html',
  styleUrls: ['./login-callback.component.scss']
})

export class LoginCallbackComponent implements OnInit{

  constructor(private store: Store<AppState>,
              public router: Router,
              private authService: AuthService) {
  }

  ngOnInit() {
    this.authService.handleLoginCallback();
  }
}
