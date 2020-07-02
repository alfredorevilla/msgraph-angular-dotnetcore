import { mergeMap, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Component } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ApplicationModel } from './application.model';
import { FormControl } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-applications',
  templateUrl: './applications.component.html',
  styleUrls: ['./applications.component.scss']
})
/** applications component*/
export class ApplicationsComponent {
  applications$: BehaviorSubject<ApplicationModel[]> = new BehaviorSubject<ApplicationModel[]>([]);
  filter = new FormControl('');
  constructor(httpClient: HttpClient) {
    this.filter.valueChanges.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      mergeMap(value => httpClient.get<ApplicationModel[]>(`/api/application?searchText=${value}`)))
      .subscribe(result => this.applications$.next(result));
  }
}
