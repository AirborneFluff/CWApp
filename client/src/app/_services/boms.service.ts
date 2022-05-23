import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { BOM } from '../_models/bom';
import { BOMEntry, NewBOMEntry } from '../_models/bomEntry';

@Injectable({
  providedIn: 'root'
})
export class BomsService {
  baseUrl = environment.baseUrl + "products/"

  constructor(private http: HttpClient) { }

  updateBomEntry(productId: number, entry: BOMEntry): Observable<BOM> {
    let body = {
      "quantity": entry.quantity,
      "componentLocation": entry.componentLocation
    }
    return this.http.put<BOM>(this.baseUrl + productId + "/boms/" + entry.bomId + "/parts/" + entry.partId, body);
  }

  addBomEntry(productId: number, bomId: number, entry: NewBOMEntry) {
    return this.http.post(this.baseUrl + productId + "/boms/" + bomId + "/parts", entry);
  }

  deleteBomEntry(productId: number, entry: BOMEntry): Observable<BOM> {
    return this.http.delete<BOM>(this.baseUrl + productId + "/boms/" + entry.bomId + "/parts/" + entry.partId);
  }
}
