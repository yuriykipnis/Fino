import {Injectable} from "@angular/core";
import {environment} from "../../environments/environment";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs/internal/Observable";
import {InstitutionOverview, LoanOverview, Overview} from "../models/overview";

@Injectable()
export class OverviewService {
  constructor(private http: HttpClient) {  }

  getOverview$(userId : string): Observable<Overview> {
    let url = environment.api.clientApiUrl + '/user/' + userId + '/overview';
    let headers = new HttpHeaders()
      .set('Authorization', 'Bearer ' + localStorage.getItem('access_token'));

    var response = this.http.get<Overview[]>(url, {headers: headers}).map((res: any) => {
      let result = new Overview({
        NetWorth: res.netWorth,
        TotalMortgage: res.totalMortgage,
        InstitutionOverviews: [],
        MortgageOverview: new LoanOverview({
          Principal: res.mortgageOverview.principal,
          Interest: res.mortgageOverview.interest
        }),
        LoanOverview: new LoanOverview({
          Principal: res.loanOverview.principal,
          Interest: res.loanOverview.interest
        }),
        NetWorthExpenses: res.netWorthExpenses,
        NetWorthIncomes:  res.netWorthIncomes,
        CashFlowExpenses: res.cashFlowExpenses,
        CashFlowIncomes: res.cashFlowIncomes,
        NumberOfMortgages: res.numberOfMortgages,
        NumberOfLoans: res.numberOfLoans,
        Loans: res.loans,
        Mortgages: res.mortgages
      });

      res.listOfInstitutions.forEach(ins => {
        result.InstitutionOverviews.push(new InstitutionOverview({
          Label: ins.label,
          Balance: ins.balance,
          ProviderName: ins.providerName
        }));
      });



      return result;
    });

    return response;
  }

}
