import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { SeInsurProfile } from "../models/se-insur.profile";
import {PolicyStatus} from "../models/basic-insur.profile";
import {environment} from "../../../environments/environment";

@Injectable()
export class SeInsurService {

  constructor(private http: HttpClient) { }

  getAccounts$(userId : string, passportId: string): Observable<SeInsurProfile[]> {
    let url = environment.api.clientApiUrl + '/user/' + passportId + '/SeInsurAccounts';
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    var response = this.http.get<SeInsurProfile[]>(url, {headers: headers}).map((res: any[]) => {
      let result = new Array<SeInsurProfile>();
      res.forEach(ef => result.push(
        new SeInsurProfile({
          Id: ef.id,
          ProviderName: ef.providerName,
          EmployerName: ef.employerName,
          PlanName: ef.planName,
          PolicyId: ef.policyId,
          PolicyStatus: ef.policyStatus as PolicyStatus,
          TotalSavings: ef.totalSavings,
          ExpectedRetirementSavingsNoPremium: ef.expectedRetirementSavingsNoPremium,
          MonthlyRetirementPensionNoPremium: ef.monthlyRetirementPensionNoPremium,
          ExpectedRetirementSavings: ef.expectedRetirementSavings,
          MonthlyRetirementPension: ef.monthlyRetirementPension,
          DepositFee: ef.depositFee,
          SavingFee: ef.savingFee,
          YearRevenue: ef.yearRevenue,
          DeathInsuranceMonthlyAmount: ef.deathInsuranceMonthlyAmount,
          DeathInsuranceAmount: ef.deathInsuranceAmount,

          PolicyOpeningDate: ef.policyOpeningDate,
          ValidationDate: ef.validationDate
        })));
      return result;
    });

    return response;
  }

  getProfile$(profileId: string): Observable<SeInsurProfile> {
    let url = environment.api.clientApiUrl + '/SeInsurAccounts/' + profileId;
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    var response = this.http.get<SeInsurProfile>(url, {headers: headers}).map((res: any) => {
      let result = new SeInsurProfile({
          Id: res.id,
          ProviderName: res.providerName,
          EmployerName: res.employerName,
          PlanName: res.planName,
          PolicyId: res.policyId,
          PolicyStatus: res.policyStatus as PolicyStatus,
          TotalSavings: res.totalSavings,
          ExpectedRetirementSavingsNoPremium: res.expectedRetirementSavingsNoPremium,
          MonthlyRetirementPensionNoPremium: res.monthlyRetirementPensionNoPremium,
          ExpectedRetirementSavings: res.expectedRetirementSavings,
          MonthlyRetirementPension: res.monthlyRetirementPension,
          DepositFee: res.depositFee,
          SavingFee: res.savingFee,
          YearRevenue: res.yearRevenue,
          DeathInsuranceMonthlyAmount: res.deathInsuranceMonthlyAmount,
          DeathInsuranceAmount: res.deathInsuranceAmount,
          PolicyOpeningDate: res.policyOpeningDate,
          ValidationDate: res.validationDate
        });
      return result;
    });

    return response;
  }
}
