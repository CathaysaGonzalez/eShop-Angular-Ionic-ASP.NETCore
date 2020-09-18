import { Component, OnInit, OnDestroy } from "@angular/core";
import { Subscription } from "rxjs";
import { NavController, AlertController, LoadingController } from "@ionic/angular";
import { ActivatedRoute, Router } from "@angular/router";
import { OrderService } from "src/app/services/order.service";
import { Order } from "src/app/models/order.model";
import { FormControl, Validators, FormGroup } from "@angular/forms";

@Component({
  selector: "app-detail-order",
  templateUrl: "./detail-order.page.html",
  styleUrls: ["./detail-order.page.scss"],
})
export class DetailOrderPage implements OnInit, OnDestroy {
  isLoading = false;
  private orderSub: Subscription;
  orderTotal: Order;
  ordersTotal: Order[];
  form: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private navCtrl: NavController,
    private ordersService: OrderService,
    private router: Router,
    private loadingCtrl: LoadingController,
    private alertCtrl: AlertController
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe((paramMap) => {
      if (!paramMap.has("orderId")) {
        this.navCtrl.navigateBack("/admin/tabs/orders");
        return;
      }
      this.orderSub = this.ordersService.orders.subscribe((ordersTotal) => {
        this.ordersTotal = ordersTotal;
      });
      this.isLoading = true;
      this.ordersService.getOrder(+paramMap.get("orderId"))
      .subscribe(
        (orderTotal) => {
          this.orderTotal = orderTotal;
          this.form = new FormGroup({
            shipped: new FormControl(this.orderTotal.shipped, {
              updateOn: "blur",
            }),
          });
          this.isLoading = false;
        },
        (error) => {
          this.alertCtrl
            .create({
              header: "Ha ocurrido un error",
              message: "No se puede cargar el pedido",
              buttons: [
                {
                  text: "Okay",
                  handler: () => {
                    this.router.navigate(["/admim/tabs/orders"]);
                  },
                },
              ],
            })
            .then((alertEl) => alertEl.present());
        }
      );
    });
  }

  ionViewWillEnter() {
    this.isLoading = true;
    this.ordersService.fetchOrders()
    .subscribe(() => {
      this.isLoading = false;
    });
  }

  onUpdateOrder() {
    if (!this.form.valid) {
      return;
    }
    this.loadingCtrl
      .create({
        message: "Updating order...",
      })
      .then((loadingEl) => {
        loadingEl.present();
        this.ordersService
          .updateOrder(
            this.orderTotal.id,
            this.orderTotal.name,
            this.orderTotal.address,
            this.form.value.shipped,
            this.orderTotal.total,
            this.orderTotal.userName,
            this.orderTotal.paymentNavigation,
            this.orderTotal.cartLines,
            this.orderTotal.appUser
          )
          .subscribe(() => {
            loadingEl.dismiss();
            this.form.reset();
            this.router.navigate(["/admin/tabs/orders"]);
          });
      });
  }

  getTotal() {
    if (this.orderTotal.cartLines)
      return this.orderTotal.cartLines.reduce(
        (sum, current) =>
          sum + current.productNavigation.price * current.quantity,
        0
      );
  }

  ngOnDestroy() {
    if (this.orderSub) {
      this.orderSub.unsubscribe();
    }
  }
}
