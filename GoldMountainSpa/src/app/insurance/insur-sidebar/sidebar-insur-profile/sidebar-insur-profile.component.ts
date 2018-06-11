import { CurrencyPipe } from '@angular/common';
import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import {BasicInsurProfile} from "../../models/basic-insur.profile";

@Component({
  selector: 'app-sidebar-insur-profile',
  templateUrl: './sidebar-insur-profile.component.html',
  styleUrls: ['./sidebar-insur-profile.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SidebarInsurProfileComponent implements OnInit {
  @Input() profile: BasicInsurProfile;

  constructor() {
  }

  ngOnInit() {
  }

}
