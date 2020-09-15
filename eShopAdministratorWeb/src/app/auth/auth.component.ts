import { Component, OnInit } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  FormControl,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { Observable } from 'rxjs';
import { AppUser } from '../models/app-user.model';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css'],
})
export class AuthComponent implements OnInit {
  validationsForm: FormGroup;
  public errorMessage: string;

  constructor(
    private authService: AuthService,
    public formBuilder: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.validationsForm = this.formBuilder.group({
      userName: new FormControl(
        '',
        Validators.compose([
          Validators.maxLength(25),
          Validators.minLength(3),
          Validators.required,
        ])
      ),
      password: new FormControl(
        '',
        Validators.compose([Validators.minLength(3), Validators.required])
      ),
    });
  }

  validationMessages = {
    userName: [
      { type: 'required', message: 'Username is required.' },
      {
        type: 'minlength',
        message: 'Username must be at least 3 characters long.',
      },
      {
        type: 'maxlength',
        message: 'Username cannot be more than 25 characters long.',
      },
    ],
    password: [
      { type: 'required', message: 'Password is required.' },
      {
        type: 'minlength',
        message: 'Password must be at least 3 characters long.',
      },
    ],
  };

  onSubmit(validationsForm: FormGroup) {
    const userName = validationsForm.value.userName;
    const password = validationsForm.value.password;
    let signObs: Observable<AppUser>;

    signObs = this.authService.login(userName, password);
    signObs.subscribe(
      (resData) => {
        if (resData.role == 'Admins') {
          this.router.navigateByUrl('/admin/products');
        }
      },
      (errRes) => {
        this.errorMessage = 'Authentication error';
      }
    );
  }
}
