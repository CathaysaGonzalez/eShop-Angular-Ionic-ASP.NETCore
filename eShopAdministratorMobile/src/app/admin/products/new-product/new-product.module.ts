import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { IonicModule } from "@ionic/angular";

import { NewProductPageRoutingModule } from "./new-product-routing.module";

import { NewProductPage } from "./new-product.page";
import { AppModule } from "src/app/app.module";
import { ProductsPageModule } from "../products.module";
import { SharedPageModule } from 'src/app/shared/shared.module';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    NewProductPageRoutingModule,
    ReactiveFormsModule,
    SharedPageModule
  ],
  declarations: [NewProductPage],
})
export class NewProductPageModule {}
