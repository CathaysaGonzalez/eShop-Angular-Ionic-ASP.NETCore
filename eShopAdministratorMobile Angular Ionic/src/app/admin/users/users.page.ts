import { Component, OnInit } from '@angular/core';
import { AppUser } from 'src/app/models/app-user.model';
import { Subscription } from 'rxjs';
import { UserService } from 'src/app/services/user.service';
import { LoadingController } from '@ionic/angular';
import { Router } from '@angular/router';

@Component({
  selector: 'app-users',
  templateUrl: './users.page.html',
  styleUrls: ['./users.page.scss'],
})
export class UsersPage implements OnInit {

  isLoading = false;
  users: AppUser[];
  private usersSub: Subscription;

  constructor(
    private usersService: UserService,
    private loadingCtrl: LoadingController,
    private router: Router
  ) { }

  ngOnInit() {
    this.usersSub = this.usersService.users.subscribe((users) => {
      this.users = users;
    });
  }

  ionViewWillEnter() {
    this.isLoading = true;
    this.usersService.fetchUsers().subscribe((users) => {
      this.isLoading = false;
    });
  }

  ngOnDestroy() {
    if (this.usersSub) {
      this.usersSub.unsubscribe();
    }
  }  

}
