import { Component, OnInit, OnDestroy } from "@angular/core";
import { Subscription } from "rxjs";
// import { ProductDetails } from 'src/app/models/product-details.model';
import { ProductsService } from "src/app/services/products.service";
import { LoadingController, IonItemSliding } from "@ionic/angular";
import { Router } from "@angular/router";
import { CategoriesService } from "src/app/services/categories.service";
import { Category } from "src/app/models/category.model";
import { Product } from "src/app/models/product.model";

@Component({
  selector: "app-products",
  templateUrl: "./products.page.html",
  styleUrls: ["./products.page.scss"],
})
export class ProductsPage implements OnInit, OnDestroy {
  isLoading = false;
  products: Product[];
  private productsSub: Subscription;
  private categoriesSub: Subscription;
  categories: Category[];
  productsByCategory: Product[];
  toggleCategory: any[] = [];
  toggleProduct: any[][] = [];
  productsForCategory: any[][] = [];

  constructor(
    private productsService: ProductsService,
    private categoriesService: CategoriesService,
    private loadingCtrl: LoadingController,
    private router: Router
  ) {}

  ngOnInit() {
    console.log("ngOnInit");
    this.productsSub = this.productsService.products.subscribe((products) => {
      this.products = products;
      this.categoriesSub = this.categoriesService.categories.subscribe(
        (categories) => {
          this.categories = categories;
          this.categories.forEach((category) => {
            this.productsService
              .searchProductsByCategory(category.name)
              .subscribe(
                (res) => {
                  this.productsForCategory[category.id] = res; 
                  this.isLoading = false;
                },
                (err) => {
                  this.productsByCategory = [];
                }
              );
          });
        }
      );
    });
  }

  ionViewWillEnter() {
    this.isLoading = true;
    this.categoriesService.fetchCategories().subscribe(() => {
      this.isLoading = false;
    });
    this.productsService.fetchProducts().subscribe(() => {
      this.isLoading = false;
    });
  }

  onEditProduct(id: number, slidingEl: IonItemSliding) {
    slidingEl.close();
    this.router.navigate(["/", "admin", "tabs", "products", "edit", id]);
  }

  onCancelProduct(id: number, slidingEl: IonItemSliding) {
    slidingEl.close();
    this.loadingCtrl.create({ message: "Eliminando..." }).then((loadingEl) => {
      loadingEl.present();
      this.productsService.cancelProduct(id).subscribe(() => {
        loadingEl.dismiss();
      });
    });
  }

  ngOnDestroy() {
    if (this.productsSub) {
      this.productsSub.unsubscribe();
    }
    if (this.categoriesSub) {
      this.categoriesSub.unsubscribe();
    }
  }
}
