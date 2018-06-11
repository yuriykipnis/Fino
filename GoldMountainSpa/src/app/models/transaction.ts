export enum TransactionType {
  Income, Expense
}

export class Transaction {
  Id: string;
  PaymentDate: string;
  PurchaseDate: string;
  Description: string;
  Amount: number;
  CurrentBalance: number;
  Type: TransactionType;

  public constructor(init?:Partial<Transaction>) {
  Object.assign(this, init);
}
}


