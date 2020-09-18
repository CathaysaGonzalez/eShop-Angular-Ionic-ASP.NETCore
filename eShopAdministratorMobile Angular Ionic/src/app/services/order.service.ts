import { Injectable } from "@angular/core";
import { BehaviorSubject, of } from "rxjs";
import { AuthService } from "./auth.service";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { tap, switchMap, take } from "rxjs/operators";
import { Payment } from "../models/payment.model";
import { CartLine } from "../models/cart-line.model";
import { Order } from "../models/order.model";
import { AppUser } from "../models/app-user.model";

@Injectable({
  providedIn: "root",
})
export class OrderService {
  private _orders = new BehaviorSubject<Order[]>([]);

  get orders() {
    return this._orders.asObservable();
  }

  constructor(private http: HttpClient, private authService: AuthService) {}

  getOrder(id: number) {
    return this.authService.token.pipe(
      take(1),
      switchMap((token) => {
        const httpOptions = {
          headers: new HttpHeaders({
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
          }),
        };
        return this.http
          .get<Order>(
            environment.apiEndpoint + "orders/details/" + id,
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
        const updatedOrderIndex =orders.findIndex((o) => o.id === id);
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
            "Content-Type": "application/json",
            Authorization: "Bearer " + fetchedToken,
          }),
        };
        return this.http.put(
          environment.apiEndpoint + "orders/" + id,
          updatedOrders[updatedOrderIndex],
          httpOptions
        );
      })
    );
  }
  
  fetchOrders() {
    return this.authService.token.pipe(
      take(1),
      switchMap((token) => {
        const httpOptions = {
          headers: new HttpHeaders({
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
          }),
        };
        return this.http.get<Order[]>(
          environment.apiEndpoint + "orders/orders/cartlines",
          httpOptions
        );
      }),
      tap((orders) => {
        this._orders.next(orders);
      })
    );
  }
}
