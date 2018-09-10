import {Component, Input, OnDestroy, OnInit, ViewEncapsulation} from '@angular/core';
import {ActivatedRoute, ParamMap, Router} from '@angular/router';
import {Transaction, TransactionScope, TransactionType} from "../../models/transaction";
import {BankService} from "../../services/bank.service";
import {CreditService} from "../../services/credit.service";
import {from, Observable, Subscription} from 'rxjs';
import 'rxjs/add/operator/filter';
import {AccountControlService, TableType} from "../services/account-control.service";
import {AccountIdentifier, AccountType} from "../models/account-identifier";
import {TreeNode} from 'primeng/components/common/api';
import {AccountsSummaryService} from "../services/accounts-summary.service";
import {UserProfileService} from "../../services/user-profile.service";
import {UserProfile} from "../../models/user.profile";

@Component({
  selector: 'app-transactions-view',
  templateUrl: './transactions-view.component.html',
  styleUrls: ['./transactions-view.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TransactionsViewComponent implements OnInit, OnDestroy {
  transactions$: Observable<Transaction[]>;
  transactions: Transaction[];
  transactionsTree: TreeNode[];

  selectedAccount: AccountIdentifier;
  period: Date;
  transactionType: TransactionType;
  transactionScope: TransactionScope;
  tableType: TableType;
  userProfile: UserProfile;
  isLoading: boolean;

  private userProfileSubscription: Subscription;
  private viewPeriodSubscription: Subscription;
  private transactionTypeSubscription: Subscription;
  private tableTypeSubscription: Subscription;
  private transactionScopeSubscription: Subscription;
  private selectedAccountIdSubscription: Subscription;
  private loadingStateSubscription: Subscription;

  treeTableCols: { field: string, header: string }[];


  constructor(private router: Router, public route: ActivatedRoute,
              private accountControlService: AccountControlService,
              private accountsSummaryService: AccountsSummaryService,
              private bankService: BankService,
              private creditService: CreditService,
              private userProfileService: UserProfileService) {
    this.transactionType = accountControlService.getTransactionType();
    this.selectedAccount = accountControlService.getSelectedAccount();
    this.tableType = accountControlService.getTableType();
    this.transactionScope = accountControlService.getTransactionScope();
    this.period = accountControlService.getViewPeriod();
    this.period.toDateString();
    this.transactions = new Array<Transaction>();
    this.isLoading = accountControlService.getIsLoading();

    this.treeTableCols = [
      { field: 'PaymentDate', header: 'PaymentDate' },
      { field: 'Description', header: 'Description' },
      { field: 'Amount', header: 'Income' },
      { field: 'Amount', header: 'Expense' }
    ];
  }

  ngOnInit() {
    this.updateTransactions(this.selectedAccount);

    this.viewPeriodSubscription = this.accountControlService.viewPeriodChanged$.subscribe(
      period => {
        this.period = period;
        if (this.transactionScope === TransactionScope.Split){
          this.updateTransactions(this.selectedAccount);
        } else {
          this.updateTransactions(null);
        }
      });

    this.transactionTypeSubscription = this.accountControlService.transactionTypeChanged$.subscribe(
      transactionType => {
        this.transactionType = transactionType;
        if (this.transactionScope === TransactionScope.Split){
          this.updateTransactions(this.selectedAccount);
        } else {
          this.updateTransactions(null);
        }
      })

    this.transactionScopeSubscription = this.accountControlService.transactionScopeChanged$.subscribe(
      transactionScope => {
        this.transactionScope = transactionScope;
        if (transactionScope === TransactionScope.Split){
          this.updateTransactions(this.selectedAccount);
        } else {
          this.updateTransactions(null);
        }
      });

    this.selectedAccountIdSubscription = this.accountControlService.selectedAccountChanged$.subscribe(
      selectedAccount => {
        this.selectedAccount = selectedAccount;
        this.updateTransactions(this.selectedAccount);
      });

    this.tableTypeSubscription = this.accountControlService.tableTypeChanged$.subscribe(
      selectedTableType => {
        this.tableType = selectedTableType;
      });

    this.userProfileSubscription = this.userProfileService.userProfile$.subscribe(up => {
      this.userProfile = up;
      this.transactions.splice(0, this.transactions.length);
    });

    this.loadingStateSubscription = this.accountControlService.isLoadingChanged$.subscribe(isLoading =>{
      this.isLoading = isLoading;
    })
  }

  ngOnDestroy() {
    this.viewPeriodSubscription.unsubscribe();
    this.transactionTypeSubscription.unsubscribe();
    this.tableTypeSubscription.unsubscribe();
    this.transactionScopeSubscription.unsubscribe();
    this.selectedAccountIdSubscription.unsubscribe();
    this.userProfileSubscription.unsubscribe();
    this.loadingStateSubscription.unsubscribe();
  }

  private retrieveAllTransactions$() : Observable<Transaction[]>{
    if (!this.userProfile || !this.userProfile.Id) {
      return new Observable<Transaction[]>();
    }
    return this.bankService.getAllTransactions$(this.userProfile.Id, this.period);
  }

  private retrieveTransactions$(account: AccountIdentifier) : Observable<Transaction[]>{
    if (account.Type === AccountType.Bank){
      return this.bankService.getTransactions$(account.Id, this.period);
    }
    else if (account.Type === AccountType.Credit){
      return this.creditService.getTransactions$(account.Id, this.period);
    }

    return new Observable<Transaction[]>();
  }

  private updateTransactions(account: AccountIdentifier){

    if (account){
      this.retrieveTransactions$(account)
        .map(trs => {
          if (this.transactionType !== TransactionType.None) {
            return trs.filter(t => t.Type === this.transactionType)
          } else {
            return trs;
          }
        }).subscribe(trs => {
          this.populateFlatTransactions(trs);
          this.populateTreeTransactions(trs);
        });
    } else { //combined view
      this.retrieveAllTransactions$().map(trs => {
        if (this.transactionType !== TransactionType.None) {
          return trs.filter(t => t.Type === this.transactionType)
        } else {
          return trs;
        }
      }).subscribe(trs => {
        this.populateFlatTransactions(trs);
        this.populateTreeTransactions(trs);
      });
    }
  }

  private populateFlatTransactions(transactions: Transaction[]){
    this.transactions.splice(0, this.transactions.length);
    transactions.forEach(t => this.transactions.push(new Transaction(t)));
  }

  private populateTreeTransactions(transactions: Transaction[]){
    let transactionsTree = new Array<TreeNode>();
    let parents = new Array<{description: string, node: TreeNode, children: number}>();
    transactions.forEach(t => {
      let newNode = {data: new Transaction(t)};
      let parent = parents.find(p=> p.description === t.Description );
      if (parent){
        if (!parent.node.children)
        {
          parent.node.children = new Array<TreeNode>();
          parent.node.children.push({data: new Transaction(parent.node.data)});
        }
        parent.node.children.push(newNode);

        if (parent.node.data.Type === newNode.data.Type)
        {
          parent.node.data.Amount += newNode.data.Amount;
        } else {
          parent.node.data.Amount -= newNode.data.Amount;
        }

        parent.children += 1;
        parent.node.data.PaymentDate = parent.children + ' transactions';
      }
      else{
        transactionsTree.push(newNode);
        parents.push({description: t.Description, node: newNode, children: 1});
      }
    });

    this.transactionsTree = transactionsTree;
  }

  getTransactionColor(type : TransactionType) {
    return type === TransactionType.Income ? "#8fac67" : "#d22a77";
  }

  getBalanceColor(balance : number)  {
    if (balance === NaN){
      return "#8fac67";
    }
    return balance >= 0 ? "#8fac67" : "#d22a77";
  }

  isFlatTable() : boolean{
    return this.tableType === TableType.Flat;
  }

  isSplitScope() : boolean{
    return this.transactionScope === TransactionScope.Split;
  }

  isIncomeTransaction(transactionType : TransactionType) : boolean{
    return transactionType == TransactionType.Income;
  }

  isBankAccount() : boolean {
    if (!this.selectedAccount){
      return false;
    }

    return this.selectedAccount.Type == AccountType.Bank;
  }

  isIncomeTransactionsSelected() : boolean{
    return this.transactionType == TransactionType.Income;
  }

  isDate(text: string): boolean {
    return (new Date(text)).toString() !== 'Invalid Date';
  }

  isNumber(text: string): boolean{
    return text !== "NaN";
  }

  isCombineTable() : boolean{
    return this.transactionScope == TransactionScope.Combine;
  }

  getTotalIncome() : number{
    let income = 0;
    this.transactions.forEach(t => {
      income += t.Type === TransactionType.Income ? t.Amount : 0;
    })

    return income;
  }

  getTotalExpense() : number{
    let income = 0;
    this.transactions.forEach(t => {
      income += t.Type === TransactionType.Expense ? t.Amount : 0;
    })

    return income;
  }
}
