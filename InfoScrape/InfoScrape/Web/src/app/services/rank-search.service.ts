import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SearchRequest } from '../models/search-request.model';

@Injectable({
  providedIn: 'root'
})
export class RankSearchService {

  constructor(private http: HttpClient) { }

  post(request: SearchRequest): any {
    return this.http.post<number[]>('https://localhost:7096/Search/Rank', request);
  }


}
