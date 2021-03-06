import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { take, switchMap, tap } from 'rxjs/operators';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Category } from '../models/category.model';
import { AuthService } from './auth.service';


@Injectable({
  providedIn: 'root'
})
export class CategoriesService {
  private _categories = new BehaviorSubject<Category[]>([]);

  get categories() {
    return this._categories.asObservable();
  }

  constructor(private authService: AuthService, private http: HttpClient) { }

  fetchCategories() {
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
          .get<Category[]>(
            environment.apiEndpoint + "category",
            httpOptions
          )
          .pipe(
            tap((categories) => {
              this._categories.next(categories);
            })
          );
      })
    );
  }
}
