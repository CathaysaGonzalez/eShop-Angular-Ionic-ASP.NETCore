import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, FormControl, Validators } from "@angular/forms";
import { AuthService} from "src/app/services/auth.service";
import { LoadingController, AlertController } from "@ionic/angular";
import { Router } from "@angular/router";
import { Observable } from 'rxjs';
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
      { type: "required", message: "El nombre de usuario es obligatorio." },
      {
        type: "minlength",
        message: "El nombre de usuario ha de tener un mínimo de 3 carácteres.",
      },
      {
        type: "maxlength",
        message: "El nombre de usuario no puede exceder de 20 carácteres.",
      },
    ],
    password: [
      { type: "required", message: "El password es obligatorio." },
      {
        type: "minlength",
        message: "El password ha de tener un mínimo de 3 carácteres.",
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
            if (resData.role == "Admins") {
              console.log("user as admins");
              this.router.navigateByUrl("/admin/tabs");
            } 
          },
          (errRes) => {
            loadingEl.dismiss();
            this.isLoading = false;
            const code = errRes.statusText;
            let message = "No se ha podido autentificar. Por favor inténtelo de nuevo.";
            if (code === "Unauthorized") {
              message = "El administrador no esta registrado o password erróneo.";
            }
            this.showAlert(message);
          }
        );
      });
  }

  private showAlert(message: string) {
    this.alertCtrl
      .create({
        header: "Error en autentificación",
        message: message,
        buttons: ["Okay"],
      })
      .then(
        (alertEl) =>{ 
          alertEl.present();
          this.validationsForm.reset();
      })
    }
}
