import { Component, OnInit, OnDestroy } from "@angular/core";

import { Platform } from "@ionic/angular";
import { SplashScreen } from "@ionic-native/splash-screen/ngx";
import { StatusBar } from "@ionic-native/status-bar/ngx";
import { AuthService } from "./services/auth.service";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";

@Component({
  selector: "app-root",
  templateUrl: "app.component.html",
  styleUrls: ["app.component.scss"],
})
export class AppComponent implements OnInit, OnDestroy {
  private authSubAdmin: Subscription;
  private previousAuthStateAdmin = false;

  constructor(
    private platform: Platform,
    private splashScreen: SplashScreen,
    private statusBar: StatusBar,
    private authService: AuthService,
    private router: Router
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
    this.authSubAdmin = this.authService.userIsAuthenticatedAsAdmin.subscribe(
      (isAuth) => {
        if (!isAuth && this.previousAuthStateAdmin !== isAuth) {
          this.router.navigateByUrl("/auth");
        }
        this.previousAuthStateAdmin = isAuth;
      }
    );
  }

  onLogout() {
    this.authService.logout();
    this.router.navigateByUrl("/auth");
  }

  ngOnDestroy() {
    if (this.authSubAdmin) {
      this.authSubAdmin.unsubscribe();
    }
  }
}
