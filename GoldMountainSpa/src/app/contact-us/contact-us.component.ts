import { Component, OnInit, ViewEncapsulation, AfterViewInit } from '@angular/core';
import {FormGroup} from '@angular/forms';
import {FormBuilder} from '@angular/forms';
import {MessageService} from 'primeng/primeng';
import {FormControl} from '@angular/forms';
import {Validators} from '@angular/forms';

@Component({
  selector: 'app-contact-us',
  templateUrl: './contact-us.component.html',
  styleUrls: ['./contact-us.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ContactUsComponent implements OnInit {
  emailRegexp = new RegExp(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/);
  name: string;
  email: string;
  phone: string;
  subject: string;
  message: string;

  isSubmitted : boolean;

  constructor() {
  }

  ngOnInit() {
    this.isSubmitted = false;
  }

  onSend() {

  }

  get isValid() : boolean {
    let isEmailValid = this.emailRegexp.test(this.email);

    if (this.name && this.name.length > 0 &&
        this.email && isEmailValid &&
        this.message && this.message.length > 0) {
      return true;
    }

    return false;
  }
}
