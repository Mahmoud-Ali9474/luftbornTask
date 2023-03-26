import { ToastrService } from 'ngx-toastr';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Product } from '../_models/product';
import { ProductService } from '../_services/product.service';
import { QueryResult } from '../_models/query-result';

@Component({
  selector: 'app-admain-product-list',
  templateUrl: './admain-product-list.component.html',
  styleUrls: ['./admain-product-list.component.css'],
})
export class AdmainProductListComponent implements OnInit {
  chechList: number[] = [];
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
    {},
  ];
  constructor(
    private productService: ProductService,
    private router: Router,
    private toastrService: ToastrService
  ) {}

  ngOnInit(): void {
    this.getProducts();
  }
  flatProductTags(product: Product) {
    return product.tags.map((k) => k.name).join(',');
  }
  getProducts() {
    this.productService.getProducts(this.query).subscribe(
      (data: any) => {
        this.queryResult.items = data.items;
        this.queryResult.totalItems = data.totalItems;
      },
      (err: any) => {
        if (err.status == 404) this.router.navigate(['/home']);
      }
    );
  }
  onChechboxChange(id: number, $event: any) {
    console.log(id);
    if ($event.target.checked) this.chechList.push(id);
    else {
      var index = this.chechList.indexOf(id);
      this.chechList.splice(index, 1);
    }
  }
  onSelectAll($event: any) {
    debugger;
    if ($event.target.checked)
      this.chechList = this.queryResult.items.map((p) => p.id);
    else this.chechList = [];
  }
  deleteMany() {
    if (this.chechList.length == 0) return;
    this.productService.deleteMany(this.chechList).subscribe((ids: any) => {
      ids.forEach((id: any) => {
        var idx = this.queryResult.items.findIndex((p) => p.id == id);
        this.queryResult.items.splice(idx, 1);
        this.chechList = [];
      });
      this.toastrService.success('Products deleted successfully.');
    });
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
