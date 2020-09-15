import { Component, OnInit } from "@angular/core";
import { Subscription, BehaviorSubject } from "rxjs";
import { Category } from "src/app/models/category.model";
import { CartRecord } from "src/app/models/cart-record.model";
import { CategoriesService } from "src/app/services/categories.service";
import { ProductsService } from "src/app/services/products.service";
import { ModalController, IonItemSliding } from "@ionic/angular";
import { CartService } from "src/app/services/cart.service";
import { CartPage } from "../cart/cart.page";
import { Product } from "src/app/models/product.model";

@Component({
  selector: "app-categories",
  templateUrl: "./categories.page.html",
  styleUrls: ["./categories.page.scss"],
})
export class CategoriesPage implements OnInit {
  private categoriesSub: Subscription;
  categories: Category[];
  productsByCategory: Product[];
  toggleCategory: any[] = [];
  toggleProduct: any[][] = [];
  result: string;
  cartResult: string;
  cartItemCount: BehaviorSubject<number>;
  cart: CartRecord[] = [];
  isLoading = false;
  products: Product[];
  private productsSub: Subscription;

  constructor(
    private categoriesService: CategoriesService,
    private productsService: ProductsService,
    private cartService: CartService,
    private modalCtrl: ModalController
  ) {}

  ngOnInit() {
    this.productsSub = this.productsService.products.subscribe((products) => {
      this.products = products;
    });
    this.categoriesSub = this.categoriesService.categories.subscribe(
      (categories) => {
        this.categories = categories;
      }
    );
    this.cartItemCount = this.cartService.getCartItemCount();
  }

  ionViewWillEnter() {
    this.isLoading = true;
    this.categoriesService.fetchCategories().subscribe((categories) => {
      this.categories = categories;
      this.onSetCategory(categories[0]);
      this.isLoading = false;
    });
    this.productsService.fetchProducts().subscribe((products) => {
      this.products = products;
      this.isLoading = false;
      });
    this.toggleCategory = [false];
  }

  onSetCategory(category: Category) {
    if (this.categories) {
      this.toggleCategory[category.id] = !this.toggleCategory[category.id];
      this.productsService.searchProductsByCategory(category.name).subscribe(
        (res) => {
          this.productsByCategory = res;
          this.toggleProduct[category.id] = this.productsByCategory;
        },
        (err) => {
          this.productsByCategory = [];
        }
      );
    }
  }

  onSetProduct(category: Category, product: Product) {
    this.toggleCategory[category.id][product.id] = !this.toggleCategory[
      category.id
    ][product.id];
  }

  addToCart(product: Product, slidingEl: IonItemSliding) {
    slidingEl.close();
    this.cartService.addProductFromList(product);
  }

  async openCart() {
    let modal = await this.modalCtrl
      .create({
        component: CartPage,
      })
      .then((modalEl) => {
        modalEl.present();
        return modalEl.onDidDismiss();
      })
      .then((resultData) => {
        this.result = JSON.stringify(resultData.data);
        this.cartResult = JSON.stringify(this.cart);
      });
  }

  ngOnDestroy() {
    if (this.categoriesSub) {
      this.categoriesSub.unsubscribe();
    }
    if (this.productsSub) {
      this.productsSub.unsubscribe();
    }
  }
}
