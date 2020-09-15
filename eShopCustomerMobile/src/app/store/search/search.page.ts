import { Component, OnInit, OnDestroy } from "@angular/core";
import { ProductsService } from "src/app/services/products.service";
import { CartService } from "src/app/services/cart.service";
import {
  ModalController,
  AlertController,
  IonItemSliding,
} from "@ionic/angular";
import { Subscription, BehaviorSubject } from "rxjs";
import { CartRecord } from "src/app/models/cart-record.model";
import { Product } from "src/app/models/product.model";

@Component({
  selector: "app-search",
  templateUrl: "./search.page.html",
  styleUrls: ["./search.page.scss"],
})
export class SearchPage implements OnInit, OnDestroy {
  products: Product[];
  selectedProducts: Product[];

  relevantProducts: Product[];
  private productsSub: Subscription;
  isLoading = false;
  cart: CartRecord[] = [];
  cartItemCount: BehaviorSubject<number>;
  result: string;
  cartResult: string;

  private selProductsSub: Subscription;

  constructor(
    private productsService: ProductsService,
    private cartService: CartService,
    private modalCtrl: ModalController,
    private alertCtrl: AlertController
  ) {}

  ngOnInit() {
    this.productsSub = this.productsService.products.subscribe((products) => {
      this.products = products;
      this.selectedProducts = products;
      this.cartItemCount = this.cartService.getCartItemCount();
    });
  }
  
    ionViewWillEnter() {
    this.isLoading = true;
    this.productsService.fetchProducts().subscribe(() => {
      this.isLoading = false;
    });
  }

  onSearchChange(search) {
    let searchValue = search.detail.value;
    if (searchValue == "") {
    } else {
      this.productsService.searchProductsByName(searchValue).subscribe(
        (res) => {
          this.selectedProducts = res;
        },
        (err) => {
          this.selectedProducts = [];
        }
      );
    }
  }

  addToCart(product: Product, slidingEl: IonItemSliding) {
    slidingEl.close();
    this.cartService.addProductFromList(product);
  }

  ngOnDestroy() {
    if (this.productsSub) {
      this.productsSub.unsubscribe();
    }
  }
}
