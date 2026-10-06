import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateItemTypeCommand, ItemType, UpdateItemTypeCommand } from '../models/item-type.models';

@Injectable({
  providedIn: 'root'
})
export class ItemTypeService {

  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/ItemType';

  getAll(): Observable<ItemType[]> {
    return this.http.get<ItemType[]>(this.baseUrl);
  }

  getById(id: number): Observable<ItemType> {
    return this.http.get<ItemType>(`${this.baseUrl}/${id}`);
  }

  create(command: CreateItemTypeCommand): Observable<ItemType> {
    return this.http.post<ItemType>(this.baseUrl, command);
  }

  update(command: UpdateItemTypeCommand): Observable<ItemType> {
    return this.http.put<ItemType>(`${this.baseUrl}/${command.id}`, command);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
