import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { filter } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import {AppState} from "../shared/store/app.states";
import * as auth0 from 'auth0-js';
import {environment} from "../../environments/environment";
import * as fromActions from '../shared/store/actions/user-profile.action';
import * as userProfileReducer from '../shared/store/reducers/user-profile.reducer';
import {UserProfile} from "../models/user.profile";
import {Observable} from 'rxjs/Observable';

@Injectable()
export class AuthService {

  auth0 = new auth0.WebAuth({
    clientID: environment.auth.clientID,
    domain: environment.auth.domain,
    responseType: 'token id_token',
    redirectUri: environment.auth.redirectUri,
    audience: environment.auth.audience,
    scope: 'openid profile email'
  });

  userProfile: Observable<UserProfile>;

  constructor(private store: Store<AppState>,
              public router: Router) {
    this.userProfile = store.select(userProfileReducer.getUserProfile);
  }

  public login(): void {
    this.auth0.authorize();
  }

  public logout(): void {
    localStorage.removeItem('expires_at');
    localStorage.removeItem('access_token');
    localStorage.removeItem('id_token');

    this.store.dispatch(new fromActions.SetProfile(new UserProfile()));
    this.router.navigate(['../']);
  }

  public handleAuthentication(): void {
    if (this.isLoggedIn)  {
      var accessToken = localStorage.getItem('access_token');
      this.setProfile(accessToken);
    }
  }

  public handleLoginCallback() {
    this.auth0.parseHash((err, authResult) => {
      if (authResult && authResult.accessToken) {
        window.location.hash = '';
        this.setProfile(authResult.accessToken);
        this.setSession(authResult);
      } else if (err) {
        console.error(`Error: ${err.error}`);
      }
    });
  }

  private setSession(authResult) {
    const expTime = authResult.expiresIn * 1000 + Date.now();
    localStorage.setItem('expires_at', JSON.stringify(expTime));
    localStorage.setItem('access_token', authResult.accessToken);
    localStorage.setItem('id_token', authResult.idToken);
  }

  private setProfile(accessToken) {
    if (accessToken) {
      var self = this;
      this.auth0.client.userInfo(accessToken, (err, user) => {
        var namespace = 'https://gold-mountain:eu:auth0:com/';

        let profile : UserProfile = {
          Id: user.sub.split('|')[1],
          PassportId: user[namespace + 'user_metadata'].passportId,
          Email: user.email,
          FirstName: "",
          LastName: "",
          FullName: user.nickname
        };

        self.store.dispatch(new fromActions.SetProfile(profile));
      }, {});
    }
  }

  public get isLoggedIn(): boolean {
    const expiresAt = JSON.parse(localStorage.getItem('expires_at'));
    return Date.now() < expiresAt;
  }
}
