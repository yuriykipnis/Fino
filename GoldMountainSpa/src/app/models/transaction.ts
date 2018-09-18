export enum TransactionType {
  Income, Expense, None
}

export enum TransactionScope {
  Split, Combine, None
}

export class Transaction {
  Id: string;
  PaymentDate: string;
  PurchaseDate: string;
  Description: string;
  ProviderName: string;
  Amount: number;
  CurrentBalance: number;
  Type: TransactionType;

  public constructor(init?:Partial<Transaction>) {
  Object.assign(this, init);
}
}


