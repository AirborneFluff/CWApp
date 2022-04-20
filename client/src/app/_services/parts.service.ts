import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Pagination } from '../_models/pagination';
import { Part } from '../_models/part';
import { PartParams } from '../_models/partParams';
import { getPaginatedResult, getPaginationHeaders } from './pagination-helper.service';

@Injectable({
  providedIn: 'root'
})
export class PartsService {
  baseUrl = environment.baseUrl;
  parts: Part[] = [];

  constructor(private http: HttpClient) {}

  getParts() {
    return this.http.get<Part[]>(this.baseUrl + "parts");
  }

  getPart(partId: string) {
    return this.http.get<Part>(this.baseUrl + "parts/" + partId);
  }

  getPaginatedParts(partParams: PartParams) {
    let params = getPaginationHeaders(partParams.pageNumber, partParams.pageSize);

    if (!(!partParams.searchValue || !partParams.searchValue.trim()))
      params = params.append("searchValue", partParams.searchValue?.toString());
    
    return getPaginatedResult<Part[]>(this.baseUrl + "parts", params, this.http).pipe(map(result => {
      return result;
    }))
  }
}
