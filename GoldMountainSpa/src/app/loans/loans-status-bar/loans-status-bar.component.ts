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
          label: 'Recommended',
          backgroundColor: '#8fac67',
          borderColor: '#8fac67',
          data: [
            1200, 1060, 920, 780,640, 500, 360, 220, 80 ,0
          ]
        },
        {
          label: 'Current',
          backgroundColor: '#d22a77',
          borderColor: '#d22a77',
          data: [
            1200, 1120, 1040, 960, 900, 840, 760, 680, 600, 540
          ]
        }
      ]
    }
  }
}
