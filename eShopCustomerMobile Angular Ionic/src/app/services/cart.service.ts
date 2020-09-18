import { Injectable } from '@angular/core';
import { CartRecord } from '../models/cart-record.model';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from './auth.service';
import { Product } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private _cart = new BehaviorSubject<CartRecord[]>(null);
  private _cartItemCount = new BehaviorSubject(0);
  get cart() {
    return this._cart.asObservable();
  }
  constructor() { }
  getCartItemCount() {
    return this._cartItemCount;
  }
  addProduct(product: CartRecord) {
    let listCart = this._cart.getValue();
    if (listCart) {
      let objIndex = listCart.findIndex((obj) => obj.id == product.id);
      if (objIndex != -1) {
        listCart[objIndex].quantity += 1;
      } else {
        listCart = [];
        listCart.push(product);
      }
      this._cart.next(listCart);
      this._cartItemCount.next(this._cartItemCount.value + 1);
    } else {
      listCart = [];
      listCart.push(product);
      this._cart.next(listCart);
      this._cartItemCount.next(this._cartItemCount.value + 1);
    }
  }
  addProductFromList(productDetails: Product) {
    let listCart = this._cart.getValue();
    if (listCart) {
      let objIndex = listCart.findIndex((obj => obj.id == productDetails.id));
      if (objIndex != -1) {
        listCart[objIndex].quantity += 1;
        this._cart.next(listCart);
      } else {
        const newCartRecord = new CartRecord(
          productDetails.id,
          productDetails.name,
          productDetails.description,
          productDetails.price,
          productDetails.unitsInStock,
          productDetails.modelNumber,
          productDetails.modelName,
          productDetails.productImage,
          productDetails.productImageLarge,
          productDetails.productImageThumb,
          productDetails.isFeatured,
          productDetails.currentPrice,
          productDetails.supplierId,
          productDetails.categoryId,
          productDetails.ratings,
          productDetails.supplier,
          productDetails.categoryNavigation,
          1
        );
        this._cart.next(listCart.concat(newCartRecord));
      }
    } else {
      listCart = [];
      const newCartRecord = new CartRecord(
        productDetails.id,
        productDetails.name,
        productDetails.description,
        productDetails.price,
        productDetails.unitsInStock,
        productDetails.modelNumber,
        productDetails.modelName,
        productDetails.productImage,
        productDetails.productImageLarge,
        productDetails.productImageThumb,
        productDetails.isFeatured,
        productDetails.currentPrice,
        productDetails.supplierId,
        productDetails.categoryId,
        productDetails.ratings,
        productDetails.supplier,
        productDetails.categoryNavigation,
        1
      );
      this._cart.next(listCart.concat(newCartRecord));
    }
    this._cartItemCount.next(this._cartItemCount.value + 1);
  }
  decreaseProduct(product: CartRecord) {
    let listCart = this._cart.getValue();
    let objIndex = listCart.findIndex((obj) => obj.id == product.id);
    listCart[objIndex].quantity -= 1;
    if (listCart[objIndex].quantity == 0) {
      listCart.splice(objIndex, 1);
    }
    this._cart.next(listCart);
    this._cartItemCount.next(this._cartItemCount.value - 1);

  }
  removeProduct(product: CartRecord) {
    let listCart = this._cart.getValue();
    let objIndex = listCart.findIndex((obj) => obj.id == product.id);
    if (objIndex != -1) {
      listCart.splice(objIndex, 1);
    }
    this._cart.next(listCart);
    this._cartItemCount.next(this._cartItemCount.value - product.quantity);
  }
  removeProducts(){
    let listCart = this._cart.getValue();
    listCart.splice(0, listCart.length);  
    this._cart.next(listCart);
    this._cartItemCount.next(0);
  }
}
