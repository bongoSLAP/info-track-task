import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { SearchResult } from '../models/search-result.model';

@Injectable({
  providedIn: 'root'
})
export class EventBusService {

  private currentTargetUrlSource = new BehaviorSubject<string>('https://www.infotrack.co.uk');
  currentTargetUrl = this.currentTargetUrlSource.asObservable();

  private searchResultsSource = new BehaviorSubject<any>(new SearchResult());
  searchResults = this.searchResultsSource.asObservable();

  constructor() { }

  updateTargetUrl(url: string): void {
    this.currentTargetUrlSource.next(url);
  }

  getCurrentTargetUrl(): string {
    return this.currentTargetUrlSource.value;
  }

  updateSearchResults(results: any): void {
    this.searchResultsSource.next(results);
  }
}
