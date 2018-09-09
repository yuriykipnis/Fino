import { Routes } from '@angular/router';
import {AccountsComponent} from "./accounts/accounts.component";
import {PlanningComponent} from "./planning/planning.component";
import {InsuranceComponent} from "./insurance/insurance.component";
import {NewAccountComponent} from "./accounts/new-account/new-account.component";
import {AuthGuard} from "./auth/auth.guard";
import {LoginCallbackComponent} from "./auth/login-callback/login-callback.component";
import {OverviewComponent} from "./overview/overview.component";
import {TransactionsViewComponent} from "./accounts/transactions-view/transactions-view.component";
import {StudyFundViewComponent} from "./insurance/views/study-fund-view/study-fund-view.component";
import {SeInsurViewComponent} from "./insurance/views/se-insur-view/se-insur-view.component";
import {PensionFundViewComponent} from "./insurance/views/pension-fund-view/pension-fund-view.component";
import {ProvidentFundViewComponent} from "./insurance/views/provident-fund-view/provident-fund-view.component";
import {MortgageInsurViewComponent} from "./insurance/views/mortgage-insur-view/mortgage-insur-view.component";
import {SummaryComponent} from "./insurance/views/summary/summary.component";
import {LoansComponent} from "./loans/loans.component";
import {LoanViewComponent} from './loans/loan-view/loan-view.component';
import {LoansSummaryComponent} from './loans/loans-summary/loans-summary.component';

export const routes: Routes = [
  { path: 'overview', component: OverviewComponent},
  { path: 'accounts', component: AccountsComponent, canActivate: [AuthGuard],
    children: [
      {path: 'add', component: NewAccountComponent},
      {path: 'summary', component: TransactionsViewComponent}
    ]
  },
  { path: 'insurance', component: InsuranceComponent, canActivate: [AuthGuard],
    children: [
      {path: 'summary', component: SummaryComponent},
      {path: 'studyfund/:ProfileId', component: StudyFundViewComponent},
      {path: 'seinsur/:ProfileId', component: SeInsurViewComponent},
      {path: 'providentfund/:ProfileId', component: ProvidentFundViewComponent},
      {path: 'mortgageinsur/:ProfileId', component: MortgageInsurViewComponent},
      {path: 'pensionfund/:ProfileId', component: PensionFundViewComponent}
    ]
  },
  { path: 'loans', component: LoansComponent, canActivate: [AuthGuard],
    children: [
      {path: 'summary', component: LoansSummaryComponent},
      {path: ':LoanId', component: LoanViewComponent},
    ]
  },
  { path: 'planning', component: PlanningComponent, canActivate: [AuthGuard] },
  { path: 'adviser', component: PlanningComponent, canActivate: [AuthGuard] },
  { path: 'login-callback', component: LoginCallbackComponent },

  { path: '**', redirectTo: 'overview' }
];


