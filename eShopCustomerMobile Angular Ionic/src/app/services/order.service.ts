import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from './auth.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { tap, switchMap, take } from 'rxjs/operators';
import { Payment } from '../models/payment.model';
import { CartLine } from '../models/cart-line.model';
import { Order, OrderData } from '../models/order.model';

interface OrderCreated {
  user: string;
  orderId: number;
  authCode: string;
  amount: number;
}

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private _orders = new BehaviorSubject<Order[]>([]);
  get orders() {
    return this._orders.asObservable();
  }
  constructor(private http: HttpClient, private authService: AuthService) { }
  fetchOrders() {
    let fetchedUserName: string;
    return this.authService.userName.pipe(
      take(1),
      switchMap((userName) => {
        if (!userName) {
          throw new Error("User not found!");
        }
        fetchedUserName = userName;
        return this.authService.token;
      }),
      take(1),
      switchMap(token => {
        const httpOptions = {
          headers: new HttpHeaders({
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
          }),
        };
        return this.http.get<Order[]>(
          environment.apiEndpoint + "orders/username/" + fetchedUserName,
          httpOptions
        );
      }),
      tap((orders) => {
        this._orders.next(orders);
      })
    );
  }
  addOrder(name: string, address: string, paymentNavigation: Payment, 
    cartLines: CartLine[], total: number) {
    let generatedId: number;
    let shipped = false;
    let newUserName: string;
    let fetchedUserName: string;
    return this.authService.userName.pipe(
      take(1),
      switchMap((userName) => {
        fetchedUserName = userName;
        if (!userName) {
          throw new Error("No user name found!");
        }
        return this.authService.token;
      }),
      take(1),
      switchMap((token) => {
        newUserName = fetchedUserName;
        let newOrderData= new OrderData(
          (name = name),
          (address = address),
          (shipped = shipped),
          total = total,
          (fetchedUserName = fetchedUserName),
          (paymentNavigation = paymentNavigation),
          (cartLines = cartLines),  
        );
        const httpOptions = {
          headers: new HttpHeaders({
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
          }),
        };
        return this.http.post<OrderCreated>(
          environment.apiEndpoint + "orders",
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
          cartLines,
        );
        this._orders.next(orders.concat(newOrder));
      })
    );
  }
  cancelOrder(id: number) {
    return this.authService.token.pipe(
      take(1),
      switchMap(token =>{        
        const httpOptions = {
          headers: new HttpHeaders({
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
          }),
        };
        return this.http.delete(
          environment.apiEndpoint + "orders/" + id,
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
