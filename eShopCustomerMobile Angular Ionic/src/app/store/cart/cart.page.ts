import { Component, OnInit } from "@angular/core";
import { Subscription, BehaviorSubject } from "rxjs";
import { Order } from "src/app/models/order.model";
import { CartService } from "src/app/services/cart.service";
import { OrderService } from "src/app/services/order.service";
import {
  ModalController,
  AlertController,
  LoadingController,
} from "@ionic/angular";
import { CartLine } from "src/app/models/cart-line.model";
import { CartRecord } from "src/app/models/cart-record.model";
import { CreateOrderComponent } from '../orders/create-order/create-order.component';

@Component({
  selector: "app-cart",
  templateUrl: "./cart.page.html",
  styleUrls: ["./cart.page.scss"],
})
export class CartPage implements OnInit {
  cartItemCount: BehaviorSubject<number>;
  order = new Order();
  cart: CartRecord[];
  private cartSub: Subscription;

  constructor(
    private cartService: CartService,
    private orderService: OrderService,
    private modalCtrl: ModalController,
    private alertCtrl: AlertController,
    private loadingCtrl: LoadingController
  ) {}

  ngOnInit() {
    this.cartSub = this.cartService.cart.subscribe((cart) => {
      this.cart = cart;
    });
    this.cartItemCount = this.cartService.getCartItemCount();
  }

  decreaseCartItem(p: CartRecord) {
    this.cartService.decreaseProduct(p);
  }

  increaseCartItem(p: CartRecord) {
    this.cartService.addProduct(p);
  }

  removeCartItem(p: CartRecord) {
    this.cartService.removeProduct(p);
  }

  getTotal() {
    if (this.cart)
      return this.cart.reduce(
        (sum, current) => sum + current.price * current.quantity,
        0
      );
  }

  close() {
    this.modalCtrl.dismiss({ cart: this.cart });
  }

  async checkout() {
    this.order.cartLines = this.cart.map((p) => new CartLine(p.id, p.quantity));
    this.order.total = this.getTotal();

    this.modalCtrl
      .create({
        component: CreateOrderComponent,
      })
      .then((modalEl) => {
        modalEl.present();
        return modalEl.onDidDismiss();
      })
      .then((resultData) => {
        if (resultData.role === "confirm") {
          this.loadingCtrl
            .create({ message: "Realizando compra..." })
            .then((loadingEl) => {
              loadingEl.present();
              const data = resultData.data.OrderData;
              let paymentNavigation = {
                cardNumber: data.cardNumber,
                cardExpiry: data.cardExpiry,
                cardSecurityCode: data.cardSecurityCode,
              };
              this.orderService
                .addOrder(
                  data.name,
                  data.address,
                  paymentNavigation,
                  this.order.cartLines,
                  this.order.total
                )
                .subscribe(async () => {
                  loadingEl.dismiss();
                  const alert = await this.alertCtrl.create({
                    header: "Gracias por su pedido",
                    message: "Su pedido será entregado tan pronto como sea posible",
                    buttons: ["Ok"],
                  });
                  alert.present().then(() => {
                    this.cartService.removeProducts();
                  });
                });
            });
        }
      });
  }

  ngOnDestroy() {
    if (this.cartSub) {
      this.cartSub.unsubscribe();
    }
  }
}
