import { Component, OnInit } from "@angular/core";
import {
  FormGroup,
  FormBuilder,
  FormControl,
  Validators,
} from "@angular/forms";
import { AuthService} from "src/app/services/auth.service";
import { AlertController, LoadingController } from "@ionic/angular";
import { AuthPage } from "../auth.page";
import { Router } from "@angular/router";
import { PasswordValidator } from "../password.validator";
import { Observable } from 'rxjs';
import { AppUser } from 'src/app/models/app-user.model';

@Component({
  selector: "app-sign-up",
  templateUrl: "./sign-up.component.html",
  styleUrls: ["./sign-up.component.scss"],
})
export class SignUpComponent implements OnInit {
  validationsForm: FormGroup;
  matchingPasswordsGroup: FormGroup;
  isLoading = false;

  constructor(
    private authService: AuthService,
    public formBuilder: FormBuilder,
    private alertCtrl: AlertController,
    private loadingCtrl: LoadingController,
    private router: Router,
    private authPage: AuthPage
  ) {}

  ngOnInit() {
    this.matchingPasswordsGroup = new FormGroup(
      {
        password: new FormControl(
          "",
          Validators.compose([Validators.minLength(3), Validators.required])
        ),
        confirmPassword: new FormControl("", Validators.required),
      },
      (formGroup: FormGroup) => {
        return PasswordValidator.areEqual(formGroup);
      }
    );

    this.validationsForm = this.formBuilder.group({
      userName: new FormControl(
        "",
        Validators.compose([
          Validators.maxLength(25),
          Validators.minLength(3),
          Validators.required,
        ])
      ),
      matchingPasswords: this.matchingPasswordsGroup,
      email: new FormControl(
        "",
        Validators.compose([
          Validators.required,
          Validators.pattern("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$"),
        ])
      ),
      fullName: new FormControl(
        "",
        Validators.compose([
          Validators.maxLength(25),
          Validators.minLength(3),
          Validators.required,
        ])
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
      { type: "required", message: "Password must be at least 3 characters long." },
      {
        type: "minlength",
        message: "Password must be at least 3 characters long.",
      },
    ],
    confirmPassword: [
      { type: "required", message: "Confirm password is required." },
    ],
    matchingPasswords: [{ type: "areEqual", message: "Password mismatch." }],
    email: [
      { type: "required", message: "Email is required." },
      { type: "pattern", message: "Please enter a valid email." },
    ],
    fullName: [
      { type: "required", message: "Full name is required." },
      {
        type: "minlength",
        message: "Full name must be at least 3 characters long.",
      },
      {
        type: "maxlength",
        message: "Full name cannot be more than 25 characters long.",
      },
    ],
  };

  onSubmit(validationsForm: FormGroup) {
    const userName = validationsForm.value.userName;
    const password = validationsForm.value.matchingPasswords.password;
    const confirmPassword = validationsForm.value.matchingPasswords.confirmPassword;
    const email = validationsForm.value.email;
    const fullName = validationsForm.value.fullName;
    this.isLoading = false;
    let signObs: Observable<AppUser>;

    this.loadingCtrl
      .create({keyboardClose: true, message: 'Logging in ...'})
      .then(loadingEl => {
        loadingEl.present();
        signObs = this.authService.signup(
          userName,
          password,
          confirmPassword,
          email,
          fullName
        );
        signObs.subscribe(
          (resData) => {
            loadingEl.dismiss();
            this.isLoading = false;
               this.router.navigateByUrl('auth');
            validationsForm.reset();
            this.authPage.isLogin = true;
          },
          (errRes) => {
            loadingEl.dismiss();
            const code = errRes.error;
            let message = "Could not sign you up, please try again";
            if (code === "Invalid user details") {
              message = "This username exists already!";
            }
            this.showAlert(message);
          }
        );
      });
  }

  private showAlert(message: string) {
    this.alertCtrl
      .create({
        header: "Authentication failed",
        message: message,
        buttons: ["Ok"],
      })
      .then((alertEl) => alertEl.present());
  }

}
