import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreatePartnerRoleTypeDto, PartnerRoleType, UpdatePartnerRoleTypeDto } from '../models/partner-role-type.models';

@Injectable({
  providedIn: 'root'
})
export class PartnerRoleTypeService {

  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/PartnerRoleType';

  getAll(): Observable<PartnerRoleType[]> {
    return this.http.get<PartnerRoleType[]>(this.baseUrl);
  }

  getById(id: number): Observable<PartnerRoleType> {
      return this.http.get<PartnerRoleType>(`${this.baseUrl}/${id}`);
    }

  create(dto: CreatePartnerRoleTypeDto): Observable<PartnerRoleType> {
    return this.http.post<PartnerRoleType>(this.baseUrl, dto);
  }

  update(id: number, dto: UpdatePartnerRoleTypeDto): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}