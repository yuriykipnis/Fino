import { Component, OnInit, ViewEncapsulation } from '@angular/core';
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
  userform: FormGroup;
  submitted: boolean;
  message: string;

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.userform = this.fb.group({
      'name': new FormControl('', Validators.required),
      'email': new FormControl('', Validators.required),
      'message': new FormControl(''),
    });
  }

  onSubmit(value: string) {
    //this.submitted = true;
    //this.messageService.add({severity:'info', summary:'Success', detail:'Form Submitted'});
  }


}
