import { Component, AfterViewInit } from '@angular/core';
import { AuthService } from "./auth/auth.service";
import {OnInit} from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit{
  title = 'app';

  constructor(public authService: AuthService) {
  }

  ngOnInit() {
    this.authService.handleAuthentication();
  }
}

