import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {PolicyStatus} from "../models/basic-insur.profile";
import {StudyFundProfile} from "../models/study-fund.profile";

@Injectable()
export class StudyFundService {

  clientApiUrl: String = 'http://localhost:5001/api';

  constructor(private http: HttpClient) { }

  getAccounts$(userId : string, passportId: string): Observable<StudyFundProfile[]> {
    let url = this.clientApiUrl + '/user/' + passportId + '/StudyFundAccounts';
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    var response = this.http.get<StudyFundProfile[]>(url, {headers: headers}).map((res: any[]) => {
      let result = new Array<StudyFundProfile>();
      res.forEach(ef => result.push(
        new StudyFundProfile({
          Id: ef.id,
          ProviderName: ef.providerName,
          EmployerName: ef.employerName,
          PlanName: ef.planName,
          PolicyId: ef.policyId,
          PolicyStatus: ef.policyStatus as PolicyStatus,
          TotalSavings: ef.totalSavings,
          WithdrawalDate: ef.withdrawalDate,
          DepositFee: ef.depositFee,
          SavingFee: ef.savingFee,
          YearRevenue: ef.yearRevenue,
          SaverDeposit: ef.saverDeposit,
          EmployerDeposit: ef.employerDeposit,
          PolicyOpeningDate: ef.policyOpeningDate,
          ValidationDate: ef.validationDate
      })));
      return result;
    });

    return response;
  }

  getProfile$(profileId : string): Observable<StudyFundProfile> {
    let url = this.clientApiUrl + '/StudyFundAccounts/' + profileId;
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    var response = this.http.get<StudyFundProfile>(url, {headers: headers}).map((res: any) => {
      let result = new StudyFundProfile({
          Id: res.id,
          ProviderName: res.providerName,
          EmployerName: res.employerName,
          PlanName: res.planName,
          PolicyId: res.policyId,
          PolicyStatus: res.policyStatus as PolicyStatus,
          TotalSavings: res.totalSavings,
          WithdrawalDate: res.withdrawalDate,
          DepositFee: res.depositFee,
          SavingFee: res.savingFee,
          YearRevenue: res.yearRevenue,
          SaverDeposit: res.saverDeposit,
          EmployerDeposit: res.employerDeposit,
          PolicyOpeningDate: res.policyOpeningDate,
          ValidationDate: res.validationDate
        });
      return result;
    });

    return response;
  }
}
