import {Injectable} from '@angular/core';
import {Subject} from 'rxjs';
import {TransactionScope, TransactionType} from "../../models/transaction";
import {AccountIdentifier, AccountType} from "../models/account-identifier";

export enum TableType {
  Flat, Combine
}

@Injectable()
export class AccountControlService {
  // Observable string sources
  private viewPeriodSource = new Subject<Date>();
  private transactionTypeSource = new Subject<TransactionType>();
  private transactionScopeSource = new Subject<TransactionScope>();
  private tableTypeSource = new Subject<TableType>();
  private selectedAccountSource = new Subject<AccountIdentifier>();
  private isBankAccountLoadingSource = new Subject<boolean>();
  private isCreditAccountLoadingSource = new Subject<boolean>();

  private viewPeriod: Date;
  private transactionType: TransactionType;
  private transactionScope: TransactionScope;
  private tableType: TableType;
  private selectedAccount: AccountIdentifier;
  private isBankLoading: boolean;
  private isCreditLoading: boolean;

  // Observable string streams
  selectedAccountChanged$ = this.selectedAccountSource.asObservable();
  viewPeriodChanged$ = this.viewPeriodSource.asObservable();
  transactionTypeChanged$ = this.transactionTypeSource.asObservable();
  tableTypeChanged$ = this.tableTypeSource.asObservable();
  transactionScopeChanged$ = this.transactionScopeSource.asObservable();
  isBankAccountLoadingChanged$ = this.isBankAccountLoadingSource.asObservable();
  isCreditAccountLoadingChanged$ = this.isCreditAccountLoadingSource.asObservable();

  constructor(){
    this.viewPeriod = new Date();
    this.transactionType = TransactionType.None;
    this.transactionScope = TransactionScope.Split;
    this.tableType = TableType.Flat;
    this.selectedAccount = null;
    this.isBankLoading = false;
    this.isCreditLoading = false;
  }

  // Service message commands
  changeViewPeriod(period: Date) {
    if (this.viewPeriod !== period) {
      this.viewPeriod = period;
      this.viewPeriodSource.next(period);
    }
  }

  changeTransactionType(transactionType: TransactionType) {
    if (this.transactionType !== transactionType) {
      this.transactionType = transactionType;
      this.transactionTypeSource.next(transactionType);
    }
  }

  changeTransactionScope(transactionScope: TransactionScope) {
    if (this.transactionScope !== transactionScope) {
      this.transactionScope = transactionScope;
      this.transactionScopeSource.next(transactionScope);
    }
  }

  changeTableType(selectedTableType: TableType) {
    if (this.tableType !== selectedTableType) {
      this.tableType = selectedTableType;
      this.tableTypeSource.next(selectedTableType);
    }
  }

  changeSelectedAccountId(selectedAccount: AccountIdentifier) {
    if (this.selectedAccount !== selectedAccount){
      this.changeTransactionScope(TransactionScope.Split);
      this.selectedAccount = selectedAccount;
      this.selectedAccountSource.next(selectedAccount);
    }
  }

  changeBankLoadingState(isLoading: boolean) {
    if (this.isBankLoading !== isLoading){
      this.isBankLoading = isLoading;
      this.isBankAccountLoadingSource.next(this.isBankLoading);
    }
  }

  changeCreditLoadingState(isLoading: boolean) {
    if (this.isCreditLoading !== isLoading){
      this.isCreditLoading = isLoading;
      this.isCreditAccountLoadingSource.next(this.isCreditLoading);
    }
  }

  getViewPeriod():Date{
    return this.viewPeriod;
  }

  getTransactionType():TransactionType{
    return this.transactionType;
  }

  getTransactionScope():TransactionScope{
    return this.transactionScope;
  }

  getTableType():TableType{
    return this.tableType;
  }

  getSelectedAccount():AccountIdentifier{
    return this.selectedAccount;
  }

  getIsBankLoading():boolean{
    return this.isBankLoading;
  }

  getIsCreditLoading():boolean{
    return this.isBankLoading;
  }
}
