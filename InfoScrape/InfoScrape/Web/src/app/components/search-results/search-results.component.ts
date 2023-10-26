import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { EventBusService } from 'src/app/services/event-bus.service';
import { skip } from 'rxjs/operators';

@Component({
  selector: 'app-search-results',
  templateUrl: './search-results.component.html',
  styleUrls: ['./search-results.component.css']
})
export class SearchResultsComponent implements OnInit {
  currentTargetUrl: string = '';
  isFirstTime: boolean = true;
  results: any;
  isError: boolean = false;
  isCompetitor: boolean = false;
  isFirstCompTargetUrl: boolean = false;

  private searchResultSub: Subscription;

  constructor(private eventBus: EventBusService) { 
    this.searchResultSub = this.eventBus.searchResults.pipe(skip(1)).subscribe(receivedResults => {
      this.isFirstCompTargetUrl = false;
      this.isError = typeof receivedResults === 'string';
      this.isCompetitor = receivedResults.isCompetitor

      if (this.isCompetitor && receivedResults.data && receivedResults.data.length > 0 && receivedResults.data[0].includes(this.currentTargetUrl)) 
        this.isFirstCompTargetUrl = true;
      
      if (this.isError)
        this.results = receivedResults;
      else
        this.results = receivedResults.data;
      
      this.currentTargetUrl = this.eventBus.getCurrentTargetUrl();

      if (this.isFirstTime) 
        this.isFirstTime = false;
    });
  }

  ngOnInit(): void {
  }

  ngOnDestroy() {
    this.searchResultSub.unsubscribe();
  }
}
