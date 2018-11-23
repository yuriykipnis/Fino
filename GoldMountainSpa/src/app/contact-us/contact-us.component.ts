import { Component, OnInit, ViewEncapsulation, AfterViewInit } from '@angular/core';
import {FormGroup} from '@angular/forms';
import {FormBuilder} from '@angular/forms';
import {MessageService} from 'primeng/primeng';
import {FormControl} from '@angular/forms';
import {Validators} from '@angular/forms';
import {ContactService} from '../services/contact.service';
import {ContactMessage} from "../models/contactMessage";
import {ProviderService} from "../services/provider.service";
import {UserProfileService} from "../services/user-profile.service";
import {UserProfile} from "../models/user.profile";

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
  isSent:boolean;
  userProfile: UserProfile;

  constructor(private contactService: ContactService,
              private userProfileService: UserProfileService) {
  }

  ngOnInit() {
    this.isSubmitted = false;
    this.isSent = false;

    this.userProfileService.userProfile$.subscribe(up => {
      this.userProfile = up;
    });
  }

  onSend() {
    this.isSubmitted = true;

    if ( !this.isValid ){
      return;
    }

    this.contactService.sendMessage$(this.userProfile.Id, new ContactMessage({
      Username: this.name,
      Email: this.email,
      PhoneNumber: this.phone,
      Subject: this.subject,
      Message: this.message
    })).subscribe(res => {
      this.isSent = true;
    });
  }


  get isValid() : boolean {
    return this.isNameValid && this.isEmailValid && this.isMessageValid;
  }

  get isNameValid() : boolean {
    return this.name && this.name.length > 0;
  }

  get isEmailValid() : boolean {
    let isEmailValid = this.emailRegexp.test(this.email);
    return this.email && isEmailValid;
  }

  get isMessageValid() : boolean {
    return this.message && this.message.length > 0;
  }

  getNameBackground() {
    let styles = {
      'background-color': this.isSubmitted && !this.isNameValid ? 'pink' : 'white'
    };
    return styles;
  }

  getEmailBackground() {
    let styles = {
      'background-color': this.isSubmitted && !this.isEmailValid ? 'pink' : 'white'
    };
    return styles;
  }

  getMessageBackground() {
    let styles = {
      'background-color': this.isSubmitted && !this.isMessageValid ? 'pink' : 'white'
    };
    return styles;
  }
}
