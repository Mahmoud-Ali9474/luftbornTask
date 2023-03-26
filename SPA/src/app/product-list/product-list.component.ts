import { QueryResult } from './../_models/query-result';
import { Router } from '@angular/router';
import { Product } from './../_models/product';
import { Component, OnInit } from '@angular/core';
import { ProductService } from '../_services/product.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css'],
})
export class ProductListComponent implements OnInit {
  readonly PAGE_SIZE = 5;
  queryResult: QueryResult = {
    items: [],
    totalItems: 0,
  };
  query: any = {
    tagName: '',
    title: '',
    pageSize: this.PAGE_SIZE,
  };
  columns: any = [
    { title: 'Id' },
    { title: 'Title', key: 'title', isSortable: true },
    { title: 'Quantity' },
    { title: 'Price', key: 'price', isSortable: true },
    { title: 'Added Date', key: 'addedDate', isSortable: true },
    { title: 'Category', key: 'category', isSortable: true },
    { title: 'Tags' },
  ];
  constructor(private productService: ProductService, private router: Router) {}

  ngOnInit(): void {
    this.getProducts();
  }
  flatProductTags(product: Product) {
    return product.tags.map((k) => k.name).join(',');
  }
  getProducts() {
    console.log(this.query);
    this.productService.getProducts(this.query).subscribe(
      (data: any) => {
        this.queryResult.items = data.items;
        this.queryResult.totalItems = data.totalItems;
      },
      (err) => {
        if (err.status == 404) this.router.navigate(['/home']);
      }
    );
  }

  onResetfilter() {
    this.query = {};
    this.query.page = 1;
    this.query.pageSize = this.PAGE_SIZE;
    this.getProducts();
  }
  onPageChanged(page: number) {
    this.query.page = page;
    this.query.pageSize = this.PAGE_SIZE;
    this.getProducts();
  }
  sortBy(columnName: string) {
    if (this.query.sortBy === columnName) {
      this.query.isSortAscending = !this.query.isSortAscending;
    } else {
      this.query.sortBy = columnName;
      this.query.isSortAscending = true;
    }
    this.getProducts();
  }
  applyFilter() {
    this.query.page = 1;
    this.query.pageSize = this.PAGE_SIZE;
    this.getProducts();
  }
}
