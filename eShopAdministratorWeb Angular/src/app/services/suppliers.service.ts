import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Supplier } from '../models/supplier.model';
import { AuthService } from './auth.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { take, switchMap, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SuppliersService {
  private _suppliers = new BehaviorSubject<Supplier[]>([]);

  get suppliers() {
    return this._suppliers.asObservable();
  }

  constructor(private authService: AuthService, private http: HttpClient) { }

  fetchSuppliers() {
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
          .get<Supplier[]>(
            environment.apiEndpoint + "supplier",
            httpOptions
          )
          .pipe(
            tap((suppliers) => {
              this._suppliers.next(suppliers);
            })
          );
      })
    );
  }
}
