import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';
import { RequestOptions, RequestMethod, Headers, ResponseContentType } from '@angular/http';
import {environment} from "../../../environments/environment";
import {Mortgage} from '../../models/mortgage';

//!!!Not in Use!!!

@Injectable()
export class LoanService {

  constructor(private http: HttpClient) {  }

  getLoan$(userId : string, loanId : string): Observable<Mortgage> {
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    let url = environment.api.clientApiUrl + '/loans/' + userId + '/' + loanId;
    var response = this.http.get<Mortgage>(url, {headers: headers}).map((l: any) => {
      let result = new Mortgage({
        Id: l.loanId,
        StartDate: l.startDate,
        EndDate: l.endDate,
        NextPaymentDate: l.nextPaymentDate,
        OriginalAmount: l.originalAmount,
        DeptAmount: l.deptAmount,
        PrepaymentCommission: l.prepaymentCommission,
        InterestType: l.interestType,
        LinkageType: l.linkageType,
        InsuranceCompany: l.insuranceCompany,
      });

      // l.subLoans.forEach(sl => {
      //   let newSubLoan = new SubLoan({
      //     Id: sl.id,
      //     OriginalAmount: sl.originalAmount,
      //     PrincipalAmount: sl.principalAmount,
      //     InterestAmount: sl.interestAmount,
      //     DeptAmount: sl.deptAmount,
      //     NextExitDate: sl.nextExitDate,
      //     StartDate: sl.startDate,
      //     EndDate: sl.endDate,
      //     InterestRate: sl.interestRate,
      //   });
      //   result.SubLoans.push(newSubLoan);
      // });

      return result;
    });

    return response;
  }
}
