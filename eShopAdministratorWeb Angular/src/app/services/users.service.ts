import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AppUser } from '../models/app-user.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from './auth.service';
import { Order } from '../models/order.model';
import { environment } from 'src/environments/environment';
import { tap, switchMap, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private _users = new BehaviorSubject<AppUser[]>([]);

  get users() {
    return this._users.asObservable();
  }
  constructor(private authService: AuthService, private http: HttpClient) { }
  fetchUsers() {
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
          .get<AppUser[]>(environment.apiEndpoint + "admin", httpOptions)
          .pipe(
            tap((users) => {
              this._users.next(users);
            })
          );
      })
    );
  }

  addUser(
    userName: string,
    password: string,
    confirmPassword: string,
    email: string,
    role: string
  ) {
    let generatedId: string;
    let orders: Order[];
    let generatedNormalizedUserName: string;
    let generatedNormalizedEmail: string;
    let generatedEmailConfirmed: boolean;
    let generatedPasswordHash: string;
    let geneartedSecurityStamp: string;
    let generatedConcurrencyStamp: string;
    let generatedPhoneNumber: string;
    let generatedPhoneNumberConfirmed: boolean;
    let generatedTwoFactorEnabled: boolean;
    let generatedLockoutEnd: Date;
    let generatedLockoutEnabled: boolean;
    let generatedAccessFailedCount: number;
    let generated_token: string;
    let generatedTokenExpirationDate: Date;
    let path: string;
    const newAppUser = new AppUser(
      generatedId,
      userName,
      generatedNormalizedUserName,
      email,
      generatedNormalizedEmail,
      generatedEmailConfirmed,
      generatedPasswordHash,
      geneartedSecurityStamp,
      generatedConcurrencyStamp,
      generatedPhoneNumber,
      generatedPhoneNumberConfirmed,
      generatedTwoFactorEnabled,
      generatedLockoutEnd,
      generatedLockoutEnabled,
      generatedAccessFailedCount,
      role,
      orders,
      generated_token,
      generatedTokenExpirationDate
    );
    return this.authService.token.pipe(
      take(1),
      switchMap((token) => {
        const httpOptions = {
          headers: new HttpHeaders({
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
          }),
        };
        if (role == "Admins") {
          path = "admin/new";
        }
        else {
          path = "admin";
        }
        return this.http
          .post<AppUser>(
            environment.apiEndpoint + path,
            {
              name: userName,
              email: email,
              password: password,
            },
            httpOptions
          )
          .pipe(
            switchMap((resData) => {
                 generatedId = resData.id;
              return this.users;
            }),
            take(1),
            tap((users) => {
              newAppUser.id = generatedId;
              this._users.next(users.concat(newAppUser));
            })
          );
      })
    );
  }

}
