import {Injectable} from '@angular/core';
import {Subject} from 'rxjs';
import {LoanViewModel} from "../models/loan-view.model";

@Injectable()
export class LoanControlService {
  // Observable string sources
  private selectedLoanSource = new Subject<LoanViewModel>();
  private selectedLoan: LoanViewModel;

  // Observable string streams
  selectedLoanChanged$ = this.selectedLoanSource.asObservable();

  constructor(){
    this.selectedLoan = null;
  }

  // Service message commands
  changeSelectedLoan(selectedLoan: LoanViewModel) {
    if (this.selectedLoan !== selectedLoan){
      this.selectedLoan = selectedLoan;
      this.selectedLoanSource.next(selectedLoan);
    }
  }

  getSelectedLoan(): LoanViewModel{
    return this.selectedLoan;
  }

}
