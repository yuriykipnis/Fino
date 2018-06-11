import {CreatingAccount} from "./creating-account";
import {Transaction} from "../../models/transaction";

export class CreditAccount implements CreatingAccount  {
  Id: string;
  Label: string;

  Name: string;
  Club: string;
  UserName: string;
  CardNumber: string;
  ExpirationDate: string;
  BankAccount: string;
  BankName: string;
  ProviderName: string;
  Transactions: Transaction[];
  IsActive: boolean;
  LastUpdate: string;

  public constructor(init?:Partial<CreditAccount>) {
    Object.assign(this, init);
  }
}

// export const CREDIT_LIGHT_ACCOUNTS: CreditLightAccount[] = [
//   { Id: "02322343-2345-2345-233432344432",
//     UserId: "00000000-0000-0000-000000000000",
//     Label: "Gold Card",
//     CardNumber: "2344-2343-2342-4323",
//     ExpirationDate:"",
//     ProviderName:"AMEX",
//     IsActive: true },
//
//   { Id: "12322343-2345-2345-233432344432",
//     UserId: "00000000-0000-0000-000000000000",
//     Label: "Gold Card",
//     CardNumber: "2344-5435-9805-3994",
//     ExpirationDate:"",
//     ProviderName:"AMEX",
//     IsActive: true },
//
//   { Id: "22322343-2345-2345-233432344432",
//     UserId: "00000000-0000-0000-000000000000",
//     Label: "Gold Card",
//     CardNumber: "3433-3234-2343",
//     ExpirationDate:"",
//     ProviderName:"Visa",
//     IsActive: true }
// ]
