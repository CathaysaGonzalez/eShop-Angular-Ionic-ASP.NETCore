import { Injectable } from '@angular/core';
import { BehaviorSubject, of } from 'rxjs';
import { AuthService } from './auth.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { take, switchMap, tap } from 'rxjs/operators';
import { Product, ProductData } from '../models/product.model';


@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  private _products = new BehaviorSubject<Product[]>([]);
  private _selectedProducts = new BehaviorSubject<Product[]>([]);

  get products() {
    return this._products.asObservable();
  }

  get selectedProducts() {
    return this._selectedProducts.asObservable();
  }

  constructor(private authService: AuthService, private http: HttpClient) { }
  fetchProducts() {
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
          .get<Product[]>(
            environment.apiEndpoint + "products",
            httpOptions
          )
          .pipe(
            tap((products) => {
              this._products.next(products);
            })
          );
      })
    );
  }

  searchProductsByName(params: string){
    return this.authService.token.pipe(
      take(1),
      switchMap((token) => {
        const httpOptions = {
          headers: new HttpHeaders({
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
          }),
          params: {search: params}
        };
        return this.http
          .get<Product[]>(
            environment.apiEndpoint + "products/search",
            httpOptions
          )
          .pipe(
            tap((selectedProducts) => {
              this._selectedProducts.next(selectedProducts);
            })
          );
      })
    );
  }

  searchProductsByCategory(params: string){
    return this.authService.token.pipe(
      take(1),
      switchMap((token) => {
        const httpOptions = {
          headers: new HttpHeaders({
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
          }),
          params: {category: params}
        };
        return this.http
          .get<Product[]>(
            environment.apiEndpoint + "products/search",
            httpOptions
          )
          .pipe(
            tap((selectedProducts) => {
               this._selectedProducts.next(selectedProducts);
            })
          );
      })
    );
  }
  
  getProduct(id: number) {
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
          .get<Product>(
            environment.apiEndpoint + "products/" + id,
            httpOptions
          )
          .pipe(
            tap((resData) => {
            })
          );
      })
    );
  }
}
