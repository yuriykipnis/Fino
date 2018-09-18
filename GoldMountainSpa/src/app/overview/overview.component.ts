import { Component, OnInit } from '@angular/core';
import {Router} from '@angular/router';
import {NavigationEnd} from '@angular/router';
import {AfterViewInit} from '@angular/core';

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.scss']
})
export class OverviewComponent implements OnInit {

  constructor(public router: Router) {

  }

  ngOnInit() {
  }

}
