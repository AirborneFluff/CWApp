import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Part } from '../_models/part';

@Injectable({
  providedIn: 'root'
})
export class PartsService {
  baseUrl = environment.baseUrl;
  parts: Part[] = [];

  constructor(private http: HttpClient) { }

  getParts() {
    return this.http.get<Part[]>(this.baseUrl + "parts");
  }

  getPart(partId: string) {
    return this.http.get<Part>(this.baseUrl + "parts/" + partId);
  }
}
