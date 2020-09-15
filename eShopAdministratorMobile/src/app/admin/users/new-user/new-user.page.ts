import { Component, OnInit } from "@angular/core";
import {
  FormGroup,
  FormBuilder,
  FormControl,
  Validators,
} from "@angular/forms";
import { AlertController, LoadingController } from "@ionic/angular";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { Order } from "src/app/models/order.model";
import { UserService } from "src/app/services/user.service";
import { AppUser } from "src/app/models/app-user.model";
import { PasswordValidator } from "../password.validator";

@Component({
  selector: "app-new-user",
  templateUrl: "./new-user.page.html",
  styleUrls: ["./new-user.page.scss"],
})
export class NewUserPage implements OnInit {
  form: FormGroup;
  matchingPasswordsGroup: FormGroup;
  isLoading = false;

  constructor(
    private userService: UserService,
    public formBuilder: FormBuilder,
    private alertCtrl: AlertController,
    private loadingCtrl: LoadingController,
    private router: Router
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

    this.form = this.formBuilder.group({
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
      role: new FormControl("", Validators.compose([Validators.required])),
    });
  }

  validationMessages = {
    userName: [
      { type: "required", message: "El nombre de usuario es obligatorio." },
      {
        type: "minlength",
        message: "El nombre de usuario debe tener al menos 3 carácteres.",
      },
      {
        type: "maxlength",
        message: "El nombre de usuario ha de tener un máximo de 25 carácteres.",
      },
    ],
    password: [
      { type: "required", message: "El password es obligatorio." },
      {
        type: "minlength",
        message: "El password ha de tener al menos 3 carácteres.",
      },
    ],
    confirmPassword: [
      { type: "required", message: "Confirmar el password es obligatorio." },
    ],
    matchingPasswords: [{ type: "areEqual", message: "Los passwords no coinciden." }],
    email: [
      { type: "required", message: "El email es obligatorio." },
      { type: "pattern", message: "Por favor entre un email válido." },
    ],
  };

  onSubmit(validationsForm: FormGroup) {
    const userName = validationsForm.value.userName;
    const password = validationsForm.value.matchingPasswords.password;
    const confirmPassword =
      validationsForm.value.matchingPasswords.confirmPassword;
    const email = validationsForm.value.email;
    this.isLoading = false;
    const role = validationsForm.value.role;

    let userObs: Observable<AppUser[]>;

    this.loadingCtrl
      .create({ keyboardClose: true, message: "Logging in ..." })
      .then((loadingEl) => {
        loadingEl.present();
        userObs = this.userService.addUser(
          userName,
          password,
          confirmPassword,
          email,
          role
        );
        userObs.subscribe(
          (resData) => {
            loadingEl.dismiss();
            this.isLoading = false;
            this.router.navigateByUrl("/admin/tabs/users");
            validationsForm.reset();
          },
          (errRes) => {
            loadingEl.dismiss();
            const code = errRes.error;
            let message = "No se ha podido crear el usuario. Inténtelo más tarde.";
            if (code === "Invalid user details") {
              message = "El nombre de usuario introducido ya esta en uso";
            }
            this.showAlert(message);
          }
        );
      });
  }

  private showAlert(message: string) {
    this.alertCtrl
      .create({
        header: "Error al crear usuario",
        message: message,
        buttons: ["Okay"],
      })
      .then(
        (alertEl) =>{ 
          alertEl.present();
          this.form.reset();
      })
    }
}
