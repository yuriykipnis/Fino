import {BankAccount} from "../accounts/models/bank-account";

export interface Industry{
  Name: string;
  Id: number;
  Description: string;
  CreatedOn: string;
  CreatedBy: string;
  Accounts: BankAccount[];
}

export const INDUSTRIES: Industry[] = [
  { Name: "Banks", Id: 0, Description: "blah blah blah", CreatedOn: "", CreatedBy: "", Accounts: [] },
  { Name: "Credit", Id: 1, Description: "blah blah", CreatedOn: "", CreatedBy: "", Accounts: [] },
  { Name: "Insurance", Id: 2, Description: "blah", CreatedOn: "", CreatedBy: "", Accounts: [] }
]
