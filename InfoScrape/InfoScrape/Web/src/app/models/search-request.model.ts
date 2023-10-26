import { SearchEngine } from 'src/app/enums/search-engine';

export class SearchRequest {
  public engine: SearchEngine = 0;
  public search: string = '';
  public targetUrl?: string;
}
