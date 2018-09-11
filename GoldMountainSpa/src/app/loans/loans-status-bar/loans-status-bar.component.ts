import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-loans-status-bar',
  templateUrl: './loans-status-bar.component.html',
  styleUrls: ['./loans-status-bar.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoansStatusBarComponent implements OnInit {
  recomendedLoanAmortisation: any;

  constructor() { }

  ngOnInit() {
    this.updateLoanAmortisation();
  }

  updateLoanAmortisation(){
    this.recomendedLoanAmortisation = {
      labels: [2018, 2019, 2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027],
      datasets: [
        {
          label: 'Interest',
          backgroundColor: '#0066cc',
          borderColor: '#0066cc',
          data: [
            63, 55, 47, 39, 31, 23, 15, 7, 3, 1
          ]
        },
        {
          label: 'Principal',
          backgroundColor: '#99ccff',
          borderColor: '#99ccff',
          data: [
            30, 37, 44, 51, 58, 65, 72, 79, 86, 93
          ]
        }
      ]
    }
  }
}
