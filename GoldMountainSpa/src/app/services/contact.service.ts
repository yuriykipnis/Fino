import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';
import { RequestOptions, RequestMethod, Headers, ResponseContentType } from '@angular/http';
import {environment} from "../../environments/environment";
import {ContactMessage} from "../models/contactMessage";

@Injectable()
export class ContactService {

  constructor(private http: HttpClient) {  }

  sendMessage$(userId : string, message: ContactMessage) : Observable<any> {
    let url = environment.api.clientApiUrl + '/contact/' + userId + '/send';
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    return this.http.post(url, message, {headers: headers}).map((res: any) => {

      return res;
    });
  }
}
