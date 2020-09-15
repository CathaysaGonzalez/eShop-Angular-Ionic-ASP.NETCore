import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductsService } from 'src/app/services/products.service';
import { CategoriesService } from 'src/app/services/categories.service';
import { Product } from 'src/app/models/product.model';
import { NgForm } from '@angular/forms';
import { SuppliersService } from 'src/app/services/suppliers.service';
import { Subscription } from 'rxjs';
import { Supplier } from 'src/app/models/supplier.model';
import { Category } from 'src/app/models/category.model';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css'],
})
export class EditProductComponent implements OnInit {
  product: Product;
  private productsSub: Subscription;
  productId: number;
  isLoading = false;
  categories: Category[];
  suppliers: Supplier[];
  private suppliersSub: Subscription;
  private categoriesSub: Subscription;
  identifier: number;
  products: Product[];

  @ViewChild('form') form: NgForm;

  newProduct = new Product();

  constructor(
    private route: ActivatedRoute, //activeRoute
    private productsService: ProductsService,
    private router: Router,
    private categoriesService: CategoriesService,
    private suppliersService: SuppliersService
  ) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      this.suppliersSub = this.suppliersService.suppliers.subscribe(
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
      });
      this.productId = +paramMap.get('id');
      this.identifier = this.productId;
      this.productsSub = this.productsService
        .getProduct(+paramMap.get('id'))
        .subscribe((product) => {
          this.newProduct = product;
         });
    });
  }

  ngAfterViewInit() {
    this.isLoading = true;
    this.productsService.fetchProducts().subscribe(() => {
      this.isLoading = false;
    });
    this.suppliersService.fetchSuppliers().subscribe(() => {
      this.isLoading = false;
    });
    this.categoriesService.fetchCategories().subscribe(() => {
      this.isLoading = false;
    });
  }

  save(form: NgForm) {
    this.productsService
      .updateProduct(
        this.identifier,
        form.value.name,
        form.value.description,
        form.value.price,
        form.value.supplierId,
        form.value.categoryId,
        form.value.unitsInStock,
        form.value.modelNumber,
        form.value.modelName,
        form.value.currentPrice,
        form.value.isFeatured,
        'image.jpg'
      )
      .subscribe((res) => {
        this.productsService.fetchProducts().subscribe((products) => {
          this.products = products;
          this.router.navigateByUrl('/admin/products');
        });
      });
  }

  ngOnDestroy() {
    if (this.productsSub) {
      this.productsSub.unsubscribe();
    }
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
