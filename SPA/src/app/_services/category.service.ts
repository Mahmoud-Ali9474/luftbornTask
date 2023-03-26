import { SaveCategory } from '../_models/save-category';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
@Injectable()
export class CategoryService {
  private readonly categoryEndpoint: string =
    environment.baseUrl + 'api/categories/';

  constructor(private httpClient: HttpClient) {}

  create(category: SaveCategory) {
    return this.httpClient.post(this.categoryEndpoint, category);
  }

  getCategory(id: number) {
    return this.httpClient.get(this.categoryEndpoint + id);
  }

  getCategories(filter: any) {
    return this.httpClient.get(
      this.categoryEndpoint + '?' + this.toQueryString(filter)
    );
  }

  toQueryString(query: any) {
    var parts = [];
    for (var property in query) {
      var value = query[property];
      if (value != null && value != undefined)
        parts.push(
          encodeURIComponent(property) + '=' + encodeURIComponent(value)
        );
    }

    return parts.join('&');
  }

  update(category: SaveCategory) {
    return this.httpClient.put(this.categoryEndpoint + category.id, category);
  }

  delete(id: number) {
    return this.httpClient.delete(this.categoryEndpoint + id);
  }
}
