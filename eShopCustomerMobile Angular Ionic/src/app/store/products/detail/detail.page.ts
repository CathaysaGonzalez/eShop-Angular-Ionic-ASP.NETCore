import { Component, OnInit } from "@angular/core";
import { Subscription } from "rxjs";
import { ActivatedRoute, Router } from '@angular/router';
import { NavController } from '@ionic/angular';
import { ProductsService } from 'src/app/services/products.service';
import { CartService } from 'src/app/services/cart.service';
import { Product } from 'src/app/models/product.model';

@Component({
  selector: "app-detail",
  templateUrl: "./detail.page.html",
  styleUrls: ["./detail.page.scss"],
})
export class DetailPage implements OnInit {
  [x: string]: any;
  product: Product;
  isLoading = false;
  private productSub: Subscription;
  products: Product[];

  constructor(
    private route: ActivatedRoute,
    private navCtrl: NavController,
    private productsService: ProductsService,
    private router: Router,
    private cartService: CartService
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe((paramMap) => {
      if (!paramMap.has("productId")) {
        this.navCtrl.navigateBack("/store/tabs/categories");
        return;
      }
      this.isLoading = true;
      this.productSub = this.productsService
        .getProduct(+paramMap.get("productId"))
        .subscribe(
          (product) => {
            this.product = product;
            this.isLoading = false;
          },
          (error) => {
            this.alertCtrl
              .create({
                header: "Ha ocurrido un error",
                message: "El producto no puede ser mostrado",
                buttons: [
                  {
                    text: "Okay",
                    handler: () => {
                      this.router.navigate(["/store/tabs/categories"]);
                    },
                  },
                ],
              })
              .then((alertEl) => alertEl.present());
          }
        );
    });
  }
  
  onAddToCart() {
    this.cartService.addProductFromList(this.product);
    this.navCtrl.navigateBack("/store/tabs/categories");
  }

  ngOnDestroy() {
    if (this.productSub) {
      this.productSub.unsubscribe();
    }
  }
}
