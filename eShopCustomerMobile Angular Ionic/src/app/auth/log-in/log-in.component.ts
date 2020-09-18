import { Component, OnInit } from "@angular/core";
import {
  FormGroup,
  FormBuilder,
  FormControl,
  Validators,
} from "@angular/forms";
import { AuthService } from "src/app/services/auth.service";
import { LoadingController, AlertController } from "@ionic/angular";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { AppUser } from 'src/app/models/app-user.model';

@Component({
  selector: "app-log-in",
  templateUrl: "./log-in.component.html",
  styleUrls: ["./log-in.component.scss"],
})
export class LogInComponent implements OnInit {
  validationsForm: FormGroup;
  isLoading = true;

  constructor(
    private authService: AuthService,
    public formBuilder: FormBuilder,
    private loadingCtrl: LoadingController,
    private alertCtrl: AlertController,
    private router: Router
  ) {}

  ngOnInit() {
    this.validationsForm = this.formBuilder.group({
      userName: new FormControl(
        "",
        Validators.compose([
          Validators.maxLength(25),
          Validators.minLength(3),
          Validators.required,
        ])
      ),
      password: new FormControl(
        "",
        Validators.compose([Validators.minLength(3), Validators.required])
      ),
    });
  }

  validationMessages = {
    userName: [
      { type: "required", message: "Username is required." },
      {
        type: "minlength",
        message: "Username must be at least 3 characters long.",
      },
      {
        type: "maxlength",
        message: "Username cannot be more than 25 characters long.",
      },
    ],
    password: [
      { type: "required", message: "Password is required." },
      {
        type: "minlength",
        message: "Password must be at least 3 characters long.",
      },
    ],
  };

  onSubmit(validationsForm: FormGroup) {

    const userName = validationsForm.value.userName;
    const password = validationsForm.value.password;
    this.isLoading = true;

    this.loadingCtrl
      .create({ keyboardClose: true, message: "Logging in ..." })
      .then((loadingEl) => {
        loadingEl.present();
        let signObs: Observable<AppUser>;
        signObs = this.authService.login(userName, password);
        signObs.subscribe(
          (resData) => {
            this.isLoading = false;
            loadingEl.dismiss();
            validationsForm.reset();
            if (resData.role == "Users") {
              this.router.navigateByUrl("/store/tabs/categories");
            } 
            else {
              this.router.navigateByUrl("/auth");
            }
          },
          (errRes) => {
            loadingEl.dismiss();
            this.isLoading = false;
            const code = errRes.statusText;
            let message = "Could not log in, please try again.";
             if (code === "Unauthorized") {
              message = "User name could not be found or wrong password.";
            }
            this.showAlert(message);
          }
        );
      });
  }

  private showAlert(message: string) {
    this.alertCtrl
      .create({
        header: "Authentication failed.",
        message: message,
        buttons: ["Ok"],
      })
      .then((alertEl) => alertEl.present());
  }
}
