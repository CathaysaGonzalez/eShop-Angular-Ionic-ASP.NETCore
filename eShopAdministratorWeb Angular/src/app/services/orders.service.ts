import { Injectable } from '@angular/core';
import { BehaviorSubject, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Order, OrderData } from '../models/order.model';
import { switchMap, take, tap } from 'rxjs/operators';
import { Payment } from '../models/payment.model';
import { CartLine } from '../models/cart-line.model';
import { AppUser } from '../models/app-user.model';
import { AuthService } from './auth.service';

interface OrderCreated {
  user: string;
  orderId: number;
  authCode: string;
  amount: number;
}

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  private _orders = new BehaviorSubject<Order[]>([]);

  get orders() {
    return this._orders.asObservable();
  }
  constructor(private http: HttpClient, private authService: AuthService) { }

  getOrder(id: number) {
    return this.authService.token.pipe(
      take(1),
      switchMap((token) => {
        const httpOptions = {
          headers: new HttpHeaders({
            'Content-Type': 'application/json',
            Authorization: 'Bearer ' + token,
          }),
        };
        return this.http
          .get<Order>(
            environment.apiEndpoint + 'orders/details/' + id,
            httpOptions
          )
          .pipe(
            tap((resData) => {
            })
          );
      })
    );
  }

  updateOrder(
    id: number,
    name: string,
    address: string,
    shipped: boolean,
    total: number,
    userId: string,
    paymentNavigation: Payment,
    cartLines: CartLine[],
    appUser: AppUser
  ) {
    let updatedOrders: Order[];
    let fetchedToken: string;
    return this.authService.token.pipe(
      take(1),
      switchMap((token) => {
        fetchedToken = token;
        return this.orders;
      }),
      take(1),
      switchMap((orders) => {
        if (!orders || orders.length <= 0) {
          return this.fetchOrders();
        } else {
          return of(orders);
        }
      }),
      switchMap((orders) => {
        const updatedOrderIndex = orders.findIndex((o) => o.id === id);
        updatedOrders = [...orders];
        const oldOrder = updatedOrders[updatedOrderIndex];
        updatedOrders[updatedOrderIndex] = new Order(
          oldOrder.id,
          oldOrder.name,
          oldOrder.address,
          shipped,
          oldOrder.total,
          oldOrder.userName,
          oldOrder.paymentNavigation,
          oldOrder.cartLines,
          oldOrder.appUser
        );
        const httpOptions = {
          headers: new HttpHeaders({
            'Content-Type': 'application/json',
            Authorization: 'Bearer ' + fetchedToken,
          }),
        };
        return this.http.put(
          environment.apiEndpoint + 'orders/' + id,
          { ...updatedOrders[updatedOrderIndex] },
          httpOptions
        );
      }),
      tap(() => {
        this._orders.next(updatedOrders);
      })
    );
  }

  fetchOrders() {
    return this.authService.token.pipe(
      take(1),
      switchMap((token) => {
        const httpOptions = {
          headers: new HttpHeaders({
            'Content-Type': 'application/json',
            Authorization: 'Bearer ' + token,
          }),
        };
        return this.http.get<Order[]>(
          environment.apiEndpoint + 'orders/orders/cartlines',
          httpOptions
        );
      }),
      tap((orders) => {
        this._orders.next(orders);
      })
    );
  }

  addOrder(
    name: string,
    address: string,
    paymentNavigation: Payment,
    cartLines: CartLine[],
    total: number
  ) {
    let generatedId: number;
    let shipped = false;
    let newUserName: string;
    let fetchedUserName: string;
    return this.authService.userName.pipe(
      take(1),
      switchMap((userName) => {
        fetchedUserName = userName;
        if (!userName) {
          throw new Error('No user name found!');
        }
        return this.authService.token;
      }),
      take(1),
      switchMap((token) => {
        newUserName = fetchedUserName;
        let newOrderData = new OrderData(
          (name = name),
          (address = address),
          (shipped = shipped),
          (total = total),
          (fetchedUserName = fetchedUserName),
          (paymentNavigation = paymentNavigation),
          (cartLines = cartLines)
        );
        const httpOptions = {
          headers: new HttpHeaders({
            'Content-Type': 'application/json',
            Authorization: 'Bearer ' + token,
          }),
        };
        return this.http.post<OrderCreated>(
          environment.apiEndpoint + 'orders',
          newOrderData,
          httpOptions
        );
      }),
      switchMap((resData) => {
        generatedId = resData.orderId;
        return this.orders;
      }),
      take(1),
      tap((orders) => {
        let newOrder = new Order(
          generatedId,
          name,
          address,
          shipped,
          total,
          newUserName,
          paymentNavigation,
          cartLines
        );
        this._orders.next(orders.concat(newOrder));
      })
    );
  }

  shipOrder(id: number) {
    let updatedOrders: Order[];
    let fetchedToken: string;
    return this.authService.token.pipe(
      take(1),
      switchMap((token) => {
        fetchedToken = token;
        return this.orders;
      }),
      take(1),
      switchMap((orders) => {
        if (!orders || orders.length <= 0) {
          return this.fetchOrders();
        } else {
          return of(orders);
        }
      }),
      switchMap((orders) => {
        const updatedOrderIndex = orders.findIndex((o) => o.id === id);
        updatedOrders = [...orders];
        const oldOrder = updatedOrders[updatedOrderIndex];
        updatedOrders[updatedOrderIndex] = new Order(
          oldOrder.id,
          oldOrder.name,
          oldOrder.address,
          true,
          oldOrder.total,
          oldOrder.userName,
          oldOrder.paymentNavigation,
          oldOrder.cartLines,
          oldOrder.appUser
        );
        const httpOptions = {
          headers: new HttpHeaders({
            'Content-Type': 'application/json',
            Authorization: 'Bearer ' + fetchedToken,
          }),
        };
        return this.http.post(
          environment.apiEndpoint + 'orders/shipped/' + id,
          {},
          httpOptions
        );
      }),
      tap(() => {
        this._orders.next(updatedOrders);
      })
    );
  }

  cancelOrder(id: number) {
    return this.authService.token.pipe(
      take(1),
      switchMap((token) => {
        const httpOptions = {
          headers: new HttpHeaders({
            'Content-Type': 'application/json',
            Authorization: 'Bearer ' + token,
          }),
        };
        return this.http.delete(
          environment.apiEndpoint + 'orders/' + id,
          httpOptions
        );
      }),
      switchMap(() => {
        return this.orders;
      }),
      take(1),
      tap((orders) => {
          this._orders.next(orders.filter((b) => b.id !== id));
      })
    );
  }
}
