import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { SearchRequest } from 'src/app/models/search-request.model';
import { EventBusService } from 'src/app/services/event-bus.service';
import { RankSearchService } from 'src/app/services/rank-search.service';
import { SearchEngine } from 'src/app/enums/search-engine';
import { CompetitorSearchService } from 'src/app/services/competitor-search.service';
import { SearchResult } from 'src/app/models/search-result.model';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css']
})
export class SearchBarComponent implements OnInit {
  rankSub: Subscription = new Subscription();
  competitorSub: Subscription = new Subscription();
  searchInput: string = '';
  infoTrackUrl: string = 'https://www.infotrack.co.uk';
  targetUrlInput: string = this.infoTrackUrl;
  selectedEngine: string = 'google';

  constructor(private eventBus: EventBusService, 
    private rankSearchService: RankSearchService, 
    private competitorSearchService: CompetitorSearchService) { }

  ngOnInit(): void {

  }

  onInputChange(event: any): void {
    this.eventBus.updateTargetUrl(event.target.value);
  }

  onBlurEvent(): void {
    if (this.targetUrlInput == '') {
      this.targetUrlInput = this.infoTrackUrl;
      this.eventBus.updateTargetUrl(this.infoTrackUrl);
    }
  }

  searchRankings(): void {
    let request = this.formRequest();
    if (!request)
      return;

    this.rankSub = this.rankSearchService.post(request).subscribe(
      (result: number[] | string[]) => {
        let searchResult = new SearchResult();
        searchResult.isCompetitor = false;
        searchResult.data = result;
        this.eventBus.updateSearchResults(searchResult);
      },
      (error: any) => {
        this.eventBus.updateSearchResults(error.error);
      }
    );
  }

  searchCompetitors(): void {
    let request = this.formRequest();
    if (!request)
      return;

    this.competitorSub = this.competitorSearchService.post(request).subscribe(
      (result: number[] | string[]) => {
        let searchResult = new SearchResult();
        searchResult.isCompetitor = true;
        searchResult.data = result;
        this.eventBus.updateSearchResults(searchResult);
      },
      (error: any) => {
        this.eventBus.updateSearchResults(error.error);
      }
    );
  }

  formRequest(): SearchRequest | false {  
    if (this.searchInput == '') {
      this.eventBus.updateSearchResults('Enter a search.');
      return false;
    }

    let request = new SearchRequest();
    request.engine = this.getSelectedEngineEnum();
    request.search = this.searchInput;

    if (this.targetUrlInput != this.infoTrackUrl)
      request.targetUrl = this.targetUrlInput;

    return request
  }

  getSelectedEngineEnum(): SearchEngine {
    switch (this.selectedEngine) {
      case 'google':
        return SearchEngine.Google;
      case 'duckduckgo':
        return SearchEngine.DuckDuckGo;
      default:
        throw new Error('Invalid search engine selected');
    }
  }
}
