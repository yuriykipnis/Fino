import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UtcToLocalTimePipe } from './utc-to-local-time.pipe';
import * as fromPrimeNg from 'primeng/primeng';
import { StoreModule } from '@ngrx/store';
import { metaReducers, reducers } from "./store/reducers";

@NgModule({
  declarations: [
    UtcToLocalTimePipe,
  ],
  exports: [
    UtcToLocalTimePipe,
    fromPrimeNg.TreeTableModule,
    fromPrimeNg.DataTableModule,
    fromPrimeNg.TabViewModule,
    fromPrimeNg.DropdownModule,
    fromPrimeNg.PanelModule,
    fromPrimeNg.ButtonModule,
    fromPrimeNg.TabMenuModule,
    fromPrimeNg.DataGridModule,
    fromPrimeNg.SharedModule,
    fromPrimeNg.RadioButtonModule,
    fromPrimeNg.PanelMenuModule,
    fromPrimeNg.AccordionModule,
    fromPrimeNg.CardModule,
    fromPrimeNg.ChartModule,
    fromPrimeNg.ProgressSpinnerModule,
    fromPrimeNg.SelectButtonModule,
    fromPrimeNg.ProgressBarModule,
    fromPrimeNg.MessagesModule,
    fromPrimeNg.MessageModule,

    fromPrimeNg.InputTextareaModule,
    fromPrimeNg.InputTextModule,
    fromPrimeNg.CheckboxModule,
    fromPrimeNg.ButtonModule,
    fromPrimeNg.CodeHighlighterModule,
  ],
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    fromPrimeNg.TreeTableModule,
    fromPrimeNg.DataTableModule,
    fromPrimeNg.TabViewModule,
    fromPrimeNg.DropdownModule,
    fromPrimeNg.PanelModule,
    fromPrimeNg.ButtonModule,
    fromPrimeNg.TabMenuModule,
    fromPrimeNg.DataGridModule,
    fromPrimeNg.SharedModule,
    fromPrimeNg.RadioButtonModule,
    fromPrimeNg.PanelMenuModule,
    fromPrimeNg.AccordionModule,
    fromPrimeNg.CardModule,
    fromPrimeNg.ChartModule,
    fromPrimeNg.SelectButtonModule,
    fromPrimeNg.ProgressBarModule,
    fromPrimeNg.MessagesModule,
    fromPrimeNg.MessageModule,

    fromPrimeNg.InputTextareaModule,
    fromPrimeNg.InputTextModule,
    fromPrimeNg.CheckboxModule,
    fromPrimeNg.ButtonModule,
    fromPrimeNg.CodeHighlighterModule,
    StoreModule.forRoot(reducers, {metaReducers})
  ]
})
export class SharedModule {
  static forRoot() {
    return {
      ngModule: SharedModule,
      providers: []
    };
  }
}
