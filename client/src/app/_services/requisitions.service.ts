import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PageParams } from '../_models/pageParams';
import { CreateRequisition, Requisition } from '../_models/requisiton';
import { getPaginatedResult, getPaginationHeaders } from './pagination-helper.service';

@Injectable({
  providedIn: 'root'
})
export class RequisitionsService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  getPaginatedRequisitions(pageParams: PageParams) {
    let params = getPaginationHeaders(pageParams.pageNumber, pageParams.pageSize);
    
    return getPaginatedResult<Requisition[]>(this.baseUrl + "requisitions", params, this.http)
  }

  getOpenRequisition(partCode: string) {
    return this.http.get<Requisition>(this.baseUrl + "parts/" + partCode + "/requisitions?latest=true")
  }

  sendRequisition(requisition: CreateRequisition) {
    return this.http.post(this.baseUrl + "requisitions", requisition);
  }

  updateRequisition(requisition: CreateRequisition) {
    return this.http.put(this.baseUrl + "requisitions", requisition);
  }


}
