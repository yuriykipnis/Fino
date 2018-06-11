import { Component, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { Store } from '@ngrx/store';
import { UserProfile } from "../../models/user.profile";
import { AppState } from "../../shared/store/app.states";
import * as userProfileReducer from '../../shared/store/reducers/user-profile.reducer';
import * as fromActions from '../../shared/store/actions/user-profile.action';
import {AuthService} from "../../auth/auth.service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnChanges {

  profile$: Observable<UserProfile>;
  private currentTab = 'accounts';

  constructor(private store: Store<AppState>,
              private authService: AuthService,
              private router: Router) {
    this.profile$ = store.select(userProfileReducer.getUserProfile);
  }

  ngOnInit() {
    // this.store.dispatch(new fromActions.FetchProfile());
  }

  ngOnChanges(changes: SimpleChanges): void {

  }

  switchTab(tab) {
      console.log("Switched tab to - ", tab);
      this.currentTab = tab;
  }
}
