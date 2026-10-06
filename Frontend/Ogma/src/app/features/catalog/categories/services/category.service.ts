import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Category, CreateCategoryCommand, UpdateCategoryCommand } from '../models/category.models';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/category';

  getAllFlat(): Observable<Category[]> {
    return this.http.get<Category[]>(this.baseUrl);
  }

  getAllTree(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.baseUrl}/tree`);
  }

  getById(id: number): Observable<Category> {
    return this.http.get<Category>(`${this.baseUrl}/${id}`);
  }

  create(command: CreateCategoryCommand): Observable<Category> {
    return this.http.post<Category>(this.baseUrl, command);
  }

  update(id: number, command: UpdateCategoryCommand): Observable<Category> {
    return this.http.put<Category>(`${this.baseUrl}/${id}`, command);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}