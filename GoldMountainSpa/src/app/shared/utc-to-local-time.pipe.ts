import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'utcToLocalTime'
})
export class UtcToLocalTimePipe implements PipeTransform {

  transform(value: string): string {
    if (!value) {
      return '';
    }
    let offset = new Date().getTimezoneOffset();
    let utcTime = new Date(value);
    return (utcTime.setMinutes(utcTime.getMinutes() - offset)).toString();
  }
}
