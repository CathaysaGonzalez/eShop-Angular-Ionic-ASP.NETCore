import {Component, OnDestroy, OnInit} from '@angular/core';
import { Order } from 'src/app/models/order.model';
import { Subscription } from 'rxjs';
import { OrderService } from 'src/app/services/order.service';
import { LoadingController, IonItemSliding } from '@ionic/angular';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.page.html',
  styleUrls: ['./orders.page.scss'],
})
export class OrdersPage implements OnInit, OnDestroy {

  loadedOrders: Order[];
  private orderSub: Subscription;
  isLoading = false;
  ordersByUser : Order[];

  constructor(
    private orderService: OrderService,
    private loadingCtrl: LoadingController
  ) { }

  ngOnInit() {
    this.orderSub = this.orderService.orders.subscribe((orders) => {
      this.loadedOrders = orders;
    });
  }

  ionViewWillEnter() {
    this.isLoading = true;
    this.orderService.fetchOrders().subscribe((orders) => {
      this.isLoading = false;
    });
  }

  onCancelOrder(id: number, slidingEl: IonItemSliding) {
    slidingEl.close();
    this.loadingCtrl
      .create({ message: "Cancelling..." })
      .then((loadingEl) => {
        loadingEl.present();
        this.orderService
          .cancelOrder(id)
          .subscribe(() => {
            loadingEl.dismiss();
          });
      });
  }

  ngOnDestroy() {
    if (this.orderSub) {
      this.orderSub.unsubscribe();
    }
  }

}
