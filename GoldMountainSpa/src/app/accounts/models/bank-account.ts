import {CreatingAccount} from "./creating-account";
import {Transaction} from "../../models/transaction";
import {Loan} from "../../models/loan";


export class BankAccount implements CreatingAccount {
  Id: string;
  Label: string;
  IsActive: boolean;
  ProviderName: string;
  BankNumber: string;
  BranchNumber: string;
  AccountNumber: string;
  Balance: number;
  LastUpdate: string;
  Transactions: Transaction[];
  Loans: Loan[];

  public constructor(init?:Partial<BankAccount>) {
    Object.assign(this, init);
  }
}

// export const BANK_LIGHT_ACCOUNTS: BankLightAccount[] = [
//   { Id: "02322343-2345-2345-233432344432",
//     UserId: "00000000-0000-0000-000000000000",
//     Label: "12-365-123432",
//     ProviderName:"Bank Hapoalim",
//     BankNumber: "12",
//     BranchNumber: "365",
//     AccountNumber: "123432",
//     Balance: 0,
//     IsActive: true },
//
//   { Id: "12322343-2345-2345-233432344432",
//     UserId: "00000000-0000-0000-000000000000",
//     Label: "12-612-569384",
//     ProviderName:"Bank Hapoalim",
//     BankNumber: "12",
//     BranchNumber: "612",
//     AccountNumber: "569384",
//     Balance: 0,
//     IsActive: true },
//
//   { Id: "22322343-2345-2345-233432344432",
//     UserId: "00000000-0000-0000-000000000000",
//     Label: "12-612-343325",
//     ProviderName:"Bank Hapoalim",
//     BankNumber: "12",
//     BranchNumber: "612",
//     AccountNumber: "343325",
//     Balance: 0,
//     IsActive: true },
// ]

