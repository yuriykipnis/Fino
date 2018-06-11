import { Component, OnInit, Input } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { BankAccount } from "../../models/bank-account";
import {Transaction, TransactionType} from "../../../models/transaction";
import { BankService } from "../../../services/bank.service";
import {CreditService} from "../../../services/credit.service";
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/switchMap';

@Component({
  selector: 'app-transactions-view',
  templateUrl: './transactions-view.component.html',
  styleUrls: ['./transactions-view.component.scss']
})
export class TransactionsViewComponent implements OnInit {
  account$: Observable<BankAccount>;
  transactions$: Observable<Transaction[]>;
  accountId: string;
  period: Date;
  accountService: any;

  constructor(private router: Router, public route: ActivatedRoute,
              private bankService: BankService,
              private creditService: CreditService) {
    this.period = new Date();
    this.period.toDateString()
  }

  ngOnInit() {
    if (this.route.snapshot.url[0].path === 'bankaccount'){
      this.accountService = this.bankService;
    }
    else if (this.route.snapshot.url[0].path === 'creditaccount'){
      this.accountService = this.creditService;
    }

    this.transactions$ = this.retriveTransactions();
  }

  getTransactionColor(type : TransactionType)
  {
    return type === TransactionType.Income ? "green" : "red";
  }

  getBalanceColor(balance : number)
  {
    return balance > 0 ? "green" : "red";
  }

  prevMonth(){
    if (this.period.getMonth() == 0) {
      this.period.setFullYear(this.period.getFullYear() - 1);
      this.period.setMonth(11);
    } else {
      this.period.setMonth(this.period.getMonth() - 1);
    }

    this.transactions$ = this.retriveTransactions();
  }

  nextMonth(){
    if (this.period.getMonth() == 11) {
      this.period.setFullYear(this.period.getFullYear() + 1);
      this.period.setMonth(0);
    } else {
      this.period.setMonth(this.period.getMonth() + 1);
    }

    this.transactions$ = this.retriveTransactions();
  }

  private retriveTransactions() : Observable<Transaction[]>{
    return this.route.paramMap
      .switchMap((params: ParamMap) => {
        this.accountId = params.get('AccountId');
        return this.accountService.getTransactions$(this.accountId, this.period);
      });
  }
}
