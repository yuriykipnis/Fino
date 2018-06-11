import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Institution } from "../models/institution";
import 'rxjs/add/operator/map';
import { RequestOptions, RequestMethod, Headers, ResponseContentType } from '@angular/http';

@Injectable()
export class InstitutionService {

  dataProviderUrl: String = 'http://localhost:5002/api';

  constructor(private http: HttpClient) {  }

  getInstitutions$(): Observable<Institution[]> {
    var response = this.http.get<Institution[]>(this.dataProviderUrl + '/institution').map((res: any[]) => {
      let result = new Array<Institution>();
      res.forEach(r => result.push(
        new Institution({
          IsSupported: r.isSupported,
          Name: r.name,
          Credentials: r.credentials,
          Type: r.type
        })));
      return result;
    });

    return response;
  }

}
