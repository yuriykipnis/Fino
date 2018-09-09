import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Loan } from "../../models/loan";
import { Observable } from 'rxjs/Observable';
import {SubLoan} from "../../models/subLoan";

@Component({
  selector: 'app-loans-summary',
  templateUrl: './loans-summary.component.html',
  styleUrls: ['./loans-summary.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoansSummaryComponent implements OnInit {
  loan: Loan;

  constructor() { }

  ngOnInit() {
    this.loan = new Loan({
      Id: "6248290690326",
      StartDate: "2015-04-25T21:26:00.000Z",
      EndDate: "2045-06-09T21:10:00.000Z",
      OriginalAmount: 406000.0,
      DeptAmount: 366550.24,
      LastPaymentAmount: 1250.68,
      PrepaymentCommission: 426.53,
      InsuranceCompany: "כלל חברה לביטוח בעמ",
      InterestType: "פריים",
      LinkageType: "לא צמוד",
      SubLoans: []
    });

    this.loan.SubLoans.push(new SubLoan({
      OriginalAmount: 206000.0,
      PrincipalAmount: 185713.9,
      InterestAmount: 10.68,
      NextExitDate: "2018-09-09T21:10:00.000Z",
      StartDate: "2015-04-25T21:26:00.000Z",
      EndDate: "2045-05-09T21:10:00.000Z",
      InterestRate : 0.7
    }));
  }
}
