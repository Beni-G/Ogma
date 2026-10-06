import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CreateItemCommand, Item, UpdateItemCommand } from '../models/item.models';
import { Observable } from 'rxjs';
import { Category } from '../../categories/models/category.models';
import { ItemType } from '../../item-types/models/item-type.models';

@Injectable({
  providedIn: 'root'
})
export class ItemService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api';

  // Items CRUD
  getAll(categoryId?: number | null): Observable<Item[]> {
    let params = new HttpParams();
    if (categoryId != null) {
      params = params.set('categoryId', categoryId.toString());
    }
    return this.http.get<Item[]>(`${this.baseUrl}/Item`, { params });
  }

  getById(id: number): Observable<Item> {
    return this.http.get<Item>(`${this.baseUrl}/Item/${id}`);
  }

  create(command: CreateItemCommand): Observable<Item> {
    return this.http.post<Item>(`${this.baseUrl}/Item`, command);
  }

  update(command: UpdateItemCommand): Observable<Item> {
    return this.http.put<Item>(`${this.baseUrl}/Item/${command.id}`, command);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/Item/${id}`);
  }

  // Lookups using existing interfaces
  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.baseUrl}/Category`);
  }

  getItemTypes(): Observable<ItemType[]> {
    return this.http.get<ItemType[]>(`${this.baseUrl}/ItemType`);
  }
}