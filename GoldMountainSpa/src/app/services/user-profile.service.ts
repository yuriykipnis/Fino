import { Observable } from 'rxjs';
import {DEV_USER, UserProfile} from "../models/user.profile";
import {Injectable} from '@angular/core';
import { Store } from '@ngrx/store';
import {AppState} from "../shared/store/app.states";
import * as userProfileReducer from '../shared/store/reducers/user-profile.reducer';

@Injectable()
export class UserProfileService {
  userProfile$ : Observable<UserProfile>;

  constructor(private store: Store<AppState>) {
    this.userProfile$ = store.select(userProfileReducer.getUserProfile);
  }

  get(): Observable<UserProfile> {
    return this.userProfile$;
  }
}

