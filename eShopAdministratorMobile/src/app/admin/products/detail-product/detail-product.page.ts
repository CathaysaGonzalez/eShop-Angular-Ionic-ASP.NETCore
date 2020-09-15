import { Component, OnInit, OnDestroy } from "@angular/core";
import { Subscription } from "rxjs";
import { ActivatedRoute, Router } from "@angular/router";
import { ProductsService } from "src/app/services/products.service";
import {
  NavController,
  LoadingController,
  AlertController,
} from "@ionic/angular";
import { Product } from 'src/app/models/product.model';

@Component({
  selector: "app-detail-product",
  templateUrl: "./detail-product.page.html",
  styleUrls: ["./detail-product.page.scss"],
})
export class DetailProductPage implements OnInit, OnDestroy {
  product: Product;
  private productsSub: Subscription;
  productId: number;
  isLoading = false;

  constructor(
    private route: ActivatedRoute,
    private productsService: ProductsService,
    private navCtrl: NavController,
    private router: Router,
    private loadingCtrl: LoadingController,
    private alertCtrl: AlertController
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe((paramMap) => {
      if (!paramMap.has("productId")) {
        this.navCtrl.navigateBack("/admin/tabs/products");
        return;
      }
      this.productId = +paramMap.get("productId");
      this.isLoading = true;
      this.productsSub = this.productsService
        .getProduct(+paramMap.get("productId"))
        .subscribe(
          (product) => {
            this.product = product;
            this.isLoading = false;
          },
          (error) => {
            this.alertCtrl
              .create({
                header: "An error occurred!",
                message:
                  "Product could not be fetched. Please try again later.",
                buttons: [
                  {
                    text: "Okay",
                    handler: () => {
                      this.router.navigate(["/admin/tabs/products"]);
                    },
                  },
                ],
              })
              .then((alertEl) => {
                alertEl.present();
              });
          }
        );
    });
  }

  ngOnDestroy() {
    if (this.productsSub) {
      this.productsSub.unsubscribe();
    }
  }
}
