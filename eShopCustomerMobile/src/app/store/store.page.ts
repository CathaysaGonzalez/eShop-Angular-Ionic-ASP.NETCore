import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CartService } from '../services/cart.service';

@Component({
  selector: 'app-store',
  templateUrl: './store.page.html',
  styleUrls: ['./store.page.scss'],
})
export class StorePage implements OnInit {
  cartItemCount: BehaviorSubject<number>;

  constructor(private cartService: CartService) { }

  ngOnInit() {
    this.cartItemCount = this.cartService.getCartItemCount();
  }

}
