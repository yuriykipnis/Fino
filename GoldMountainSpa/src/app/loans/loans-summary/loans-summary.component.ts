import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Mortgage } from "../../models/mortgage";
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-loans-summary',
  templateUrl: './loans-summary.component.html',
  styleUrls: ['./loans-summary.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoansSummaryComponent implements OnInit {
  loan: Mortgage;

  constructor() { }

  ngOnInit() {
    this.loan = new Mortgage({
      Id: "6248290690326",
      StartDate: "2015-04-25T21:26:00.000Z",
      EndDate: "2045-06-09T21:10:00.000Z",
      OriginalAmount: 406000.0,
      DeptAmount: 366550.24,
      PrepaymentCommission: 426.53,
      InsuranceCompany: "כלל חברה לביטוח בעמ",
      InterestType: "פריים",
      IndexType: "לא צמוד",
    });
  }
}
