import { QueryResult } from './../_models/query-result';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Category } from '../_models/category';
import { CategoryService } from '../_services/category.service';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css'],
})
export class CategoryListComponent implements OnInit {
  readonly PAGE_SIZE: number = 5;

  queryResult: QueryResult = { items: [], totalItems: 0 };
  query: any = {
    name: '',
    pageSize: this.PAGE_SIZE,
  };
  columns: any = [
    { title: 'Id' },
    { title: 'Title', key: 'name', isSortable: true },
    { title: 'Tags' },
    {},
  ];
  constructor(
    private categoryService: CategoryService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getCategories();
  }
  flatCategoriesTags(category: Category) {
    return category.tags.map((k) => k.name).join(',');
  }
  getCategories() {
    console.log(this.query);
    this.categoryService.getCategories(this.query).subscribe(
      (data: any) => {
        this.queryResult.items = data.items;
        this.queryResult.totalItems = data.totalItems;
        console.log(data);
      },
      (err) => {
        if (err.status == 404) this.router.navigate(['/home']);
      }
    );
  }
  onPageChanged(page: number) {
    this.query.page = page;
    this.query.pageSize = this.PAGE_SIZE;
    this.getCategories();
  }
  sortBy(columnName: string) {
    if (this.query.sortBy === columnName) {
      this.query.isSortAscending = !this.query.isSortAscending;
    } else {
      this.query.sortBy = columnName;
      this.query.isSortAscending = true;
    }
    this.getCategories();
  }
  applyFilter() {
    this.query.page = 1;
    this.query.pageSize = this.PAGE_SIZE;
    this.getCategories();
  }
}
