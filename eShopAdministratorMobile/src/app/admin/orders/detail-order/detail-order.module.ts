import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { DetailOrderPageRoutingModule } from './detail-order-routing.module';

import { DetailOrderPage } from './detail-order.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    DetailOrderPageRoutingModule,
    ReactiveFormsModule
  ],
  declarations: [DetailOrderPage]
})
export class DetailOrderPageModule {}
