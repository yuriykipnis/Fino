import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import {CreditAccount} from "../../models/credit-account";

@Component({
  selector: 'app-sidebar-credit-account',
  templateUrl: './sidebar-credit-account.component.html',
  styleUrls: ['./sidebar-credit-account.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SidebarCreditAccountComponent implements OnInit {
  @Input() account: CreditAccount;
  constructor() { }

  ngOnInit() {
  }

  getTotalCharge(account){
    let result: number = 0;
    let now = new Date();
    account.Transactions.filter(t => {
      let tDate = new Date(t.PaymentDate);
      return tDate.getMonth() === now.getMonth() && tDate.getFullYear() === now.getFullYear();
    }).forEach(t => {
      result += t.Amount;
    });

    return result;
  }
}
