import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {MortgageCoverage, MortgageInsurProfile} from "../models/mortgage-insur.profile";
import {PolicyStatus} from "../models/basic-insur.profile";
import {environment} from "../../../environments/environment";

@Injectable()
export class MortgageInsurService {

  constructor(private http: HttpClient) { }

  getAccounts$(userId : string, passportId: string): Observable<MortgageInsurProfile[]> {
    let url = environment.api.clientApiUrl + '/user/' + passportId + '/MortgageInsurAccounts';
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    var response = this.http.get<MortgageInsurProfile[]>(url, {headers: headers}).map((res: any[]) => {
      let result = new Array<MortgageInsurProfile>();
      res.forEach(ma => {
        let coverages = new Array<MortgageCoverage>();
        ma.coverage.forEach(c => coverages.push({
            CoverageName: c.coverageName,
            Amount: c.amount,
            DueDate: c.dueDate,
            ActualFee: c.actualFee
        }));

        result.push(new MortgageInsurProfile({
          Id: ma.id,
          ProviderName: ma.providerName,
          PlanName: ma.planName,
          PolicyId: ma.policyId,
          PolicyStatus: ma.policyStatus as PolicyStatus,
          TotalSavings: ma.totalSavings,
          DepositFee: ma.depositFee,
          SavingFee: ma.savingFee,
          WorkDisabilityMonthly:ma.workDisabilityMonthly,
          WorkDisabilityOneTime: ma.WorkDisabilityOneTime,
          Coverage: coverages,

          PolicyOpeningDate: ma.policyOpeningDate,
          ValidationDate: ma.validationDate
        }))
      });
      return result;
    });
    return response;
  }

  getProfile$(profileId: string): Observable<MortgageInsurProfile> {
    let url = environment.api.clientApiUrl + '/MortgageInsurAccounts/' + profileId;
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    var response = this.http.get<MortgageInsurProfile>(url, {headers: headers}).map((res: any) => {
      let coverages = new Array<MortgageCoverage>();
      res.coverage.forEach(c => coverages.push({
        CoverageName: c.coverageName,
        Amount: c.amount,
        DueDate: c.dueDate,
        ActualFee: c.actualFee
      }));

      let result = new MortgageInsurProfile({
          Id: res.id,
          ProviderName: res.providerName,
          PlanName: res.planName,
          PolicyId: res.policyId,
          PolicyStatus: res.policyStatus as PolicyStatus,
          TotalSavings: res.totalSavings,
          DepositFee: res.depositFee,
          SavingFee: res.savingFee,
          WorkDisabilityMonthly:res.workDisabilityMonthly,
          WorkDisabilityOneTime: res.WorkDisabilityOneTime,
          Coverage: coverages,

          PolicyOpeningDate: res.policyOpeningDate,
          ValidationDate: res.validationDate
        });
      return result;
    });

    return response;
  }
}
