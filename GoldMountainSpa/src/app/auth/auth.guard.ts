import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';
import {ActivatedRoute} from '@angular/router';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  canActivate(next: ActivatedRouteSnapshot,state: RouterStateSnapshot)
    : Observable<boolean> | Promise<boolean> | boolean
  {
    if (!this.authService.isLoggedIn) {
      //window.location.href = '/login';
      this.authService.login();
      return false;
    }
    return true;
  }

}
