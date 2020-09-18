import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';

export interface IsLogin {
  isLogin: boolean;
}

@Component({
  selector: 'app-auth',
  templateUrl: './auth.page.html',
  styleUrls: ['./auth.page.scss'],
})
export class AuthPage implements OnInit {

  isLogin=true;

  constructor(public formBuilder: FormBuilder) { }

  ngOnInit() {
  }

  onSwitchAuthMode() {
    this.isLogin = !this.isLogin;
  }

}
