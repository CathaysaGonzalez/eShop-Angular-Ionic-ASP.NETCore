import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { Product, ProductData } from 'src/app/models/product.model';
import { Category } from 'src/app/models/category.model';
import { Supplier } from 'src/app/models/supplier.model';
import { NgForm } from '@angular/forms';
import { ProductsService } from 'src/app/services/products.service';
import { CategoriesService } from 'src/app/services/categories.service';
import { Router } from '@angular/router';
import { SuppliersService } from 'src/app/services/suppliers.service';

@Component({
  selector: 'app-new-product',
  templateUrl: './new-product.component.html',
  styleUrls: ['./new-product.component.css']
})
export class NewProductComponent implements OnInit, OnDestroy {
  private suppliersSub: Subscription;
  private categoriesSub: Subscription;
  private productsSub: Subscription;
  products: Product[];
  categories: Category[];
  suppliers: Supplier[];
  isLoading = false;
  product: Product;
  newProduct = new ProductData();
  @ViewChild('form') form: NgForm;

  constructor(
    private productsService: ProductsService,
    private router: Router,
    private supplierService: SuppliersService,
    private categoriesService: CategoriesService
  ) { }

  ngOnInit(): void {
    this.suppliersSub = this.supplierService.suppliers.subscribe(
      (suppliers) => {
        this.suppliers = suppliers;
      }
    );
    this.categoriesSub = this.categoriesService.categories.subscribe(
      (categories) => {
        this.categories = categories;
      }
    );
    this.productsSub = this.productsService.products.subscribe((products) => {
      this.products = products;
    })
  }

  save(form: NgForm) {
    this.productsService
      .addProduct(
        form.value.name,
        form.value.description,
        form.value.price,
        form.value.isFeatured,
        form.value.unitsInStock,
        form.value.modelNumber,
        form.value.modelName,
        form.value.currentPrice,
        'aaa.jpg',
        'aaa.jpg',
        'aaa.jpg',
        form.value.supplierId,
        form.value.categoryId
      )
      .subscribe(() => {
        this.productsService.fetchProducts().subscribe(products =>{
          this.products=products;
          this.router.navigateByUrl('/admin/products');
        });
      });
    this.suppliersSub = this.supplierService.suppliers.subscribe(
      (suppliers) => {
        this.suppliers = suppliers;
      }
    );
    this.categoriesSub = this.categoriesService.categories.subscribe(
      (categories) => {
        this.categories = categories;
      }
    );
  }

  ngAfterViewInit() {
    this.isLoading = true;
    this.supplierService.fetchSuppliers().subscribe(() => {
      this.isLoading = false;
    });
    this.categoriesService.fetchCategories().subscribe(() => {
      this.isLoading = false;
    });
  }

  ngOnDestroy() {
    if (this.suppliersSub) {
      this.suppliersSub.unsubscribe();
    }
    if (this.categoriesSub) {
      this.categoriesSub.unsubscribe();
    }
    if (this.productsSub) {
      this.productsSub.unsubscribe();
    }
  }

}
