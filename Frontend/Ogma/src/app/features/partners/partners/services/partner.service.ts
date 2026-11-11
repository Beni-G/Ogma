import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Partner, CreatePartnerRequest, UpdatePartnerRequest } from '../models/partner.models';

@Injectable({
  providedIn: 'root'
})
export class PartnerService {
  private http = inject(HttpClient);
  private apiUrl = '/api/Partner';

  getAll(): Observable<Partner[]> {
    return this.http.get<Partner[]>(this.apiUrl);
  }

  getById(id: number): Observable<Partner> {
    return this.http.get<Partner>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreatePartnerRequest): Observable<Partner> {
    return this.http.post<Partner>(this.apiUrl, dto);
  }

  update(id: number, dto: UpdatePartnerRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}