import { Component, OnDestroy, OnInit } from "@angular/core";

import { Platform } from "@ionic/angular";
import { SplashScreen } from "@ionic-native/splash-screen/ngx";
import { StatusBar } from "@ionic-native/status-bar/ngx";
import { AuthService } from "./services/auth.service";
import { Router } from "@angular/router";
import { Subscription, BehaviorSubject } from "rxjs";
import { CartService } from './services/cart.service';

@Component({
  selector: "app-root",
  templateUrl: "app.component.html",
  styleUrls: ["app.component.scss"],
})
export class AppComponent implements OnInit, OnDestroy {
  private authSubUser: Subscription;
  private previousAuthStateUser = false;
  cartItemCount: BehaviorSubject<number>;

  constructor(
    private platform: Platform,
    private splashScreen: SplashScreen,
    private statusBar: StatusBar,
    private authService: AuthService,
    private router: Router,
    private cartService: CartService
  ) {
    this.initializeApp();
  }

  initializeApp() {
    this.platform.ready().then(() => {
      this.statusBar.styleDefault();
      this.splashScreen.hide();
    });
  }

  ngOnInit() {
    this.authSubUser = this.authService.userIsAuthenticatedAsCustomer.subscribe(
      (isAuth) => {
        if (!isAuth && this.previousAuthStateUser !== isAuth) {
          this.router.navigateByUrl("/auth");
        }
        this.previousAuthStateUser = isAuth;
      }
    );
    this.cartItemCount = this.cartService.getCartItemCount();
  }

  onLogout() {
    this.authService.logout();
    this.router.navigateByUrl("/auth");
  }

  ngOnDestroy() {
    if (this.authSubUser) {
      this.authSubUser.unsubscribe();
    }
  }
}
