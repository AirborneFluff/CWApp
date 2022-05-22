import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { BOM } from '../_models/bom';
import { Part } from '../_models/part';
import { Product } from '../_models/product';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  baseUrl = environment.baseUrl + "products/";

  constructor(private http: HttpClient) { }

  getProducts() {
    return this.http.get<Product[]>(this.baseUrl);
  }

  getProduct(id: number) {
    return this.http.get<Product>(this.baseUrl + id);
  }

  getBom(productId: number, bomId: number) {
    return this.http.get<BOM>(this.baseUrl + productId + "/boms/" + bomId);
  }
}
