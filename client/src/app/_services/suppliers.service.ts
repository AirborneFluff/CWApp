import { HttpClient } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Supplier } from '../_models/supplier';

@Injectable({
  providedIn: 'root'
})
export class SuppliersService {
  baseUrl = environment.baseUrl;
  supplierNames: string[];
  nameObserverable: Observable<string[]>;

  constructor(private http: HttpClient) {
  }

  getSuppliersNames(): Observable<string[]> {
    if (this.supplierNames !== undefined) return of(this.supplierNames);
    if (this.nameObserverable !== undefined) return this.nameObserverable;

    this.nameObserverable = this.http.get<string[]>(this.baseUrl + "suppliers/names").pipe(
      map(response => {
        this.supplierNames = response;
        this.nameObserverable = undefined;
        return this.supplierNames;
      })
    )
    return this.nameObserverable
  }
}
