import { Injectable, OnDestroy } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map, tap, switchMap } from "rxjs/operators";
import { HttpClient } from "@angular/common/http";
import { environment } from 'src/environments/environment';
import { AppUser } from '../models/app-user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnDestroy{
  private _user = new BehaviorSubject<AppUser>(null);
  private activeLogoutTimer: any;

  get userIsAuthenticatedAsAdmin(){
    return this._user.asObservable().pipe(
      map((user) => {
        if (user) {
          if (user.role == "Admins") {
            return true;
          } else {
            return false;
          }
        } else {
          return false;
        }
      })
    );
  }

  get userName() {
    return this._user.asObservable().pipe(
      map((user) => {
        if (user) {
          return user.userName;
        } else {
          return null;
        }
      })
    );
  }

  get token() {
    return this._user.asObservable().pipe(
      map((user) => {
        if (user) {
          return user.token;
        } else {
          return null;
        }
      })
    );
  }

  constructor(private http: HttpClient) { }

  roleMatch(userRole) {
    if (userRole == "Users") {
      return true;
    } else {
      return false;
    }
  }

  login(userName: string, password: string) {
    return this.http
      .post<AppUser>(environment.apiEndpoint + "account/token", {
        userName: userName,
        password: password,
      })
      .pipe(
        tap(
          this.setUserData.bind(this)
        )
      );
  }

  private setUserData(userData: AppUser) {
    const tokenExpirationDate = new Date(
      new Date().getTime() + +userData.tokenExpirationDate * 1000 * 60
    );
    const user = new AppUser(
      userData.id,
      userData.userName,
      userData.normalizedUserName,
      userData.email,
      userData.normalizedEmail,
      userData.emailConfirmed,
      userData.passwordHash,
      userData.securityStamp,
      userData.concurrencyStamp,
      userData.phoneNumber,
      userData.phoneNumberConfirmed,
      userData.twoFactorEnabled,
      userData.lockoutEnd,
      userData.lockoutEnabled,
      userData.accessFailedCount,
      userData.role,
      userData.orders,
      userData.token,
      tokenExpirationDate,
    );
    this._user.next(user);
    this.storeAuthData(
      userData.id,
      userData.userName,
      userData.normalizedUserName,
      userData.email,
      userData.normalizedEmail,
      userData.emailConfirmed.toString(),
      userData.passwordHash,
      userData.securityStamp,
      userData.concurrencyStamp,
      userData.phoneNumber,
      userData.phoneNumberConfirmed.toString(),
      userData.twoFactorEnabled.toString(),
      null,
      userData.lockoutEnabled.toString(),
      userData.accessFailedCount.toString(),
      userData.role,
      null,
      userData.token,
      tokenExpirationDate.toISOString(),
    );
  }

  private storeAuthData(
    id: string,
    userName: string,
    normalizedUserName: string,
    email: string,
    normalizedEmail: string,
    emailConfirmed: string,
    passwordHash: string,
    securityStamp: string,
    concurrencyStamp: string,
    phoneNumber: string,
    phoneNumberConfirmed: string,
    twoFactorEnabled: string,
    lockoutEnd: string,
    lockoutEnabled: string,
    accessFailedCount: string,
    role: string,
    orders: string,
    token: string,
    tokenExpirationDate: string,
   ) {
    const data = JSON.stringify({
      id: id,
      userName: userName,
      normalizedUserName: normalizedUserName,
      email: email,
      normalizedEmail: normalizedEmail,
      emailConfirmed: emailConfirmed,
      passwordHash: passwordHash,
      securityStamp: securityStamp,
      concurrencyStamp: concurrencyStamp,
      phoneNumber: phoneNumber,
      phoneNumberConfirmed: phoneNumberConfirmed,
      twoFactorEnabled: twoFactorEnabled,
      lockoutEnd: lockoutEnd,
      lockoutEnabled: lockoutEnabled,
      accessFailedCount: accessFailedCount,
      role: role,
      orders: orders,
      token: token,
      tokenExpirationDate: tokenExpirationDate,
    });
    localStorage.setItem('authData', data);
  }

  logout() {
    if (this.activeLogoutTimer) {
      clearTimeout(this.activeLogoutTimer);
    }
    this._user.next(null);
    localStorage.removeItem('authData');
  }

  //Automatically logout when time expires
  private autoLogout(duration: number) {
    if (this.activeLogoutTimer) {
      clearTimeout(this.activeLogoutTimer);
    }
    this.activeLogoutTimer = setTimeout(() => {
      this.logout();
    }, duration);
  }

  ngOnDestroy() {
    if (this.activeLogoutTimer) {
      clearTimeout(this.activeLogoutTimer);
    }
  }
}
