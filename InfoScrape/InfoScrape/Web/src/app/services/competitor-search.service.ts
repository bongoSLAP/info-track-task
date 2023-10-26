import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SearchRequest } from '../models/search-request.model';

@Injectable({
  providedIn: 'root'
})
export class CompetitorSearchService {

  constructor(private http: HttpClient) { }

  post(request: SearchRequest): any {
    return this.http.post<string[]>('https://localhost:7096/Search/Competitor', request);
  }
}
