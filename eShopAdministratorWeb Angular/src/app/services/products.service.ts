import { Injectable } from '@angular/core';
import { switchMap, take, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Product, ProductData } from '../models/product.model';
import { AuthService } from './auth.service';
import { BehaviorSubject, of } from 'rxjs';

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
            'Content-Type': 'application/json',
            Authorization: 'Bearer ' + token,
          }),
        };
        return this.http
          .get<Product[]>(
            environment.apiEndpoint + 'products',
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

  searchProductsByName(params: string) {
    return this.authService.token.pipe(
      take(1),
      switchMap((token) => {
        const httpOptions = {
          headers: new HttpHeaders({
            'Content-Type': 'application/json',
            Authorization: 'Bearer ' + token,
          }),
          params: { search: params },
        };
        return this.http
          .get<Product[]>(environment.apiEndpoint + 'products/search', httpOptions)
          .pipe(
            tap((selectedProducts) => {
              this._selectedProducts.next(selectedProducts);
            })
          );
      })
    );
  }

  searchProductsByCategory(params: string) {
    return this.authService.token.pipe(
      take(1),
      switchMap((token) => {
        const httpOptions = {
          headers: new HttpHeaders({
            'Content-Type': 'application/json',
            Authorization: 'Bearer ' + token,
          }),
          params: { category: params },
        };
        return this.http
          .get<Product[]>(environment.apiEndpoint + 'products/search', httpOptions)
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
            'Content-Type': 'application/json',
            Authorization: 'Bearer ' + token,
          }),
        };
        return this.http
          .get<Product>(environment.apiEndpoint + 'products/' + id, httpOptions)
          .pipe(
            tap((resData) => {
            })
          );
      })
    );
  }

  addProduct(
    name: string,
    description: string,
    price: number,
    isFeatured: boolean,
    unitsInStock: number,
    modelNumber: string,
    modelName: string,
    currentPrice: number,
    productImage: string,
    productImageLarge: string,
    productImageThumb: string,
    supplierId: number,
    categoryId: number
  ) {
    let generatedId: number;
    //newProductWithoutId is needed because I am not able to pass primary key
    const newProductWithoutId = new ProductData(
      name,
      description,
      price,
      unitsInStock,
      modelNumber,
      modelName,
      productImage,
      productImageLarge,
      productImageThumb,
      isFeatured,
      currentPrice,
      supplierId,
      categoryId,
    );
    const newProduct = new Product(
      generatedId,
      name,
      description,
      price,
      unitsInStock,
      modelNumber,
      modelName,
      productImage,
      productImageLarge,
      productImageThumb,
      isFeatured,
      currentPrice,
      supplierId,
      categoryId
    );
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
          .post<{ id: number }>(
            environment.apiEndpoint + 'products',
            newProductWithoutId,
            httpOptions
          )
          .pipe(
            switchMap((resData) => {
              generatedId = resData.id;
              return this.products;
            }),
            take(1),
            tap((products) => {
              newProduct.id = generatedId;
              this._products.next(products.concat(newProduct));
            })
          );
      })
    );
  }

  updateProduct(
    id: number,
    name: string,
    description: string,
    price: number,
    supplierId: number,
    categoryId: number,
    unitsInStock: number,
    modelNumber: string,
    modelName: string,
    currentPrice: number,
    isFeatured: boolean,
    image: string
  ) {
    let updatedProducts: Product[];
    let fetchedToken: string;
    return this.authService.token.pipe(
      take(1),
      switchMap((token) => {
        fetchedToken = token;
        return this.products;
      }),
      take(1),
      switchMap((products) => {
        if (!products || products.length <= 0) {
          return this.fetchProducts();
        } else {
          return of(products);
        }
      }),
      switchMap((products) => {
        const updatedProductIndex = products.findIndex((p) => p.id === id);
        updatedProducts = [...products];
        const oldProduct = updatedProducts[updatedProductIndex];
        updatedProducts[updatedProductIndex] = new Product(
          oldProduct.id,
          name,
          description,
          price,
          unitsInStock,
          modelNumber,
          modelName,
          image,
          image,
          image,
          isFeatured,
          currentPrice,
          supplierId,
          categoryId
        );
        const httpOptions = {
          headers: new HttpHeaders({
            'Content-Type': 'application/json',
            Authorization: 'Bearer ' + fetchedToken,
          }),
        };
        return this.http.put(
          environment.apiEndpoint + 'products/' + id,
          {...updatedProducts[updatedProductIndex]},
          httpOptions
        );
      }),
      tap(() => {
        this._products.next(updatedProducts);
      })
    );
  }

  cancelProduct(id: number) {
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
          environment.apiEndpoint + 'products/' + id,
          httpOptions
        );
      }),
      switchMap(() => {
        return this.products;
      }),
      take(1),
      tap((products) => {
        this._products.next(products.filter((b) => b.id !== id));
      })
    );
  }

}
