import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Pagination } from '../_models/pagination';
import { Part, SourcePrice, SupplySource } from '../_models/part';
import { PartParams } from '../_models/partParams';
import { getPaginatedResult, getPaginationHeaders } from './pagination-helper.service';

@Injectable({
  providedIn: 'root'
})
export class PartsService {
  baseUrl = environment.baseUrl;
  parts: Part[] = [];
  partCodes: string[] = [];

  constructor(private http: HttpClient) {}

  getPart(partCode: string) {
    return this.http.get<Part>(this.baseUrl + "parts/" + partCode);
  }

  getAllPartcodes() {
    return this.http.get<string[]>(this.baseUrl + "parts/partcodes").pipe(map(result => {
      this.partCodes = result;
    }));
  }

  getPaginatedParts(partParams: PartParams) {
    let params = getPaginationHeaders(partParams.pageNumber, partParams.pageSize);

    if (!(!partParams.searchValue || !partParams.searchValue.trim()))
      params = params.append("searchValue", partParams.searchValue?.toString());
    
    return getPaginatedResult<Part[]>(this.baseUrl + "parts", params, this.http).pipe(map(result => {
      return result;
    }))
  }

  updatePrices(partCode: String, sourceId: Number, prices: SourcePrice[]) {
    var arr = new Array;
    prices.forEach(elem => {
      var price = {
        unitPrice: elem.unitPrice,
        quantity: elem.quantity
      };
      arr.push(price);
    });
    
    return this.http.put(this.baseUrl + `parts/${partCode}/sources/${sourceId}/prices`, arr);
  }

  updateSupplySource(partCode: String, source: SupplySource) {
    var body = {
      supplierName: source.supplier.name,
      supplierSKU: source.supplierSKU,
      manufacturerSKU: source.manufacturerSKU,
      packSize: source.packSize,
      minimumOrderQuantity: source.minimumOrderQuantity,
      notes: source.notes,
      roHS: source.roHS
    }
    
    return this.http.put(this.baseUrl + `parts/${partCode}/sources/${source.id}`, body);
  }

  addSupplySource(partCode: string, supplierName: string) {
    return this.http.post(this.baseUrl + `parts/${partCode}/sources?supplierName=${supplierName}`, {});
  }

  removeSupplySource(partCode: string, sourceId: number) {
    return this.http.delete(this.baseUrl + `parts/${partCode}/sources/${sourceId}`, {});
  }

  removeSourcePrice(partCode: string, sourceId: number, priceId: number) {
    return this.http.delete(this.baseUrl + `parts/${partCode}/sources/${sourceId}/prices/${priceId}`, {});
  } 

  addSourcePrice(partCode: string, sourceId: number, unitPrice: number, quantity: number) {
    var body = {
      unitPrice: unitPrice,
      quantity: quantity
    }
    return this.http.post<SourcePrice>(this.baseUrl + `parts/${partCode}/sources/${sourceId}/prices`, body);
  } 
}
