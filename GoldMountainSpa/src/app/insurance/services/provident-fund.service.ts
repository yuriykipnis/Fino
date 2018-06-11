import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {ProvidentFundProfile} from "../models/provident-fund.profile";
import {PolicyStatus} from "../models/basic-insur.profile";


@Injectable()
export class ProvidentFundService {

  clientApiUrl: String = 'http://localhost:5001/api';

  constructor(private http: HttpClient) { }

  getAccounts$(userId : string, passportId: string): Observable<ProvidentFundProfile[]> {
    let url = this.clientApiUrl + '/user/' + passportId + '/ProvidentFundAccounts';
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    var response = this.http.get<ProvidentFundProfile[]>(url, {headers: headers}).map((res: any[]) => {
      let result = new Array<ProvidentFundProfile>();
      res.forEach(la => result.push(
        new ProvidentFundProfile({
          Id: la.id,
          ProviderName: la.providerName,
          PolicyId: la.policyId,
          PolicyStatus: la.policyStatus as PolicyStatus,

          PolicyOpeningDate: la.policyOpeningDate,
          ValidationDate: la.validationDate
        })));
      return result;
    });

    return response;
  }

  getProfile$(profileId: string): Observable<ProvidentFundProfile> {
    let url = this.clientApiUrl + '/ProvidentFundAccounts/' + profileId;
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    var response = this.http.get<ProvidentFundProfile>(url, {headers: headers}).map((res: any) => {
      let result = new ProvidentFundProfile({
          Id: res.id,
          ProviderName: res.providerName,
          PolicyId: res.policyId,
          PolicyStatus: res.policyStatus as PolicyStatus,
          PolicyOpeningDate: res.policyOpeningDate,
          ValidationDate: res.validationDate
        });
      return result;
    });

    return response;
  }

}
