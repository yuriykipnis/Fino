export enum AccountType {
  None, Bank, Credit
}
export interface AccountIdentifier {
  Id: string;
  Type: AccountType;
}
