import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {PolicyStatus} from "../models/basic-insur.profile";
import {PensionFundProfile} from "../models/pension-fund.profile";

@Injectable()
export class PensionFundService {

  clientApiUrl: String = 'http://localhost:5001/api';

  constructor(private http: HttpClient) { }

  getAccounts$(userId : string, passportId: string): Observable<PensionFundProfile[]> {
    let url = this.clientApiUrl + '/user/' + passportId + '/PensionFundAccounts';
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    var response = this.http.get<PensionFundProfile[]>(url, {headers: headers}).map((res: any[]) => {
      let result = new Array<PensionFundProfile>();
      res.forEach(p => result.push(
        new PensionFundProfile({
          Id: p.id,
          ProviderName: p.providerName,
          EmployerName: p.employerName,
          PlanName: p.planName,
          PolicyId: p.policyId,
          PolicyStatus: p.policyStatus as PolicyStatus,
          TotalSavings: p.totalSavings,
          ExpectedRetirementSavingsNoPremium: p.expectedRetirementSavingsNoPremium,
          MonthlyRetirementPensionNoPremium: p.monthlyRetirementPensionNoPremium,
          ExpectedRetirementSavings: p.expectedRetirementSavings,
          MonthlyRetirementPension: p.monthlyRetirementPension,
          DepositFee: p.depositFee,
          SavingFee: p.savingFee,
          YearRevenue: p.yearRevenue,
          SaverDeposit: p.saverDeposit,
          EmployerDeposit: p.employerDeposit,
          PartnerSurvivors: p.partnerSurvivors,
          ChildrenSurvivors: p.childrenSurvivors,
          ParentSurvivors: p.parentSurvivors,
          InvalidPension: p.invalidPension,
          WorkDisabilityMonthly: p.workDisabilityMonthly,
          WorkDisabilityOneTime: p.WorkDisabilityOneTime,

          PolicyOpeningDate: p.policyOpeningDate,
          ValidationDate: p.validationDate
        })));
      return result;
    });

    return response;
  }

  getProfile$(profileId: string): Observable<PensionFundProfile> {
    let url = this.clientApiUrl + '/PensionFundAccounts/' + profileId;
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    var response = this.http.get<PensionFundProfile>(url, {headers: headers}).map((res: any) => {
      let result = new PensionFundProfile({
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
          SaverDeposit: res.saverDeposit,
          EmployerDeposit: res.employerDeposit,
          PartnerSurvivors: res.partnerSurvivors,
          ChildrenSurvivors: res.childrenSurvivors,
          ParentSurvivors: res.parentSurvivors,
          InvalidPension: res.invalidPension,
          WorkDisabilityMonthly: res.workDisabilityMonthly,
          WorkDisabilityOneTime: res.WorkDisabilityOneTime,

          PolicyOpeningDate: res.policyOpeningDate,
          ValidationDate: res.validationDate
        });
      return result;
    });

    return response;
  }

}
