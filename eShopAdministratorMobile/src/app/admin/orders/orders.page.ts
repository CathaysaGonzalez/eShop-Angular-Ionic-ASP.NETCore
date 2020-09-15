import { Component, OnInit, OnDestroy } from '@angular/core';
import { Order } from 'src/app/models/order.model';
import { Subscription } from 'rxjs';
import { OrderService } from 'src/app/services/order.service';
import { LoadingController, IonItemSliding } from '@ionic/angular';
import { Router } from '@angular/router';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.page.html',
  styleUrls: ['./orders.page.scss'],
})
export class OrdersPage implements OnInit, OnDestroy {
  loadedOrders: Order[];
  private orderSub: Subscription;
  isLoading = false;

  constructor(
    private orderService: OrderService,
    private router: Router
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

  onEditOrder(id: number, slidingEl: IonItemSliding) {
    slidingEl.close();
    this.router.navigate(['/', 'admin', 'tabs','orders', 'edit', id]);
  }  

  ngOnDestroy() {
    if (this.orderSub) {
      this.orderSub.unsubscribe();
    }
  }

}
