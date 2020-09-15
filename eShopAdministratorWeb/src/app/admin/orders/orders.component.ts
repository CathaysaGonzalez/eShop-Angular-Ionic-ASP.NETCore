import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { Order } from 'src/app/models/order.model';
import { Router } from '@angular/router';
import { OrdersService } from 'src/app/services/orders.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css'],
})
export class OrdersComponent implements OnInit {
  orders: Order[];
  private orderSub: Subscription;
  isLoading = false;

  includeShipped = false;

  constructor(private orderService: OrdersService, private router: Router) {}

  ngOnInit(): void {
    this.orderSub = this.orderService.orders.subscribe((orders) => {
      this.orders = orders;
    });
  }

  getOrders() {
    return this.orders.filter((o) => this.includeShipped || !o.shipped);
  }

  ngAfterViewInit() {
    this.isLoading = true;
    this.orderService.fetchOrders().subscribe((orders) => {
      this.isLoading = false;
    });
  }

  onMarkShippedOrder(id: number) {
    this.orderService.shipOrder(id).subscribe(() => {});
  }

  onCancelOrder(id: number) {
    this.orderService.cancelOrder(id).subscribe(() => {});
  }

  ngOnDestroy() {
    if (this.orderSub) {
      this.orderSub.unsubscribe();
    }
  }
}
