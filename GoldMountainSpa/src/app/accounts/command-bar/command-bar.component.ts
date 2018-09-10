import {Component, OnDestroy, OnInit, ViewEncapsulation} from '@angular/core';
import {AccountControlService, TableType} from '../services/account-control.service';
import {TransactionScope, TransactionType} from "../../models/transaction";
import {Subscription} from 'rxjs';


@Component({
  selector: 'app-command-bar',
  templateUrl: './command-bar.component.html',
  styleUrls: ['./command-bar.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CommandBarComponent implements OnInit, OnDestroy {
  period: Date;

  filterScope: {label:string, value:TransactionScope}[];
  filterType: {label:string, value:TransactionType}[];
  tableType: {label:string, value:TableType}[];

  _selectedScope: TransactionScope;
  _selectedType: TransactionType;
  _selectedTableType: TableType;

  accountSelectedSubscription: Subscription;

  set selectedType(selectedType: TransactionType) {
    this._selectedType = selectedType;
    this.accountControlService.changeTransactionType(this._selectedType);
  }
  get selectedType(): TransactionType {
    return this._selectedType;
  }

  set selectedScope(selectedScope: TransactionScope) {
    this._selectedScope = selectedScope;
    this.accountControlService.changeTransactionScope(this._selectedScope);
  }
  get selectedScope(): TransactionScope {
    return this._selectedScope;
  }

  set selectedTableType(tableType: TableType) {
    this._selectedTableType = tableType;
    this.accountControlService.changeTableType(this._selectedTableType);
  }
  get selectedTableType(): TableType {
    return this._selectedTableType;
  }

  constructor(private accountControlService: AccountControlService) {
    this.period = accountControlService.getViewPeriod();
    this.selectedType = accountControlService.getTransactionType();
    this.selectedScope = accountControlService.getTransactionScope();
    this.selectedTableType = accountControlService.getTableType();

    this.filterScope = [
      {label:'Split', value:TransactionScope.Split},
      {label:'Combine', value:TransactionScope.Combine}
    ];

    this.filterType = [
      {label:'Income', value:TransactionType.Income},
      {label:'All', value:TransactionType.None},
      {label:'Expense', value:TransactionType.Expense}
    ];

    this.tableType = [
      {label:'Flat', value: TableType.Flat},
      {label:'Combine', value:TableType.Combine}
    ];
  }

  ngOnInit() {
    this.accountSelectedSubscription = this.accountControlService.selectedAccountChanged$.subscribe(selectedAccount => {
      this.selectedScope = TransactionScope.Split;
    });
  }

  ngOnDestroy() {
    this.accountSelectedSubscription.unsubscribe();
  }

  prevMonth(){
    if (this.period.getMonth() == 0) {
      this.period.setFullYear(this.period.getFullYear() - 1);
      this.period.setMonth(11);
    } else {
      this.period.setMonth(this.period.getMonth() - 1);
    }

    this.accountControlService.changeViewPeriod(new Date(this.period));
  }

  nextMonth(){
    if (this.period.getMonth() == 11) {
      this.period.setFullYear(this.period.getFullYear() + 1);
      this.period.setMonth(0);
    } else {
      this.period.setMonth(this.period.getMonth() + 1);
    }

    this.accountControlService.changeViewPeriod(new Date(this.period));
  }
}
