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

  constructor(private http: HttpClient) {}

  getParts() {
    return this.http.get<Part[]>(this.baseUrl + "parts");
  }

  getPart(partCode: string) {
    return this.http.get<Part>(this.baseUrl + "parts/" + partCode);
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
    
    return this.http.patch(this.baseUrl + `parts/${partCode}/${sourceId}/update-prices`, arr);
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
    
    return this.http.patch(this.baseUrl + `parts/${partCode}/${source.id}`, body);
  }

  addSupplySource(partCode: string, supplierName: string) {
    var body = {
      supplierName: supplierName
    }
    return this.http.patch(this.baseUrl + `parts/${partCode}/add-source?supplierName=${supplierName}`, {});
  }

  removeSupplySource(partCode: string, sourceId: number) {
    return this.http.patch(this.baseUrl + `parts/${partCode}/${sourceId}/remove-source`, {});
  }

  removeSourcePrice(partCode: string, sourceId: number, priceId: number) {
    return this.http.patch(this.baseUrl + `parts/${partCode}/${sourceId}/remove-price?priceId=${priceId}`, {});
  } 

  addSourcePrice(partCode: string, sourceId: number, unitPrice: number, quantity: number) {
    var body = {
      unitPrice: unitPrice,
      quantity: quantity
    }
    return this.http.post<SourcePrice>(this.baseUrl + `parts/${partCode}/${sourceId}/add-price`, body);
  } 
}
