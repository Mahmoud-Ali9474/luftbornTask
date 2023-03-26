import { SaveProduct } from './../_models/save-product';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from 'src/environments/environment';
@Injectable()
export class ProductService {
  private readonly productEndpoint = environment.baseUrl + 'api/products/';

  constructor(private httpClient: HttpClient) {}

  create(product: SaveProduct) {
    return this.httpClient.post(this.productEndpoint, product);
  }

  getProduct(id: number) {
    return this.httpClient.get(this.productEndpoint + id);
  }

  getProducts(query: any) {
    return this.httpClient.get(
      this.productEndpoint + '?' + this.toQueryString(query)
    );
  }

  toQueryString(filter: any) {
    var parts = [];
    for (var property in filter) {
      var value = filter[property];
      if (value != null && value != undefined)
        parts.push(
          encodeURIComponent(property) + '=' + encodeURIComponent(value)
        );
    }

    return parts.join('&');
  }

  update(product: SaveProduct) {
    return this.httpClient.put(this.productEndpoint + product.id, product);
  }

  delete(id: number) {
    return this.httpClient.delete(this.productEndpoint + id);
  }
  deleteMany(ids: number[]) {
    return this.httpClient.delete(this.productEndpoint + 'deletemany', {
      body: ids,
    });
  }
}
