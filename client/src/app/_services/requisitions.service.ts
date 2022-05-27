import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PageParams } from '../_models/pageParams';
import { Requisition } from '../_models/requisiton';
import { getPaginatedResult, getPaginationHeaders } from './pagination-helper.service';

@Injectable({
  providedIn: 'root'
})
export class RequisitionsService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  getPaginatedRequisitions(pageParams: PageParams) {
    let params = getPaginationHeaders(pageParams.pageNumber, pageParams.pageSize);
    
    return getPaginatedResult<Requisition[]>(this.baseUrl + "requisitions", params, this.http).pipe(map(result => {
      return result;
    }))
  }


}
