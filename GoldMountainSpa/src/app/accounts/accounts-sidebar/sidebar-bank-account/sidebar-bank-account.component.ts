import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import {BankAccount} from "../../models/bank-account";
import {AccountControlService} from "../../services/account-control.service";

@Component({
  selector: 'app-sidebar-bank-account',
  templateUrl: './sidebar-bank-account.component.html',
  styleUrls: ['./sidebar-bank-account.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SidebarBankAccountComponent implements OnInit {
  @Input() account: BankAccount;
  constructor(private accountControlService: AccountControlService) {
  }

  ngOnInit() {
  }

  isSelected() {
    return this.account.Id == this.accountControlService.getSelectedAccount().Id;
  }
}
